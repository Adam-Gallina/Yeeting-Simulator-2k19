using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicThrown : BasicAsset {

    public float gravScale = 0.75f;
    protected Rigidbody rb;
    [HideInInspector] public Collider coll;
    //Size of the object - used for spawning
    public Vector3 center;
    public Vector3 size;
    
    //Check whether the object has been moved - used with ObjectSpawners
    [HideInInspector] public bool moved = false;
    [HideInInspector] public bool held = false;

    //Record which objects the material is currently interacting with
    [HideInInspector] public GameObject smashColl;
    [HideInInspector] public GameObject addColl;

    [HideInInspector] public Vector3 holdPos = new Vector3();

    //Draw the size of the object for spawning
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + center, size);
    }

    void Awake()
    {
        BasicAwake(true, false);

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        coll = GetComponent<Collider>();

        r = model.GetComponent<Renderer>();
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Asset")
        {
            //Update variables based on collided object
            //Check if the other object is a material
            if (collision.gameObject.GetComponent<BasicAsset>().grabbable)
            {
                smashColl = collision.gameObject;
            }
            //Check if the other object is a valid building
            if (collision.gameObject.GetComponent<BasicAsset>().placeable)
            {
                addColl = collision.gameObject;
                r.material = hovered;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Asset")
        {
            //Reset collided object variables
            if (collision.gameObject.GetComponent<BasicAsset>().grabbable)
            {
                smashColl = null;
            }
            if (collision.gameObject.GetComponent<BasicAsset>().placeable)
            {
                addColl = null;
                r.material = normal;
            }
        }
    }

    private void FixedUpdate()
    {
        //Prevent the object from falling when the player is holding it/hitting other objects
        if (held)
        {
            if (holdPos != new Vector3())
                holdPos = transform.position;
            coll.isTrigger = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
        }
        else
        {
            coll.isTrigger = false;
            if (gravScale != 0)
                rb.AddForce(Physics.gravity * rb.mass * gravScale);
            
            if (holdPos != new Vector3())
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);
                transform.position = holdPos;
            }
        }
    }
}