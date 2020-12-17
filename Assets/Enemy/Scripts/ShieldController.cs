using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public Renderer rend;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
    }

    public void enableShield()
    {
        rend.enabled = true;
    }

    public void shieldVanish()
    {
        float vanishSpeed = 0.05f;
        float currentThreshold = rend.material.GetFloat("_Threshold");
        if(currentThreshold == 1.0f)
        {
            shieldDestory();
            return;
        } 
        float newThreshold = currentThreshold + Time.deltaTime*vanishSpeed;
        newThreshold = Mathf.Clamp(newThreshold, 0,1);
        rend.material.SetFloat("_Threshold", newThreshold);
    }

    void shieldDestory()
    {
        rend.enabled = false;
    }
}
