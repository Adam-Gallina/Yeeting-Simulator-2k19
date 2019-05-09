using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : BasicMenu {
    
    private GameObject cam;

    public GameObject wood;
    public GameObject food;
    public GameObject stone;

    private void Start()
    {
        menu = transform.GetChild(0).gameObject;
        cam = GameObject.Find("Camera");
    }

    private void Update()
    {
        menu.SetActive(active);
    }

    public void AddWood()
    {
        GameObject spawn = Instantiate(wood, new Vector3(cam.transform.position.x, 2, cam.transform.position.z), new Quaternion(0, 0, 0, 0));
        spawn.transform.parent = GameObject.Find("Material Holder").transform;
    }
    public void AddFood()
    {
        GameObject spawn = Instantiate(food, new Vector3(cam.transform.position.x, 2, cam.transform.position.z), new Quaternion(0, 0, 0, 0));
        spawn.transform.parent = GameObject.Find("Material Holder").transform;
    }
    public void AddStone()
    {
        GameObject spawn = Instantiate(stone, new Vector3(cam.transform.position.x, 2, cam.transform.position.z), new Quaternion(0, 0, 0, 0));
        spawn.transform.parent = GameObject.Find("Material Holder").transform;
    }
}
