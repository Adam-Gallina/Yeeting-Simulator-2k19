using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMaker : MonoBehaviour
{
    public int Left;
    public bool Reset;
    public int Forward;
    [Range(-3, 3)]
    public float HeightMultiplier;
    public int Smoothing = 5;
    Transform parent;
    public List<List<GameObject>> Chunks = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }
    void AddChunk(int x, int y, int height)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.SetParent(parent);
        g.transform.position = new Vector3(x, height, y);
        g.name = "[" + x + ", " + y + "]";
        Chunks[x].Add(g);
    }
    float nearbyHeight(int x, int y)
    {
        float height = -50;
        if (TileExists(x, y))
        {
            height = 0;
            height += chunkH(x, y);
        }
        return height;
    }
    public float NearbyHeights(int x, int y)
    {
        int nbh = 0;
        float height = 1;
        var xx = -1;
        var yy = 0;
        if(nearbyHeight(x + xx, y + yy) != -50)//left
        {
            nbh += 1;
            height += nearbyHeight(x + xx, y + yy);
        }
        xx = 1;
        if (nearbyHeight(x + xx, y + yy) != -50)//right
        {
            nbh += 1;
            height += nearbyHeight(x + xx, y + yy);
        }
        xx = 0;
        yy = 1;
        if (nearbyHeight(x + xx, y + yy) != -50)//up
        {
            nbh += 1;
            height += nearbyHeight(x + xx, y + yy);
        }
        yy = -1;
        if (nearbyHeight(x + xx, y + yy) != -50)//down
        {
            nbh += 1;
            height += nearbyHeight(x + xx, y + yy);
        }
        return (height);
    }
    float chunkH(int x, int y)
    {
        return Chunks[x][y].transform.position.y;
    }
    public bool TileExists(int x, int y)
    {
        bool exists = false;
        var size = Chunks.Count;
        if (x >= 0 && y >= 0)
        {
            if (x < size && y < size)
            {
                exists = true;
            }
        }
        return exists;
    }
    public void Generate()
    {
        Chunks = new List<List<GameObject>>();
        parent = new GameObject().transform;
        for(int x = 0; x < Left; x++)
        {
            Chunks.Add(new List<GameObject>());
            for(int y = 0; y < Forward; y++)
            {
                AddChunk(x, y, 0);
            }
        }
        for (int x = 0; x < Chunks.Count - 1; x++)
        {
            for (int y = 0; y < Chunks[x].Count - 1; y++)
            {
                var c = Chunks[x][y].transform;
                c.position = new Vector3(c.position.x, 0, c.position.z);
                Debug.Log(c.position);
                if(Random.Range(0, 25) == 5)
                {
                    c.transform.position += new Vector3(0, Random.Range(-1, 1)/10 + NearbyHeights(x, y), 0);
                }
            }
        }
        for (int x = 0; x < Chunks.Count - 1; x++)
        {
            for (int y = 0; y < Chunks[x].Count - 1; y++)
            {
                var c = Chunks[x][y].transform;
                c.position = new Vector3(c.position.x, NearbyHeights(x, y)*HeightMultiplier, c.position.z);
            }
        }
        for (int i = 0; i < Smoothing; i++)
            ReArrange();
    }
    public void ReArrange()
    {
        for (int x = 0; x < Chunks.Count - 1; x++)
        {
            for (int y = 0; y < Chunks[x].Count - 1; y++)
            {
                var c = Chunks[x][y].transform;
                c.position = new Vector3(c.position.x, NearbyHeights(x, y) * HeightMultiplier/5, c.position.z);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Reset)
        {
            Reset = false;
            Destroy(parent.gameObject);
            Generate();
        }
    }
}
