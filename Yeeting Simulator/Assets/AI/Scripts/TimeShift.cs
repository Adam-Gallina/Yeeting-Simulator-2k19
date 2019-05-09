using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeShift : MonoBehaviour
{
    public float TimeScale = 100;
    public float Multiplier = 1;
    public TMPro.TextMeshPro t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Multiplier = 10;
        }
        else
        {
            Multiplier = 1;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            TimeScale -= 5f * Multiplier;
            if (TimeScale < 0)
                TimeScale = 0;
            TimeScale = Mathf.Round(TimeScale);
            Time.timeScale = (TimeScale/100);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TimeScale += 5f * Multiplier;
            TimeScale = Mathf.Round(TimeScale);
            Time.timeScale = (TimeScale / 100);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        t.text = "Time Speed: " + TimeScale;
    }
}
