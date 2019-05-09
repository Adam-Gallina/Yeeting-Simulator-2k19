using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tile
{
    public string Name;
    public AIMaster m;
    public Vector3 Position;
    public bool Filled;
    public GameObject Visualizer;
    public Vector2 ArrPos;
    public bool Road = false;
    public void Show()
    {
        Visualizer.SetActive(true);
        
    }
}
[System.Serializable]
public class CraftingRecipe
{
    public string Name;
    public int AmountBuilt;
    public int MinAmt;
    [Range(0, 100)]
    public float PercentNeeded;
    public int Value;
    public int Size;
    public int Stone;
    public int Wood;
    public GameObject Prefab;
}
public class ConstructionMaster : MonoBehaviour
{
    public CraftingRecipe[] CraftingR;
    public ConstructionMaster Master;
    public List<Construction> Workers = new List<Construction>();
    AIBank bank;
    public List<Tile> tiles = new List<Tile>();
    public List<List<Tile>> Tiles2D = new List<List<Tile>>();
    public int Spacing;
    public int Size;
    public float YPoint;
    public int BuiltTotal;
    public CraftingRecipe Road;
    public int ID = 1;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < FindObjectsOfType<AIBank>().Length; i++)
        {
            if (FindObjectsOfType<AIBank>()[i].ID == ID)
                bank = FindObjectsOfType<AIBank>()[i];
        }
        if(Master == null)
        {
            GetTiles();
            GetGoodTile().Filled = true; //for bank
        }
        if(Master != null)
        {
            tiles = Master.tiles;
            Tiles2D = Master.Tiles2D;
        }
    }
    public void GetTiles()
    {
        var gp = new GameObject();
        Material m1 = FindObjectOfType<AIMaster>().ContructionMat;
        Material m2 = FindObjectOfType<AIMaster>().MinerMat;
        for (int t1 = 0; t1 < Size/Spacing; t1++)
        {
            var mm1 = m1;
            m1 = m2;
            m2 = mm1;
            Tiles2D.Add(new List<Tile>());
            for(int t2 = 0; t2 < Size/Spacing; t2++)
            {
                var newT = new Tile() { Position = transform.position + new Vector3(t1 * Spacing, YPoint, t2 * Spacing), Filled = false, Name = transform.position + new Vector3(t1, YPoint, t2) + "" };
                tiles.Add(newT);
                Tiles2D[t1].Add(newT);
                var g1 = new GameObject();
                var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    g1.transform.localPosition = tiles[tiles.Count - 1].Position;
                g1.transform.SetParent(gp.transform);
                    if (t2 % 2 == 0)
                        g.transform.GetComponent<MeshRenderer>().material = m1;
                    if (t2 % 2 == 1)
                        g.transform.GetComponent<MeshRenderer>().material = m2;
                newT.Visualizer = g1;
                g.transform.SetParent(g1.transform);
                newT.m = FindObjectOfType<AIMaster>();
                newT.ArrPos = new Vector2(t1, t2);
                var g2 = new GameObject();
                g2.transform.position = new Vector3(0, 0, 0);
                g2.transform.SetParent(g1.transform);
                g2.transform.localPosition = Vector3.zero;
                g2.AddComponent<TMPro.TextMeshPro>();
                g2.GetComponent<TMPro.TextMeshPro>().text = "Tile " + (tiles.Count - 1) + ", Index: [" + t1 + "][" + t2 + "]";
                g2.transform.position += new Vector3(0, 8, 0);
                g2.AddComponent<Info>();
                g.transform.position = Vector3.zero;
                g.transform.localPosition = Vector3.zero;
                g.transform.localPosition = new Vector3(Spacing / 2, 0, Spacing / 2);
                g.transform.localScale = new Vector3(Spacing, 1, Spacing);
                g2.transform.localScale = new Vector3(.2f, .2f, .2f);
                g1.SetActive(false);
            }
        }
    }
    void GetWorkers()
    {
        Workers = new List<Construction>();
        foreach(AI ai in FindObjectsOfType<AI>())
        {
            if(ai.GetComponent<YeetedAI>().Side == ID)
            if(ai.r.type == RoleType.Builder)
            {
                if(ai.GetComponent<Construction>())
                {
                    if (ai.GetComponent<Construction>().Complete)
                    Workers.Add(ai.GetComponent<Construction>());
                }
                else
                {
                    Workers.Add(ai.gameObject.AddComponent<Construction>());
                }
            }
        }
    }
    public void GiveJobs()
    {
        if(Master != null)
        {
            tiles = Master.tiles;
            Tiles2D = Master.Tiles2D;
        }
        GetWorkers();
        if(Workers.Count > 0)
        if(CanBuild(GetNeededBuilding()) && GetGoodTile() != null)
        {
            if(!GetGoodTile().Filled)
            {
                if(!GetGoodTile().Road)
                {
                    var g = new GameObject();
                    var tile = GetGoodTile();
                    g.transform.position = tile.Position;
                    tile.Filled = true;
                    g.name = "Build Point";
                    CraftingRecipe b = GetNeededBuilding();
                    Workers[0].Complete = false;
                    Workers[0].CurrentBuilding = b;
                    b.AmountBuilt += 1;
                    bank.Wood -= b.Wood;
                    bank.Stone -= b.Stone;
                    Workers[0].GetComponent<AI>().SetDestination(g.transform, true);

                }
                else
                {
                    if(GetGoodTile().Road)
                    {
                        var g = new GameObject();
                        var tile = GetGoodTile();
                        g.transform.position = tile.Position;
                        tile.Filled = true;
                        g.name = "Build Point";
                        CraftingRecipe b = Road;
                        Workers[0].Complete = false;
                        Workers[0].CurrentBuilding = b;
                        b.AmountBuilt += 1;
                        bank.Wood -= b.Wood;
                        bank.Stone -= b.Stone;
                        Workers[0].GetComponent<AI>().SetDestination(g.transform, true);
                    }
                }
            }
        }
        if (Master != null)
        {
            Master.tiles = tiles;
            Master.Tiles2D = Tiles2D;
        }
    }
    
    public bool CanBuild(CraftingRecipe recipe)
    {
        bool cb = true;
        //Debug.Log(bank);
        if (recipe.Wood > bank.Wood || recipe.Stone > bank.Stone)
            cb = false;
        return cb;
    }
    public CraftingRecipe GetNeededBuilding()
    {

        CraftingRecipe cr = CraftingR[0];
        //iterate through each recipe, check if minimum amt has been reached. If so, go through the other buildings. Then, check to see if the max amount/percent has been reached.
        foreach(CraftingRecipe c in CraftingR)
        {
            if(cr.AmountBuilt >= cr.MinAmt)//minimum amt of buildings has been reached
            {
                Debug.Log(cr.Name + ", " + cr.PercentNeeded / BuiltTotal);
                if (c.PercentNeeded/100 <= c.PercentNeeded / BuiltTotal)
                    cr = c;
            }
        }

        return cr;
    }
    public Tile GetGoodTile()
    {
        //get tile closest to the bank
        List<Tile> unfilled = new List<Tile>();
        for(int i = 0; i < tiles.Count; i++)
        {
            if (!tiles[i].Filled)
                unfilled.Add(tiles[i]);
        }
        Debug.Log(unfilled);
        Tile closest = null;
        if(unfilled.Count > 0)
        closest = unfilled[0];
        if (unfilled.Count == 0)
            return null;
        foreach (Tile t in unfilled)
        {//OH NO NOT CAPS LOCK
            if (Vector3.Distance(t.Position, bank.transform.position) < Vector3.Distance(closest.Position, bank.transform.position) && !t.Filled)
            {
                closest = t;
            }
        }
        int x = Mathf.RoundToInt(closest.ArrPos.x);
        int y = Mathf.RoundToInt(closest.ArrPos.y);
        //check to the left and right of the tile to determine if it should be a road

        if(TileExists(x+1, y) && Tiles2D[x + 1][y].Filled && !Tiles2D[x + 1][y].Road || TileExists(x - 1, y) && Tiles2D[x - 1][y].Filled && !Tiles2D[x - 1][y].Road)
        {
            closest.Road = true;
        }
        else
        {
            if (TileExists(x + 1, y) && TileExists(x - 1, y) && Tiles2D[x + 1][y].Road && Tiles2D[x - 1][y].Road)
                closest.Road = true;
        }
        if (!closest.Road)
            if (y % 5 == 0)
                closest.Road = true;
        return closest;
    }
    public bool TileExists(int x, int y)
    {
        bool exists = false;
        var size = Tiles2D.Count;
        if(x >= 0 && y >=0)
        {
            if(x < size && y < size)
            {
                exists = true;
            }
        }
        return exists;
    }
    float timeToGiveJobs = 1f;
    // Update is called once per frame
    void Update()
    {
        timeToGiveJobs -= Time.deltaTime;
        if(timeToGiveJobs <= 0)
        {
            timeToGiveJobs = 1f;
            GiveJobs();
        }
    }
}
