using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : Producer
{
    public MatPrice speedUpgrade;

    public void UpgradeHoomanSpeed()
    {
        if (speedUpgrade.CheckPrice())
        {
            speedUpgrade.Buy();
            GameObject.Find("ViveCurvePointers").GetComponent<SmashController>().hoomanSpeed += 0.5f;
        }
    }
}
