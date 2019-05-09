using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public CraftingRecipe CurrentBuilding;
    public bool Complete;
    
    // Start is called before the first frame update
    void Start()
    {
        Complete = true;
    }
    public void Build()
    {
        Complete = true;
        Debug.Log(gameObject.name + " built a " + CurrentBuilding.Name);
        var building = Instantiate(CurrentBuilding.Prefab);
        building.transform.position = GetComponent<AI>().d.CurrentDestination.Location.position;
        building.transform.SetParent(GameObject.FindGameObjectWithTag("Enviroment").transform);
    }
}
