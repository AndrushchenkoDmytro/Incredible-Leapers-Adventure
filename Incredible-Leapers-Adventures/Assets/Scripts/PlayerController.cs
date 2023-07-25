using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int jumpCount = 2;
    [SerializeField] private Transform groundCheker;
    [SerializeField] private float groundChekerRadius = 0.25f;
    [SerializeField] private LayerMask ground;
    private float timeToChekGround = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Animator animator;

    private float xDirection = 0;
    private float yDirection = 0;
    private Vector2 Direction = Vector2.zero;
    private bool isFasingRight = true;

    private State playerState = State.Idle;
    [SerializeField] private bool onGround = true;
    [SerializeField] private bool isRunnig = false;
    [SerializeField] private bool isJump = false;
    [SerializeField] private bool isDoubleJump = false;
    [SerializeField] private bool isFall = false;

    private bool checkGround = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheker = transform.GetChild(0).transform;
    }

    void Update()
    {
        CheckInput();
        CountdownToCheckGround();
        ApplyMovement();
        CheckMovementDirection();
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
        CheckPlayerState();
    }

    void CheckInput()
    {
        xDirection = Input.GetAxis("Horizontal") * movementSpeed;
        yDirection = rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(xDirection != 0)
        {
            isRunnig = true;
        }
        else
        {
            isRunnig = false;
        }

        if(yDirection > 0.1f)
        {
            onGround = false;
            if (yDirection > 16)
            {
                yDirection = 16;
            }
        }
        else
        {
            isJump = false;
            isDoubleJump = false;
            if(onGround == false)
            {
                isFall = true;
            }
            else
            {
                isFall = false;
            }
        }
    }

    private void CountdownToCheckGround()
    {
        if(timeToChekGround > 0)
        {
            timeToChekGround -= Time.deltaTime;
        }
        else
        {
            checkGround = true;
        }
    }

    private void CheckPlayerState()
    {
        if(isJump == true)
        {
            playerState = State.Jump;
        }
        else if(isDoubleJump == true)
        {
            playerState = State.DoubleJump;
        }
        else if(isFall == true)
        {
            playerState = State.Fall;
        }
        else if(isRunnig == true)
        {
            playerState = State.Run;
        }
        else
        {
            playerState = State.Idle;
        }
        animator.SetInteger("PlayerState",(int)playerState);
    }

    void CheckMovementDirection()
    {
        if(isFasingRight == true && xDirection < 0)
        {
            FlipCharacterRotation();

        }
        else if(isFasingRight == false && xDirection > 0)
        {
            FlipCharacterRotation();
        }
    }

    void FlipCharacterRotation()
    {
        isFasingRight = !isFasingRight;
        playerSprite.flipX = !isFasingRight;
    }

    void CheckSurroundings()
    {
        if (checkGround == true)
        {
            if(Physics2D.OverlapCircle(groundCheker.position, groundChekerRadius,ground) == true)
            {
                onGround = true;
                jumpCount = 2;
                checkGround = false;
                timeToChekGround = 0.15f;
            }
        }
    }

    void ApplyMovement()
    {
        Direction = new Vector2(xDirection, yDirection);
        rb.velocity = Direction;
    }

    void Jump()
    {
        if(jumpCount == 2)
        {
            isJump = true;
            isFall = false;
            //waitBeforCheckGround = 0.2f;
            yDirection = jumpForce;
            jumpCount--;
        }
        else if(jumpCount == 1)
        {
            isDoubleJump = true;
            isFall = false;
            //rb.AddForce(new Vector2(0, jumpForce * 1.5f));
            yDirection = jumpForce;
            jumpCount--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheker.position, groundChekerRadius);
    }
}

public enum State
{
    Idle,
    Run,
    Fall,
    WallJump,
    DoubleJump,
    Jump,
    Hit,
}
