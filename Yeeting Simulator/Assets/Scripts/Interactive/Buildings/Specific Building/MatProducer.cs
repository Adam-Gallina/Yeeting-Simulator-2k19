using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatProducer : Producer
{
    //Determines the time between spawns
    public float frequency;
    //Used to set the time of the next object spawn
    protected float nextSpawn = 0.0f;

    void Start()
    {
        //Initiate nextSpawn variable at the current time
        nextSpawn = Time.time;
    }

    void Update()
    {
        upgrades.SetActive(upgradesOpen);

        //Spawn an object at time=nextSpawn
        if (Time.time > nextSpawn)
        {
            if (SpawnMat(mat, center, size, lm) != null)
                nextSpawn = Time.time + frequency;  //Reset nextSpawn variable
        }
    }
}
