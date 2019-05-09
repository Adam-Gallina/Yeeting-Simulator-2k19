using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class YeetedAI : MonoBehaviour
{
    [Range(1, 2)]
    public int Side = 1;
    public Weapon CurrentWeapon;
    [SerializeField]
    Transform Face;
    public YeetedAI CurrentEnemy;
    public List<YeetedAI> Enemies = new List<YeetedAI>();
    public float Range = 16;
    public LayerMask lm;
    AI self;
    public bool BeingThrown;
    float timeToCheck = 2;
    float WeaponCooldown;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<AI>();
        if(Side == 2)
        {
            var m = transform.GetChild(0).GetComponent<MeshRenderer>().material;
            m.color = Color.red;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = m;
        }
        EquipWeapon(0);
    }
    public void EquipWeapon(int index)
    {
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);
        CurrentWeapon = transform.GetChild(2).GetChild(0).GetChild(index).GetComponent<Weapon>();
        CurrentWeapon.gameObject.SetActive(true);
    }
    public void CheckView()
    {
        UpdateEnemies();
        for(int i = 0; i < Enemies.Count; i++)
        {
            if(Vector3.Distance(transform.position, Enemies[i].transform.position) <= 300 && self.enabled)
            {
                RaycastHit hit;
                if(Physics.Raycast(Face.transform.position, Enemies[i].transform.position - transform.position, out hit, Range, lm))
                {
                    if(hit.transform.root.GetComponent<YeetedAI>())
                        if(hit.transform.root.GetComponent<YeetedAI>().Side != Side)
                        {
                            //got an enemy. attack it
                            GoAttack(hit.transform.root.GetComponent<YeetedAI>());
                        }
                }
            }
        }
    }
    public void GoAttack(YeetedAI enemy)
    {
        self.SetDestination(enemy.transform, false);
        CurrentEnemy = enemy;
    }
    public void StrikeAttack(YeetedAI enemy)
    {
        WeaponCooldown = 0;
        CurrentWeapon.transform.parent.parent.GetComponent<Animator>().Play(CurrentWeapon.AttackName);
        enemy.GetComponent<Health>().TakeDamage(gameObject, CurrentWeapon.Damage);
    }
    public void UpdateEnemies()
    {
        var ai = FindObjectsOfType<YeetedAI>();
        Enemies = new List<YeetedAI>();
        for(int i = 0; i < ai.Length; i++)
        {
            if (ai[i].Side != Side)
                Enemies.Add(ai[i]);
        }
    }
    float TimeInAir = 0;
    public void OnCollisionEnter(Collision c)
    {
        if (TimeInAir > 1)
        {
            BeingThrown = false;
            transform.localEulerAngles = Vector3.zero;
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            GetComponent<AI>().enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(BeingThrown)
        {
            TimeInAir += Time.deltaTime;
            return;
        }
        if (self.r.type != RoleType.Hunter && self.r.type != RoleType.Yeeter)
            return;
        WeaponCooldown += Time.deltaTime;
        timeToCheck -= Time.deltaTime;
        if(timeToCheck <= 0 && CurrentEnemy != null)
        {
            timeToCheck = 1;
            CheckView();
        }
        if(self.d.CurrentDestination.Location != null)
        if(self.d.CurrentDestination.Location.GetComponent<YeetedAI>())
        {
            var t = self.d.CurrentDestination.Location.GetComponent<YeetedAI>();
            if(t.Side != Side)
            {
                if(Vector3.Distance(t.transform.position, transform.position) < CurrentWeapon.Range)
                if(WeaponCooldown >= CurrentWeapon.Cooldown)
                    StrikeAttack(t);
            }
        }
    }
}
