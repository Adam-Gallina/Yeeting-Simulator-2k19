using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InfoObject : MonoBehaviour
{
    public GameObject Main;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        FindObjectOfType<InfoCanvas>().Select(Main);
    }
}
