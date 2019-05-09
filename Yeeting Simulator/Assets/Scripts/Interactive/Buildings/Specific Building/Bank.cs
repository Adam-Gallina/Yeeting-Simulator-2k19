using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Container {

    private SmashController sc;

    public MatPrice storageUpgrade;
    public int upgradeMod;

    private void Start()
    {
        sc = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>();

    }

    public void UpgradeStorage()
    {
        if (storageUpgrade.CheckPrice())
        {
            storageUpgrade.Buy();
            storageUpgrade.food++;
            storageUpgrade.wood++;
            storageUpgrade.stone++;

            sc.food.maxMat += upgradeMod;
            sc.wood.maxMat += upgradeMod;
            sc.stone.maxMat += upgradeMod;
        }
    }

    //Handle objects being added/removed - bank specific
    public override void AddObject(GameObject obj)
    {
        if (!placeable)
            return;
        //Increase the amount of the resource and destroy the object
        switch (obj.GetComponent<BasicThrown>().assetType.ToString())
        {
            case "Food":
                if (!sc.food.Add(1))
                    return;
                break;
            case "Wood":
                if (!sc.wood.Add(1))
                    return;
                break;
            case "Stone":
                if (!sc.stone.Add(1))
                    return;
                break;
            default:
                Debug.LogError("Material Unrecognized");
                return; //Material unrecognized
        }
        Destroy(obj);
    }
    //Lower the global materials - purchases, etc
    public override bool RemObject(string type, int amount)
    {
        switch (type)
        {
            case "Food":
                if (sc.food.totMat >= amount)
                {
                    sc.food.totMat -= amount;
                    return true;
                }
                break;
            case "Wood":
                if (sc.wood.totMat >= amount)
                {
                    sc.wood.totMat -= amount;
                    return true;
                }
                break;
            case "Stone":
                if (sc.stone.totMat >= amount)
                {
                    sc.stone.totMat -= amount;
                    return true;
                }
                break;
        }
        return false;
    }
    public override void RemObject(int place)
    {
        throw new System.NotImplementedException();
    }
}
