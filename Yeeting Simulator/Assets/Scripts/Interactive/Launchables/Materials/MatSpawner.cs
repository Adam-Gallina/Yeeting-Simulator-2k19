using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class MatSpawn
{
    //Material spawned under these parameters
    public List<GameObject> mat = new List<GameObject>();

    //Materials spawned when the game starts
    public int startAmount;
    //Maximum amount of materials at any time
    public int maxMats;
    //Total materials - unmoved
    [HideInInspector] public int totMats;
    [HideInInspector] public List<GameObject> mats = new List<GameObject>();

    public Vector2 spawnFrequency;
    [HideInInspector] public float nextSpawn = 0.0f;

    //How high to spawn objects
    public float yOffset = 5;

    //Field for spawning objects in
    public bool ShowConstraints = true;
    public Vector3 center;
    public Vector3 size;
}

public class MatSpawner : MonoBehaviour {

    //Testing - false prevents spawns
    public bool spawn = true;

    public MatSpawn food = new MatSpawn();
    public MatSpawn wood = new MatSpawn();
    public MatSpawn stone = new MatSpawn();
    protected List<MatSpawn> mats = new List<MatSpawn>();

    //LayerMask for spawned object collisions
    public LayerMask lm;

    private void OnValidate()
    {
        mats = new List<MatSpawn>() { food, wood, stone };
    }

    //Draw each spawn field for the different materials
    private void OnDrawGizmosSelected()
    {
        mats = new List<MatSpawn>() { food, wood, stone };
        List<Color> cols = new List<Color>() { Color.green, new Color(150, 75, 0), Color.grey };
        for (int i = 0; i < mats.Count; i++)
        {
            if (mats[i].ShowConstraints)
            {
                Gizmos.color = cols[i];
                Gizmos.DrawWireCube(mats[i].center + transform.position, mats[i].size);
            }
        }
    }

    private void Start()
    {
        if (mats.Count == 0)
            mats = new List<MatSpawn>() { food, wood, stone };

        if (spawn)
            StartCoroutine("SpawnStartMats");
        
    }

    //Spawn all of the initial materials
    IEnumerator SpawnStartMats()
    {
        foreach (MatSpawn mat in mats)
        {
            while (mat.totMats < mat.startAmount)
            {
                //Search for a valid location to spawn
                GameObject newMat = SpawnMat(mat.mat, mat.center, mat.size, lm);
                if (newMat != null)
                {
                    mat.mats.Add(newMat);
                    mat.totMats++;
                }
                else
                    yield return null;
            }
            //Update the next spawn time for that material
            mat.nextSpawn = Time.time + Random.Range(mat.spawnFrequency.x, mat.spawnFrequency.y);
        }
    }

    void Update()
    {
        if (spawn)
        {
            foreach (MatSpawn mat in mats)
            {
                //Check if any materials have been moved by the player
                foreach (GameObject obj in mat.mats.ToArray())
                {
                    if (obj == null)
                        mat.mats.Remove(obj);
                    if (obj.GetComponent<BasicThrown>().moved)
                        mat.mats.Remove(obj);
                }
                //Update the total number of variables
                mat.totMats = mat.mats.Count;

                //Spawn Materials if there aren't enough, and it's time
                if (mat.totMats < mat.maxMats && Time.time > mat.nextSpawn)
                {
                    GameObject newMat = SpawnMat(mat.mat, mat.center, mat.size, lm);
                    if (newMat != null)
                    {
                        mat.mats.Add(newMat);
                        mat.nextSpawn = Time.time + Random.Range(mat.spawnFrequency.x, mat.spawnFrequency.y);
                    }
                }
            }
        }
    }

    //Interact with the ObjectSpawner script to spawn materials
    public GameObject SpawnMat(List<GameObject> mats, Vector3 center, Vector3 size, LayerMask lm)
    {
        GameObject mat = mats.Count == 1 ? mats[0] : mats[Random.Range(0, mats.Count)];

        Vector3 place = ObjectSpawner.SpawnObj(mat, center + transform.position, size, lm);
        if (place != new Vector3())
        {
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
