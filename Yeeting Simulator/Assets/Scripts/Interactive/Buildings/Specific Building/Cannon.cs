using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Container {

    /*Used initially to test containers, unused in the latest version
     Building - Launches objects that are placed in it at a target when a button is pressed*/

    //Power modifier for launching objects
    public int launchForce;
    public Transform target;

    void Start()
    {
        target = transform.parent.GetChild(1);
    }
    
    //Called by button - launches all held objects sequentially
    public void Launch()
    {
        int total = held.Count;
        for (int i = 0; i < total; i++)
        {
            Fire(0, target.position, launchForce);
        }
    }

    //Removes a contained object, and launches it
    public void Fire(int place, Vector3 target, float power)
    {
        //Debug.Log("Fire: " + Time.time);
        GameObject obj = held[place];

        RemObject(place);
        if (!FireObj(obj, target, power))
            Debug.Log("Something went wrong here?");
    }

    //Calculate the amount of force needed to hit a target point
    public bool FireObj(GameObject obj, Vector3 target, float force)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 toTarget = target - transform.position;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = force * force + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if (discriminant < 0)
        {
            // Target is too far away to hit at this speed.
            // Abort, or fire at max speed in its general direction?
            return false;
        }

        //float discRoot = Mathf.Sqrt(discriminant);

        // Highest shot with the given max speed:
        //float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);
        // Most direct shot with the given max speed:
        //float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
        // Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));

        float T = T_lowEnergy; // choose T_max, T_min, or some T in-between like T_lowEnergy

        // Convert from time-to-hit to a launch velocity:
        Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;

        // Apply the calculated velocity (do not use force, acceleration, or impulse modes)
        rb.AddForce(velocity, ForceMode.VelocityChange);
        return true;
    }

    //Handle objects being added/removed - cannon specific
    public override void AddObject(GameObject obj)
    {
        //Debug.Log("add" + Time.time);
        obj.GetComponent<BasicThrown>().ChangeParent(transform.parent);
        obj.transform.position = placedPos + transform.position;
        obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        obj.transform.eulerAngles = placedAng;
        obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        held.Add(obj);
    }
    public override void RemObject(int place)
    {
        //Debug.Log("rem" + Time.time);
        GameObject obj = held[place];
        obj.transform.localScale = obj.GetComponent<BasicThrown>().normalScale;
        obj.GetComponent<BasicThrown>().ChangeParent();
        held.RemoveAt(place);
    }
    public override bool RemObject(string type, int amount)
    {
        throw new System.NotImplementedException();
    }
}
