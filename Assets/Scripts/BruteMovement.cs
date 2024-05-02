using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteMovement : MonoBehaviour
{
    public CharacterController controller;
    private Animator animator;

    public float speed = 25f;
    public float gravity = -78.48f;
    public float jumpHeight = 6f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Grounded: {isGrounded}, Moving: {animator.GetBool("isMoving")}, Jumping: {animator.GetBool("isJumping")}, Falling: {animator.GetBool("isFalling")}");
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // jumping speed
            animator.SetBool("isJumping", true);
            isJumping = true;
        }

        if (!isGrounded)
        {
            animator.SetBool("isGrounded", false);

            if ((isJumping && velocity.y < 0) || (velocity.y < -2f))
            {
                animator.SetBool("isFalling", true);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
