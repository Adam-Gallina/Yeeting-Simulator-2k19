using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum RType { Tree, Stone, Fruit, Animal};
public class Resource : MonoBehaviour
{
    public RType ResourceType;
    public int MinAmt = 2;
    public int MaxAmt = 5;
    public int ResourcesLeft = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Collect(AI ai)
    {
        int rndm = Random.Range(MinAmt, MaxAmt);
        if(ResourceType == RType.Tree)
        ai.resources.Wood += rndm;
        if (ResourceType == RType.Stone)
            ai.resources.Stone += rndm;
        if (ResourceType == RType.Animal)
            ai.resources.Food += rndm;
        ResourcesLeft -= rndm;
        if(ResourcesLeft <= 0 || ResourceType == RType.Animal)
        {
            if (GetComponent<NavMeshAgent>())
            {
                GetComponent<NavMeshAgent>().enabled = false;
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.mass = 900;
            foreach(Collider c in GetComponentsInChildren<Collider>())
            {
            if(ResourceType != RType.Animal)
            Destroy(c, 10);
            }
            Destroy(gameObject, 15);
            Destroy(this);
        }

    }
}
