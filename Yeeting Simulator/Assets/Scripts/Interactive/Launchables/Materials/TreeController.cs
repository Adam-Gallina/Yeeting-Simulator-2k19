using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : BasicThrown {

    //Height of the tree
    public float spawnTime;

	void Start ()
    {
        //Place the tree underground
        transform.position = new Vector3(transform.position.x, -size.y/2, transform.position.z);

        StartCoroutine("Spawn");

        r = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
	}

    public IEnumerator Spawn()
    {
        float startSpawn = Time.time;

        //Prevent the tree from being moved around while it's "growing"
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.useGravity = false;
        grabbable = false;

        //Move the tree upwards to spawn
        while (Time.time - startSpawn < spawnTime)
        {
            //Determine the new height to place the tree at
            float y = -size.y / 2 + (size.y * ((Time.time - startSpawn) / spawnTime));
            //Move the center of the collider to prevent other trees trying to spawn on it
            Vector3 c = GetComponent<BoxCollider>().center;
            c.y = size.y/2 - y;
            GetComponent<BoxCollider>().center = c;
            //Update the position
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        //Allow the tree to be moved - spawn finished
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        grabbable = true;
    }
}
