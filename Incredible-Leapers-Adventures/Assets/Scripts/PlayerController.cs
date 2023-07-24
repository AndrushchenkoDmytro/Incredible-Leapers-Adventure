using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private float horizonatDirection = 0;
    private float verticalDirection = 0;
    private Vector2 generalDirection = Vector2.zero;

    private bool isFasingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        ApplyMovement();
    }

    void CheckInput()
    {
        horizonatDirection = Input.GetAxis("Horizontal") * 10f;
        verticalDirection = rb.velocity.y;

        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    void CheckMovementDirection()
    {
        if(isFasingRight == true && horizonatDirection < 0)
        {
            FlipCharacterRotation();

        }
        else if(isFasingRight == false && horizonatDirection > 0)
        {
            FlipCharacterRotation();
        }
    }

    void FlipCharacterRotation()
    {
        isFasingRight = !isFasingRight;
        transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
    }

    void ApplyMovement()
    {
        generalDirection = new Vector2(horizonatDirection, verticalDirection);
        rb.velocity = generalDirection;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce));
    }
}
