using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BoatType { Small, Fat};

[System.Serializable]
public class SmallBoat
{
    public float Speed = 4;
    public int HoomansInStock = 3;
}
[System.Serializable]
public class FatBoat
{
    public int SmallBoatsInStock;
    public int HoomansInStock;
    public Transform[] Cannons;
    public float CannonForce = 250;
    public float FireRate = 3;
}
public class EnemyBoat : MonoBehaviour {
    public BoatType BType;
    public float Health;
    public float WaterLevel = -35;
    public GameObject HoomanPrefab;
    public FatBoat FatProperties;
    public SmallBoat SmallProperties;
    float TimeToFire = 0;
    
	// Use this for initialization
	void Start () {
		
	}
	public void FireCannon()
    {
        //get random cannon
        Transform cnn = FatProperties.Cannons[Random.Range(0, FatProperties.Cannons.Length - 1)];
        //fire that cannon
        Transform Target = GameObject.Find("EnemyTargetRange").transform;
        //get random position between these points
        var p0 = Target.GetChild(0).position;
        var p1 = Target.GetChild(1).position;
        Vector3 v = p1 - p0;
        Vector3 target_position = p0 + Random.value * v;
        cnn.LookAt(target_position);
        //fire the cannon
        var g = Instantiate(HoomanPrefab);
        g.GetComponent<NavMeshAgent>().enabled = false;
        g.GetComponent<Health>().type = HealthType.Yeeter;
        g.GetComponent<AI>().r.type = RoleType.Yeeter;
        g.GetComponent<AI>().enabled = false;
        g.transform.position = cnn.GetChild(0).position;
        var rb = g.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        var o = g.GetComponent<YeetedAI>();
        o.BeingThrown = true;
        o.Side = 2;
        Vector3 toTarget = (target_position - cnn.GetChild(0).position);
        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = FatProperties.CannonForce * FatProperties.CannonForce + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
        float T = T_lowEnergy; // choose T_max, T_min, or some T in-between like T_lowEnergy
        // Convert from time-to-hit to a launch velocity:
        Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;
        // Apply the calculated velocity (do not use force, acceleration, or impulse modes)
        rb.AddForce(velocity, ForceMode.VelocityChange);
        //rb.AddForce((target_position - cnn.GetChild(0).position) * FatProperties.CannonForce);
    }
	// Update is called once per frame
	void Update () {
        TimeToFire += Time.deltaTime;
        if(TimeToFire >= FatProperties.FireRate)
        {
            TimeToFire = 0;
            FireCannon();
        }
	}
}
