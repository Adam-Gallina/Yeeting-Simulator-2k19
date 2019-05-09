using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthType { Innocent, Yeeter, None};
public class Health : MonoBehaviour
{
    YeetedAI ai;
    public HealthType type;
    public float CurrentHealth;
    public float MaxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        if (GetComponent<YeetedAI>())
        {
            ai = GetComponent<YeetedAI>();
            if(ai.GetComponent<AI>().r.type == RoleType.Yeeter || ai.GetComponent<AI>().r.type == RoleType.Hunter)
                type = HealthType.Yeeter;
        }
    }
    public void TakeDamage(GameObject source, float amt)
    {
        CurrentHealth -= amt;
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
