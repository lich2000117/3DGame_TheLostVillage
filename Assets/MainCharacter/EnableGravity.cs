using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    Vector3 velocity;
    public CharacterController controller;

    // gravity is always pulling towards downside
    void Update()
    {
        //bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //if (isGrounded && velocity.y < 0) {
            //velocity.y = -2f;
        //}
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
