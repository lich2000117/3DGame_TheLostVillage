using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float acceleratedSpeed = 18f;
    public float gravity = -9.81f;
    public float jumpHeight = 10f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    AudioManage audioManage;
    bool isGrounded;

    void Start() {
        audioManage = GetComponent<AudioManage>();
    }

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);



        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        controller.Move(velocity * Time.deltaTime);


        if (direction.magnitude >= 0.1f) {
            // Atan2 returns angle between x-axis and the vector starting at 0
            // and terminating at x,y
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift)) {
                controller.Move(moveDir.normalized * acceleratedSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)){
                // audioManage.walkSound.Play();
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

        }
    }

    public bool isOnGround(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }
}
