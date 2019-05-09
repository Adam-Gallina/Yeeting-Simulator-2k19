using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagment : MonoBehaviour
{
    public List<Resource> Wood = new List<Resource>();
    public List<Resource> Stone = new List<Resource>();
    public List<Resource> Food = new List<Resource>();
    AIMaster master;
    // Start is called before the first frame update
    void Start()
    {
        master = FindObjectOfType<AIMaster>();
        GetR();
    }
    public void GetR()
    {
        Resource[] rs = FindObjectsOfType<Resource>();
        Stone = new List<Resource>();
        Wood = new List<Resource>();
        Food = new List<Resource>();
        for(int ii = 0; ii < rs.Length; ii++)
        {
            Resource ri = rs[ii];
            if (ri.ResourceType == RType.Tree)
                Wood.Add(ri);
            if (ri.ResourceType == RType.Stone)
                Stone.Add(ri);
            if (ri.ResourceType == RType.Animal)
                Food.Add(ri);
        }
    }
    public Resource GetNearbyR(RType type, Vector3 point)
    {
        Resource[] rs = FindObjectsOfType<Resource>();
        Resource closest = null;
        for (int i = 0; i < rs.Length; i++)
        {
            Resource ri = rs[i];
            if (ri.ResourceType == type)
            {
                if(closest == null || master.IsResourceEmpty(ri) && closest != null && Vector3.Distance(closest.transform.position, point) > Vector3.Distance(point, ri.transform.position))
                {
                    closest = ri;
                }
            }
        }
        return closest;
    }
}
