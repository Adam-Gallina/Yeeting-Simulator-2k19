using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDcontroller : BasicMenu {

    private Transform cam;
    private GameObject msgBoard;
    private float nextHide = 0.0f;
    public bool showBoard = false;

	void Start ()
    {
        cam = transform.parent;
        msgBoard = transform.GetChild(0).gameObject;
	}
	
	void Update ()
    {
        msgBoard.SetActive(showBoard);
        
        if (Time.time >= nextHide)
        {
            showBoard = false;
        }
    }

    public void SendMsg(string msg, float time, Color col)
    {
        Debug.Log("Sending " + msg + " at " + time.ToString());
        msgBoard.transform.GetChild(1).GetComponent<TextMesh>().text = msg;
        msgBoard.transform.GetChild(1).GetComponent<TextMesh>().color = col;
        nextHide = Time.time + time;
        showBoard = true;
    }
}
