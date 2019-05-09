using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class SmashObject
{
    public string recipeName;
    public string obj1;
    public string obj2;
    public GameObject result;
}

[System.Serializable] public class MatStorage
{
    [HideInInspector] public int totMat;
    public int startMats;
    [HideInInspector] public int maxMat;
    public int startMax;

    public void Init()
    {
        totMat = startMats;
        maxMat = startMax;
    }

    public bool Add(int amount)
    {
        if (totMat + amount < maxMat)
        {
            totMat += amount;
            return true;
        }
        return false;
    }
}

public class SmashController : MonoBehaviour {
    
    [HideInInspector] public HUDcontroller hud;

    public MatStorage food = new MatStorage();
    public MatStorage wood = new MatStorage();
    public MatStorage stone = new MatStorage();

    public List<SmashObject> recipes = new List<SmashObject>();

    public float launchForce;
    
    public GameObject lHand;
    [HideInInspector] public GameObject lHeld;
    private float lVelocity;
    
    public GameObject rHand;
    [HideInInspector] public GameObject rHeld;
    private float rVelocity;

    private float totalVelocity;
    public float smashVelocity;

    public float hoomanSpeed = 1f;

    void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUDcontroller>();
        Physics.IgnoreLayerCollision(11, 5); //Grabbable objects and the UI will not collide
        Physics.IgnoreLayerCollision(14, 15); //Hoomans and Resources

        food.Init();
        wood.Init();
        stone.Init();
    }
      
	void Update ()
    {
        lVelocity = lHand.GetComponent<HandController>().velocity;  //Get the current velocity of each hand
        rVelocity = rHand.GetComponent<HandController>().velocity;
        totalVelocity = lVelocity + rVelocity;                      //Find the total velocity of the hands
        //Debug.Log("Total v: " + totalVelocity);
        lHeld = lHand.GetComponent<HandController>().held;          //Check if the player is holding anything
        rHeld = rHand.GetComponent<HandController>().held;
        
        if (totalVelocity > smashVelocity && lHeld != null && rHeld != null)    //Check if the player is holding two objects and the controllers are moving fast enough
        {
            if (lHeld.GetComponent<BasicThrown>().smashColl == rHeld || rHeld.GetComponent<BasicThrown>().smashColl == lHeld)   //Check if the objects collided with each other
                Smash(rHeld, lHeld);
        }

        //Easter Egg :)
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.I))
            EasterEgg();
        //Add materials - Testing purposes
        if (Input.GetKey(KeyCode.Keypad7))
            food.totMat++;
        if (Input.GetKey(KeyCode.Keypad8))
            wood.totMat++;
        if (Input.GetKey(KeyCode.Keypad9))
            stone.totMat++;
        if (Input.GetKey(KeyCode.Keypad0))
        {
            food.totMat++;
            wood.totMat++;
            stone.totMat++;
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            food.totMat = 0;
            wood.totMat = 0;
            stone.totMat = 0;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            hud.SendMsg("You pressed Key number 2", 2f, Color.black);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            hud.SendMsg("3: hoomanSpeed = " + hoomanSpeed.ToString(), 0.5f, Color.green);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.parent.Translate(GameObject.Find("Main Camera").transform.right * -.75f);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.parent.Translate(GameObject.Find("Main Camera").transform.right * .75f);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.parent.Translate(GameObject.Find("Main Camera").transform.forward * .75f);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.parent.Translate(GameObject.Find("Main Camera").transform.forward * -.75f);
    }

    public void Smash(GameObject smash1, GameObject smash2) //Call functions that are used to control what happens when 2 objects are combined
    {
        //Debug.Log("SmashController: smash " + Time.time);
        string type1 = smash1.GetComponent<BasicThrown>().assetType.ToString();     //Get the name of the objects
        string type2 = smash2.GetComponent<BasicThrown>().assetType.ToString();
        for (int i = 0; i < recipes.Count; i++)                     //Iterate through all of the recipes
        {
            SmashObject curr = recipes[i];
            if (curr.obj1 == type1 && curr.obj2 == type2 || curr.obj1 == type2 && curr.obj2 == type1)   //Check if any combination of the objects are the same as the recipe
            {
                GameObject obj = Instantiate(curr.result, rHand.transform.position, new Quaternion(0, 0, 0, 0));    //Create the result of the recipe
                obj.GetComponent<BasicThrown>().normalParent = smash1.GetComponent<BasicThrown>().normalParent;     //Set the parent of the object to the right hand
                rHand.GetComponent<HandController>().PickUp(obj);
                Destroy(smash1);                                                                                    //Destroy crafting materials
                Destroy(smash2);
            }
        }
    }

    public void EasterEgg()
    {
        GameObject i = GameObject.Find("Island");
        i.AddComponent<BoxCollider>();
        i.AddComponent<Rigidbody>().useGravity = false;
        i.AddComponent<BasicThrown>().gravScale = 1;
        //i.GetComponent<BasicThrown>().grabbable = true;
        //i.GetComponent<BasicThrown>().holdPos = i.transform.position;
    }
}
