using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMenu : MonoBehaviour {

    [HideInInspector] public bool active = false;
    protected GameObject menu;

    public void ToggleMenu()
    {
        active = !active;
    }
}
