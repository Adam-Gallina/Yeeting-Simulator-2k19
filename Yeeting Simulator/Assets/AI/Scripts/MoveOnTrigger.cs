using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter(Collider c)
    {
        if(c.transform.root.GetComponent<AI>())
        {
            c.transform.root.position = Vector3.zero;
        }
        if(c.transform.GetComponentInParent<Resource>())
        {
            Destroy(c.transform.GetComponentInParent<Resource>().gameObject);
        }
    }
}
