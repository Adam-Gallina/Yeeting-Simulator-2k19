using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AIBank : MonoBehaviour
{
    public int Wood;
    public int Stone;
    public int Food;
    public Transform[] EntryPoints;
    [SerializeField] TextMeshPro log;
    float timeToClearLog = 4;
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Deposit(AI ai)
    {
        log.text = "";
        timeToClearLog = 4;
        log.color = new Color(log.color.r, log.color.g, log.color.b, 1);
        int[] collected = { ai.resources.Wood, ai.resources.Stone, ai.resources.Food};
        string logged = "";
        if (collected[0] > 0)
            logged += "+ " + collected[0] + " Wood \n";
        if (collected[1] > 0)
            logged += "+ " + collected[1] + " Stone \n";
        if (collected[2] > 0)
            logged += "+ " + collected[2] + " Food \n";
        log.text = logged;
        Wood += collected[0];
        Stone += collected[1];
        Food += collected[2];
        ai.resources.Stone = 0;
        ai.resources.Wood = 0;
        ai.resources.Food = 0;
        ai.resources.CurrentTotal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToClearLog > 0)
        {
            timeToClearLog -= Time.deltaTime;
        }
        if(timeToClearLog <= 0 && log.color.a > 0)
        {
            log.color = new Color(log.color.r, log.color.g, log.color.b, log.color.a - Time.deltaTime);
        }
    }
}
