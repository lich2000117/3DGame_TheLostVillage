using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//For player character currently
public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float currentSpeed = 0;
    

    
    void Update()
    {
        float translation = Input.GetAxis("Vertical")*speed;
        float rotation = Input.GetAxis("Horizontal")*rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0,0, translation);
        currentSpeed = translation;

        transform.Rotate(0,rotation,0);
    }
}
