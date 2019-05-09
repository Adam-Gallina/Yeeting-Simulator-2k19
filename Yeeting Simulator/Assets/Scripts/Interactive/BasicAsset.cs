using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AssetType { None, Food, Wood, Stone, Hooman, Building };
public abstract class BasicAsset : MonoBehaviour {

    /*The base script for any interactive asset in the game
        - Allows all objects to be treated the same, with different characteristics*/

    public AssetType assetType;

    [HideInInspector] public Transform normalParent;
    [HideInInspector] public Vector3 normalScale;

    //Interactive flags - determine what the player is able to do with the object
    public bool grabbable;
    public bool placeable;
    public bool canClick;

    protected GameObject model;

    protected Renderer r;
    public Material normal, hovered;

    //Simple Awake() code
    protected void BasicAwake(bool canGrab, bool canPlace)
    {
        tag = "Asset";

        grabbable = canGrab;
        placeable = canPlace;

        model = transform.GetChild(0).gameObject;

        normalParent = transform.parent;
        normalScale = transform.localScale;
    }

    //Returns true if the other object can be placed in this object
    public bool TryAdd(GameObject obj)
    {
        if (placeable)
        {
            GetComponent<Container>().AddObject(obj);
            return true;
        }
        return false;
    }

    public void ChangeParent(Transform parent = null)
    {
        if (assetType == AssetType.Hooman)
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        if (parent == null)
        {
            parent = normalParent;
            if (assetType == AssetType.Hooman)
                GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        transform.parent = parent;
    }
}
