using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankAmounts : BasicMenu {

    private SmashController sc;
    private TextMesh wood, food, stone;

    void Start ()
    {
        menu = transform.GetChild(0).gameObject;
        wood = menu.transform.GetChild(1).GetComponent<TextMesh>();
        food = menu.transform.GetChild(2).GetComponent<TextMesh>();
        stone = menu.transform.GetChild(3).GetComponent<TextMesh>();
        sc = GameObject.Find("ViveCurvePointers").GetComponent<SmashController>();
    }

    void Update ()
    {
        menu.SetActive(active);
        wood.text = "Wood: " + sc.wood.totMat.ToString();
        food.text = "Food: " + sc.food.totMat.ToString();
        stone.text = "Stone: " + sc.stone.totMat.ToString();
    }
}
