using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : BasicThrown {

    protected List<GameObject> stored = new List<GameObject>();
    public int maxStored;

    protected float velocity;
    public float breakVelocity;
    protected Vector3 lastPos;
    protected Vector3 currPos;

    void Start ()
    {
        placeable = true;

        lastPos = transform.position;
	}
	
    void FixedUpdate()
    {
        currPos = transform.position;

        velocity = Vector3.Distance(currPos, lastPos);

        if (velocity >= breakVelocity)
            Debug.Log("Crash");

        lastPos = currPos;
    }

	void Update () {
		
	}
}
