using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : BasicBuilding {

    /*The base script for any building that will be producing objects during the game*/

    //Material spawned by object
    public GameObject mat;
    protected Transform matHolder;

    //Minimum distance between objects
    public int spacing;
    public LayerMask lm;

    //Editing - Draw the location of the box for spawning objects
    //Location/size of the box
    public Vector3 center;
    public Vector3 size;
    //Draw the box in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 255, 0, .75f);
        Gizmos.DrawWireCube(transform.position + center, size);
    }
    
    void Awake()
    {
        BasicAwake(false, true);

        upgrades = transform.GetChild(1).gameObject;
        matHolder = GameObject.Find("Material Holder").transform;
    }

    //Spawn a new material - Call ObjectSpawner functions, and handle the returned GameObject
    public GameObject SpawnMat(GameObject mat, Vector3 center, Vector3 size, LayerMask lm)
    {
        //Get the xyz coordinate for a new object
        Vector3 place = ObjectSpawner.SpawnObj(mat, transform.position + center, size, lm);
        if (place != new Vector3())
        {
            //Spawn the material if possible, and update the parent of it
            GameObject newMat = Instantiate(mat);
            //Vector3 newPos2 = new Vector3(newPos.x, hit.point.y + obj.mat.GetComponent<BasicThrown>().size.y + 0.1f / 2, newPos.z);
            newMat.transform.position = place;
            //newMat.transform.rotation = ang;
            newMat.transform.parent = GameObject.Find("Material Holder").transform;
            return newMat;
        }
        return null;
    }
}
