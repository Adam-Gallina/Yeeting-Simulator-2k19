using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectSpawner {
    
    //Return a location to spawn an object at
    public static Vector3 SpawnObj(GameObject obj, Vector3 center, Vector3 size, LayerMask lm)
    {
        Vector3 newPos = RandSpot(center, size);
        //Quaternion ang = TooClose(newPos, obj.mat);
        //if (ang != new Quaternion())
        int x = TooClose(newPos, obj, lm);
        if (x == 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(newPos, Vector3.down, out hit, Mathf.Infinity, ~lm))
            {
                /*GameObject mat = Instantiate(obj.mat);
                Vector3 newPos2 = new Vector3(newPos.x, hit.point.y + obj.mat.GetComponent<BasicThrown>().size.y + 0.1f / 2, newPos.z);
                mat.transform.position = newPos2;
                //mat.transform.rotation = ang;
                mat.transform.parent = GameObject.Find("Material Holder").transform;
                return mat;*/
                return new Vector3(newPos.x, hit.point.y + obj.GetComponent<BoxCollider>().size.y / 1.9f, newPos.z);
            }
        }
        return new Vector3();
    }
    //Check if a potential spot is too close to other objects
    public static int TooClose(Vector3 spot, GameObject mat, LayerMask lm)
    {
        //Quaternion ang = mat.transform.rotation;
        //ang = new Quaternion(ang.x, Random.Range(0, 360), ang.z, ang.w);
        Collider[] obstCheck = Physics.OverlapBox(spot, mat.GetComponent<BasicThrown>().size / 2, new Quaternion(), ~lm);

        //if (obstCheck.Length > 0)
        //    ang = new Quaternion();

        ExtDebug.DrawBox(spot, mat.GetComponent<BasicThrown>().size / 2, new Quaternion(), Color.red, 0.5f);
        return obstCheck.Length;
    }
    //Get a random spot inside of a spawn field
    public static Vector3 RandSpot(Vector3 center, Vector3 size)
    {
        Vector3 spot = new Vector3();
        size /= 2;

        spot.x = Random.Range(center.x - size.x, center.x + size.x);
        spot.y = Random.Range(center.y - size.y, center.y + size.y);
        spot.z = Random.Range(center.z - size.z, center.z + size.z);

        return spot;
    }
}
