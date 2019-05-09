using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : BasicBuilding {

    /*This is the base script for any building object that will have materials put into it*/


    //Used to determine where/how big placed objects are
    public Vector3 placedPos;
    public Vector3 placedAng;
    public Vector3 placedScale;
    public List<GameObject> held = new List<GameObject>();

    private void Awake()
    {
        BasicAwake(false, true);

        upgrades = transform.GetChild(1).gameObject;
    }

    private void FixedUpdate()
    {
        //Update the positions of the contained objects
        HoldObjects();
    }

    //Abstract classes to add/remove objects - customized in specific building scripts
    public abstract void AddObject(GameObject obj);
    public abstract void RemObject(int place);
    public abstract bool RemObject(string type, int amount);
    
    //Iterate through the contained objects, and ensure that they remain in the correct spot
    protected void HoldObjects()
    {
        for (int i = 0; i < held.Count; i++)
        {
            held[i].transform.position = placedPos + transform.position;
            held[i].transform.eulerAngles = placedAng;
            held[i].transform.localScale = placedScale;
            held[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            held[i].GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
