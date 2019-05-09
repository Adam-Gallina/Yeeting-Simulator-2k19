using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class Control
{
    public bool Override = false;
    public bool OverrideAuto = false;
}

[System.Serializable]
public class Destination
{
    public Transform Location;
}
[System.Serializable]
public class Destinations
{
    public Destination CurrentDestination;
    public Destination FinalDestination;
    public List<Destination> SubDestinations = new List<Destination>();
    public Vector3 LastPoint;
}
public enum RoleType { None, Farmer, Hunter, Child, Slave, AnimalFarmer, Lumberjack, Miner, Builder, Yeeter, Blacksmith};
[System.Serializable]
public class Role
{
    public RoleType type;
}
[System.Serializable]
public class PublicMarkings
{
    public bool Criminal;
    public bool LawEnforcement;
    public bool Prisoner;
}
[System.Serializable]
public class CarriedResources
{
    public int MaxAmt = 5;
    public int CurrentTotal;
    public int Wood;
    public int Food;
    public int Stone;
}

public class AI : MonoBehaviour
{
    public Destinations d;
    public Control c;
    public Role r;
    public PublicMarkings markings;
    public CarriedResources resources;
    AIMaster master;
    private ResourceManagment rm;
    private NavMeshAgent nav;
    public float JustArrived = 1;
    public AIBank bank;
    RoleType[] roles = { RoleType.Lumberjack, RoleType.Miner, RoleType.Hunter, RoleType.Yeeter, RoleType.None };
    RoleType[] uncommonRoles = { RoleType.Blacksmith, RoleType.Builder };
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Init", .01f);
    }
    public void Init()
    {
        if (GetComponent<YeetedAI>().Side == 2)
        {
            nav = GetComponent<NavMeshAgent>();
            return;
        }

        r.type = roles[Random.Range(0, roles.Length)];
        if (r.type == RoleType.None)
        {
            r.type = RoleType.Builder;

        }
        master = FindObjectOfType<AIMaster>();
        JustArrived = 1;
        rm = FindObjectOfType<ResourceManagment>();
        nav = GetComponent<NavMeshAgent>();
        Invoke("NewDestination", .01f);
        for (int i = 0; i < FindObjectsOfType<AIBank>().Length; i++)
        {
            if (FindObjectsOfType<AIBank>()[i].ID == GetComponent<YeetedAI>().Side)
                bank = FindObjectsOfType<AIBank>()[i];
        }
    }
    public void WanderToPoint()
    {
        if(GetComponent<YeetedAI>().Side == 2)
        {
            GetComponent<YeetedAI>().CheckView();
            GetComponent<YeetedAI>().CurrentEnemy = GetComponent<YeetedAI>().Enemies[Random.Range(0, GetComponent<YeetedAI>().Enemies.Count - 1)];
        }
        var m = FindObjectOfType<AIMaster>().MinBounds;
        var x = FindObjectOfType<AIMaster>().MaxBounds;
        var destination = new Vector3(Random.Range(m.x, x.x), Random.Range(m.y, x.y), Random.Range(m.z, x.z));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 3, NavMesh.AllAreas))
        {
            //Debug.Log(hit.position);
            destination = hit.position;
        }
        var g = new GameObject();
        g.transform.position = destination;
        SetDestination(g.transform, true);
    }
    public void NewDestination()
    {
        nav.enabled = true;
        //Debug.Log("new dest");
        if(r.type == RoleType.Lumberjack)
        {
            //get wood
            GoGetResource(RType.Tree);
            if (d.FinalDestination.Location != null)
                return;
        }
        if (r.type == RoleType.Miner)
        {
            //get stone
            GoGetResource(RType.Stone);
            if (d.FinalDestination.Location != null)
                return;
        }
        if (r.type == RoleType.Hunter)
        {
            //get food
            GoGetResource(RType.Animal);
            if (d.FinalDestination.Location != null)
                return;
        }
        if (d.FinalDestination.Location == null)
            WanderToPoint();
    }
    public void GoGetResource(RType type)
    {
        if(resources.CurrentTotal >= resources.MaxAmt)
        {
            GoToBank();
            return;
        }
        if(rm.GetNearbyR(type, transform.position) != null)
        SetDestination(rm.GetNearbyR(type, transform.position).transform, true);
    }
    public void AddDestination(Transform dest)
    {
        d.SubDestinations.Add(new Destination { Location = dest });
        d.CurrentDestination = d.SubDestinations[d.SubDestinations.Count - 1];
        for (int di = 0; di < d.SubDestinations.Count; di++)
        {
            if (d.SubDestinations[di].Location == null)
            {
                d.SubDestinations.RemoveAt(di);
            }
        }
        JustArrived = 1;
    }
    public void GoToBank()
    {
        Transform targetEntry = master.GetEntryPoint(bank.EntryPoints);
        SetDestination(targetEntry, false);
    }
    public void ArriveAtDestination()
    {
        JustArrived = 1;
        d.LastPoint = new Vector3(0, -1000, 0);
        Transform cd = d.CurrentDestination.Location;
        if(Vector3.Distance(cd.position, transform.position) < 3)
        if(cd.GetComponent<Resource>())
        {
            Resource cr = cd.GetComponent<Resource>();
            if(r.type == RoleType.Lumberjack && cr.ResourceType == RType.Tree || r.type == RoleType.Miner && cr.ResourceType == RType.Stone || r.type == RoleType.Hunter && cr.ResourceType == RType.Animal)
            {
                CollectResource(cr);
            }
        }
        if(cd.GetComponentInParent<AIBank>())
        {
            Debug.Log("Deposit");
            cd.GetComponentInParent<AIBank>().Deposit(this);
        }
        if(cd.gameObject.name == "Build Point" && r.type == RoleType.Builder)
        {
            GetComponent<Construction>().Build();
            d.CurrentDestination.Location = null;
            d.SubDestinations = new List<Destination>();
            d.FinalDestination.Location = null;
        }
        //determine next action
        if(d.SubDestinations.Count > 0 && d.CurrentDestination.Location != d.SubDestinations[d.SubDestinations.Count - 1].Location)
        {
            Debug.Log("Next Action: Next Destination");
            var l = new List<Destination>();
            for(int i = 0; i < d.SubDestinations.Count - 1; i++)
            {
                l.Add(d.SubDestinations[i]);
            }
            d.SubDestinations = l;
            d.CurrentDestination = d.SubDestinations[d.SubDestinations.Count - 1];
        }
        else
        {
            if(!c.OverrideAuto)
            NewDestination();
        }
    }
    
    public void CollectResource(Resource re)
    {
        re.Collect(this);
        rm.GetR();
        resources.CurrentTotal = resources.Stone + resources.Wood + resources.Food;
        if(resources.CurrentTotal >= resources.MaxAmt)
        {
            //deposit 
            GoToBank();
        }
    }
    public void SetDestination(Transform Destination, bool ErasePrevious)
    {
        if(ErasePrevious)
        {
            d.SubDestinations = new List<Destination>();
        }
        for(int di = 0; di < d.SubDestinations.Count; di++)
        {
            if(d.SubDestinations[di].Location == null)
            {
                d.SubDestinations.RemoveAt(di);
            }
        }
        d.FinalDestination.Location = Destination;
        d.SubDestinations.Add(d.FinalDestination);
        d.CurrentDestination = d.SubDestinations[d.SubDestinations.Count - 1];
        JustArrived = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (c.Override)
            return;
        gameObject.name = r.type.ToString();
        if (JustArrived > 0)
            JustArrived -= Time.deltaTime;
        if(d.CurrentDestination.Location != null)
        {
            Vector3 destination = d.CurrentDestination.Location.position;
            if(Vector3.Distance(d.CurrentDestination.Location.position, d.LastPoint) > 0.01)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(d.CurrentDestination.Location.position, out hit, 3, NavMesh.AllAreas))
                {
                    //Debug.Log(hit.position);
                    d.LastPoint = d.CurrentDestination.Location.position;
                   destination = hit.position;
                }
            }
            nav.SetDestination(destination);
        }
        if(!c.OverrideAuto)
        if(d.CurrentDestination.Location == null && JustArrived <= 0)
        {
            NewDestination();
        }
        if(GetComponent<YeetedAI>().Side == 1)
        if (d.CurrentDestination.Location != null && nav.pathStatus == NavMeshPathStatus.PathComplete && JustArrived <= 0)
        {
            //Debug.Log(nav.remainingDistance);
            if(nav.remainingDistance <= .05f || nav.remainingDistance <= 1.5f && d.CurrentDestination.Location.GetComponent<Animal>())
                ArriveAtDestination();
        }
    }
}
