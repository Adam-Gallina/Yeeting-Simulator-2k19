using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : ButtonController {

    public float maxDisplacement;
    public float speed;

    private int dir = 1;
    private float startPos;
    private float currDisplacement;



    private void Start()
    {

        startPos = transform.parent.position.y;

        currDisplacement = Random.Range(0, maxDisplacement * 10) / 10;
        dir = Random.Range(0, 2) == 0 ? -1 : 1;

        startPos += currDisplacement;
    }

    private void FixedUpdate()
    {
        if (currDisplacement <= 0 || currDisplacement >= maxDisplacement)
            dir *= -1;

        currDisplacement += speed * dir;

        Vector3 tp = transform.parent.position;
        transform.parent.position = new Vector3(tp.x, startPos + currDisplacement, tp.z);

        Vector3 cam = GameObject.Find("Main Camera").transform.position;
        transform.parent.LookAt(new Vector3(cam.x, tp.y, cam.z));
	}
}
