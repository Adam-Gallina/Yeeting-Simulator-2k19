using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentBuildNav : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            if(!m.GetComponent<NavMeshSourceTag>())
            {
                m.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
