using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Producer {

    public MatPrice price;

    public void TryPurchase()
    {
        //Purchase if there's enough resources to buy
        if (price.CheckPrice())
        {
            if (SpawnMat(mat, center, size, lm) != null)
            {
                price.Buy();
            }
        }
    }
}
