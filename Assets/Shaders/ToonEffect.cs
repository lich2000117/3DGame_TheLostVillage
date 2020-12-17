using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonEffect : MonoBehaviour
{
    public Shader toonShader;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.SetReplacementShader(toonShader,"");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
