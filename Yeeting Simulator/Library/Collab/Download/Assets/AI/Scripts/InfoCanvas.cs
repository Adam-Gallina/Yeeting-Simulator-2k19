using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InfoCanvas : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Info;
    public TextMeshProUGUI Desc;
    public GameObject Selected;
    // Start is called before the first frame update
    void Start()
    {
        Title = GetComponentsInChildren<TextMeshProUGUI>()[0];
        Info = GetComponentsInChildren<TextMeshProUGUI>()[1];
        Desc = GetComponentsInChildren<TextMeshProUGUI>()[2];
        Select(null);
    }
    public void Select(GameObject selected)
    {
        Debug.Log("Select");
        Title.text = "";
        Info.text = "";
        Desc.text = "Click on an object to view it's information";
        Selected = selected;
        if (selected == null)
            return;
        if(selected.GetComponent<Resource>())
        {
            var r = selected.GetComponent<Resource>();
            Title.text = r.ResourceType + "";
            Info.text = r.ResourceType + " (Resource)";
            Desc.text = "Range of materials: " + r.MinAmt + " - " + r.MaxAmt + " \n";
        }
        if(selected.GetComponent<AI>())
        {
            var ai = selected.GetComponent<AI>();
            Title.text = ai.gameObject.name + " (Side " + ai.GetComponent<YeetedAI>().Side + ")";
            //Info.text = ai.gameObject.name + " (Side " + ai.GetComponent<YeetedAI>().Side + ")";
            Desc.text = "Resources: \n Wood: " + ai.resources.Wood + " \n Stone: " + ai.resources.Stone + " \n Meat: " + ai.resources.Food + "\n Misc: \n Role: " + ai.r.type + " \n Destination: " + ai.d.CurrentDestination.Location.gameObject.name;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.transform.gameObject);
            }
        }
    }
}
