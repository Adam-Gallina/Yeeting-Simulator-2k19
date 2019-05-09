using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour {

    /*Script to spawn trees - deprecated
     Replaced with MatSpawner.cs - Same function, but allows the specification of materials to spawn*/

    public GameObject[] TreePrefabs;
    
    public int MaxTrees;
    int Trees;
    float yOffset = 5;
    public LayerMask lm;
    [Header("Positioning")]
    public float Spacing;
    public float[] xConstraints = { -25, 25 };
    public float[] yConstraints = { -20, 20 };
    public float[] zConstraints = { -25, 25 };
    List<Vector3> treePos = new List<Vector3>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 255, 0, .75f);
        Vector3 center = new Vector3(transform.position.x, transform.position.y - GetComponent<BoxCollider>().size.y / 2, transform.position.z);
        center = new Vector3(center.x + (xConstraints[0] + xConstraints[1]) / 2, center.y + (yConstraints[0] + yConstraints[1]) / 2, center.z + (zConstraints[0] + zConstraints[1]) / 2);
        Gizmos.DrawWireCube(center, new Vector3(Mathf.Abs(xConstraints[0]) + Mathf.Abs(xConstraints[1]), Mathf.Abs(yConstraints[0]) + Mathf.Abs(yConstraints[1]), Mathf.Abs(zConstraints[0]) + Mathf.Abs(zConstraints[1])));
    }

    bool TooClose(Vector3 vector)
    {
        bool tc = false;
        foreach (Vector3 v in treePos)
        {
            if (Vector3.Distance(vector, v) < Spacing)
            {
                tc = true;
            }
        }
        return tc;
    }

    public void SpawnTree(GameObject tree)
    {
        Vector3 newPos = new Vector3(Random.Range(xConstraints[0], xConstraints[1]), 200, Random.Range(zConstraints[0], zConstraints[1]));
        if(!TooClose(newPos))
        {
            RaycastHit hit;
            if(Physics.Raycast(newPos, Vector3.down, out hit, Mathf.Infinity, lm))
            {
                var t = Instantiate(tree);
                Vector3 newPos2 = new Vector3(newPos.x, hit.point.y + yOffset, newPos.z);
                t.transform.position = newPos2;
                Trees += 1;
                t.transform.parent = transform;
            }
        }
    }
	
	void Update () {
		if(Trees < MaxTrees)
        {
            SpawnTree(TreePrefabs[Random.Range(0, TreePrefabs.Length - 1)]);
        }
	}
}
