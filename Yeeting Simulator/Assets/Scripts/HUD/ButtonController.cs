using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {

    private bool hover = false;
    private bool press = false;

    public Material normal, hovered;
    public float displacement = 0.2f;
    public UnityEvent call;

    protected Renderer r;

    private void Awake()
    {
        r = transform.GetChild(0).GetComponent<Renderer>();
    }

    public void Hover()
    {
        if(!hover)
        {
            hover = true;
            r.material = hovered;
        }
    }
    public void UnHover()
    {
        if (hover)
        {
            hover = false;
            r.material = normal;
        }
    }
    public void Press()
    {
        if (!press)
        {
            press = true;
            transform.position += transform.up * -displacement;
            call.Invoke();
        }
    }
    public void UnPress()
    {
        if (press)
        {
            press = false;
            transform.position += transform.up * displacement;
        }
    }
}
