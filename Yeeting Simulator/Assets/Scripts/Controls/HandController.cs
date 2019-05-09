using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    
    [HideInInspector] public bool doGrab = false;
    [HideInInspector] public GameObject held;

    private Vector3 lastPos;
    private Vector3 currPos;
    private Vector3 dir;
    [HideInInspector] public float velocity;
    public float minSpinVelocity;

    [HideInInspector] public Transform pointerOrigin;
    /*[HideInInspector] */public LayerMask pointerIgnore;
    [HideInInspector] public LineRenderer pointer;
    private ButtonController lastBut;
    private BasicBuilding lastBui;

    private void Start()
    {
        lastPos = new Vector3();
        //pointerIgnore =  LayerMask.NameToLayer("UI") + LayerMask.NameToLayer("Building");

        pointer = transform.GetChild(0).GetChild(1).GetComponent<LineRenderer>();
        pointerOrigin = transform.GetChild(0).GetChild(0).transform.GetChild(0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asset")
        {
            if (other.gameObject.GetComponent<BasicAsset>().grabbable && held == null && doGrab && !pointer.enabled)
            {
                PickUp(other.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        currPos = transform.position;
        
        dir = (currPos - lastPos).normalized;
        velocity = Vector3.Distance(currPos, lastPos) * GameObject.Find("ViveCurvePointers").GetComponent<SmashController>().launchForce;

        lastPos = currPos;
    }
    public void Update()
    {
        RaycastHit hit = new RaycastHit(); Physics.Raycast(pointerOrigin.position, transform.forward, out hit, Mathf.Infinity, pointerIgnore);
        if (hit.collider != null && held == null)
        {
            showPointer(hit);

            if (hit.collider.tag == "Button")
            {
                lastBut = hit.transform.parent.GetComponent<ButtonController>();
                lastBut.Hover();
            }
            else if (lastBut != null)
            {
                lastBut.UnHover();
                lastBut = null;
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Building"))
            {
                lastBui = hit.transform.GetComponent<BasicBuilding>();
                lastBui.Hover();
            }
            else if (lastBui != null)
            {
                lastBui.UnHover();
                lastBui = null;
            }
        }
        else
        {
            pointer.enabled = false;
            if (lastBut != null)
            {
                lastBut.UnHover();
                lastBut = null;
            }
            if (lastBui != null)
            {
                lastBui.UnHover();
                lastBui = null;
            }
        }

        if (held != null && !doGrab)
        {
            held.GetComponent<BasicAsset>().ChangeParent();
            bool drop = true;
            if (held.GetComponent<BasicThrown>().addColl != null)
            {
                drop = !held.GetComponent<BasicThrown>().addColl.GetComponent<BasicAsset>().TryAdd(held);
            }
            if (drop)
            {
                if (velocity > minSpinVelocity)
                {
                    Rigidbody rb = held.GetComponent<Rigidbody>();
                    rb.AddForce(dir * velocity);
                    rb.angularVelocity = dir * velocity;
                }
            }

            held.GetComponent<BasicThrown>().held = false;
            held = null;
        }
        else if (held != null)
        {
            held.transform.position = transform.position;
            held.transform.rotation = transform.rotation;
        }
    }

	public void Grab()
    {
        if (lastBut != null)
            lastBut.Press();
        else if (lastBui != null)
            lastBui.Press();
        else
            doGrab = true;
    }
    public void UnGrab()
    {
        if (lastBut != null)
            lastBut.UnPress();
        else if (lastBui != null)
            lastBui.UnPress();
        else
            doGrab = false;
    }
    public void PickUp(GameObject obj)
    {
        if (obj.gameObject.GetComponent<BasicThrown>().held || !obj.GetComponent<BasicAsset>().grabbable)
            return;
        held = obj;
        BasicThrown script = held.GetComponent<BasicThrown>();
        script.ChangeParent(transform);
        script.held = true;
        held.transform.position = transform.position;
        held.transform.rotation = transform.rotation;
        script.moved = true;
    }
    public void showPointer(RaycastHit hit)
    {
        pointer.enabled = true;
        Vector3[] positions = { pointerOrigin.position, hit.point };
        pointer.SetPositions(positions);
    }
}
