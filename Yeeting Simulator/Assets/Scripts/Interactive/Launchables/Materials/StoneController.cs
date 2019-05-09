using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : BasicThrown {

    private GameObject volcano;

	void Start ()
    {
        volcano = GameObject.Find("Volcano").transform.GetChild(0).gameObject;

        transform.position = volcano.transform.position;

        Vector2 randDir = Random.insideUnitCircle * 15;
        Vector3 dir = new Vector3(randDir.x, 20, randDir.y);
        rb.velocity = dir;
	}
}
