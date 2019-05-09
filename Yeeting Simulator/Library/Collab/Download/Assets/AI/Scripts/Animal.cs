using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    Vector3 destination;
    NavMeshAgent nav;
    float timeAtDestination;
    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.Warp(destination);
        nav.enabled = false;
        Invoke("WanderToPoint", .05f);
    }
    public void WanderToPoint()
    {
        timeAtDestination = 0;
        nav.enabled = true;
        var m = FindObjectOfType<AIMaster>().MinBounds;
        var x = FindObjectOfType<AIMaster>().MaxBounds;
        destination = new Vector3(Random.Range(m.x, x.x), Random.Range(m.y, x.y), Random.Range(m.z, x.z));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 3, nav.areaMask))
        {
            //Debug.Log(hit.position);
            destination = hit.position;
        }
        nav.SetDestination(destination);
    }
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(destination, transform.position) < 1)
        {
            timeAtDestination += Time.deltaTime;
            if(timeAtDestination > 3f)
            {
                WanderToPoint();
            }
        }
    }
}
