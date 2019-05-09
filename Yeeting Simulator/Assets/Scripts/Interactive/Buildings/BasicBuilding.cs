using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Class for purchasing objects ingame
[System.Serializable] public class MatPrice
{
    [HideInInspector] public SmashController sc; 
    //Price of the object
    public int food, wood, stone;

    //Returns true if the player has enough resources to purchase
    public bool CheckPrice()
    {
        sc = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>();
        if (sc.food.totMat >= food && sc.wood.totMat >= wood && sc.stone.totMat >= stone)
            return true;
        GameObject.Find("ViveCurvePointers").GetComponent<SmashController>().hud.SendMsg("Not enough resources", 1.0f, Color.red);
        return false;
    }

    //Purchases the object
    public void Buy()
    {
        sc = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>();
        sc.food.totMat -= food;
        sc.wood.totMat -= wood;
        sc.stone.totMat -= stone;
    }
}

public abstract class BasicBuilding : BasicAsset {
    
    protected GameObject upgrades;
    protected bool upgradesOpen = false;

    protected bool hover = false;
    protected bool press = false;

    public bool changeMaterial = true;
    //In case the object has button-like characteristics
    public UnityEvent call;

    new protected void BasicAwake(bool canGrab, bool canPlace)
    {
        grabbable = canGrab;
        placeable = canPlace;

        model = transform.GetChild(0).gameObject;

        normalParent = transform.parent;
        normalScale = transform.localScale;

        r = GetComponent<Renderer>();
    }

    private void Update()
    {
        upgrades.SetActive(upgradesOpen);

        //Ensure the menu is always facing the player
        upgrades.transform.LookAt(GameObject.Find("Main Camera").transform);
    }

    //Toggles the upgrades bool - called by a button script
    public void ToggleUpgrades()
    {
        upgradesOpen = !upgradesOpen;
    }

    //Functions for when the object is hovered or pressed, all update the material of the renderer
    public void Hover()
    {
        if (!canClick)
            return;

        if (!hover)
        {
            hover = true;
            if (changeMaterial)
                r.material = hovered;
        }
    }
    public void UnHover()
    {
        if (!canClick)
            return;

        if (hover)
        {
            hover = false;
            if (changeMaterial)
                r.material = normal;
        }
    }
    public void Press()
    {
        if (!canClick)
            return;

        if (!press)
        {
            press = true;
            call.Invoke();
        }
    }
    public void UnPress()
    {
        if (!canClick)
            return;

        if (press)
        {
            press = false;
        }
    }
}
