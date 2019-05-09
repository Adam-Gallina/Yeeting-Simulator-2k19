using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AIMaster : MonoBehaviour
{
    public List<AI> AllAI = new List<AI>();
    public Material ContructionMat;
    public Material LumberMat;
    public Material MinerMat;
    public Material HunterMat;
    ResourceManagment r;
    public Vector3 MinBounds;
    public Vector3 MaxBounds;
    // Start is called before the first frame update
    void Start()
    {
        r = FindObjectOfType<ResourceManagment>();
        AllAI = FindObjectsOfType<AI>().ToList();
        AssignMaterials();
    }
    public void AssignMaterials()
    {
        for (int mi = 0; mi < AllAI.Count; mi++)
        {
            var ai = AllAI[mi].r.type;
            var msh = AllAI[mi].transform.GetChild(1).GetComponent<MeshRenderer>();
            if (ai == RoleType.Builder) { msh.material = ContructionMat; }
            if (ai == RoleType.Lumberjack) { msh.material = LumberMat; }
            if (ai == RoleType.Miner) { msh.material = MinerMat; }
            if (ai == RoleType.Hunter) { msh.material = HunterMat; }
        }
    }
    public bool IsResourceEmpty(Resource resource)
    {
        bool empty = true;
        for(int iri = 0; iri < AllAI.Count; iri++)
        {
            if(resource.transform == AllAI[iri].d.CurrentDestination.Location)
            {
                empty = false;
            }
        }
        return empty;
    }
    public bool IsDestinationEmpty(Transform destination)
    {
        bool empty = true;
        for (int iri = 0; iri < AllAI.Count; iri++)
        {
            if (destination == AllAI[iri].d.CurrentDestination.Location)
            {
                empty = false;
            }
        }
        return empty;
    }
    public int GetTargetAmount(Transform destination)
    {
        int amt = 0;
        for (int iri = 0; iri < AllAI.Count; iri++)
        {
            //Debug.Log(amt + " ... " + "Destination: " + destination + " ...  AI Destination: " + AllAI[iri].d.CurrentDestination.Location);
            if (destination != null && AllAI[iri].d.CurrentDestination.Location != null && destination.transform == AllAI[iri].d.CurrentDestination.Location.transform)
            {
                amt += 1;
            }
        }
        //Debug.Log(amt);
        return amt;
    }
    public Transform GetEntryPoint(Transform[] transforms)
    {
        Transform lowestPoint = transforms[0]; //the point with the least npcs trying to get to it
        int lowestAmt = 999; //corresponds to lowestPoint
        for(int iti = 0; iti < transforms.Length; iti++)
        {
            int targ = GetTargetAmount(transforms[iti]);
            if (targ < lowestAmt)
            {
                lowestAmt = targ;
                lowestPoint = transforms[iti];
            }
        }
        return lowestPoint;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
