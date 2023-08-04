using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float flyForce = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int jumpCount = 2;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask checkLayer;
    private float timeToChekGround = 0.15f;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(1,1);
    [SerializeField] private Vector3 leftWallCheckShift = new Vector3(0.07f, 0, 0);
    [SerializeField] private Vector3 rightWallCheckShift = new Vector3(0.05f, 0, 0);
    [SerializeField] private int leftWalljumpBonus = 1;
    [SerializeField] private int rightWalljumpBonus = 1;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Animator animator;

    private float xDirection = 0;
    private float yDirection = 0;
    private Vector2 Direction = Vector2.zero;
    private bool isFasingRight = true;

    [SerializeField] private State playerState = State.Idle;
    [SerializeField] private bool isHit = false;
    [SerializeField] private bool onGround = true;
    [SerializeField] private bool onWall = false;
    [SerializeField] private bool isRunnig = false;
    [SerializeField] private bool isJump = false;
    [SerializeField] private bool isDoubleJump = false;
    [SerializeField] private bool isFall = false;
    [SerializeField] private bool isWallRight = false;
    [SerializeField] private bool isWallLeft = false;
    [SerializeField] public bool isFly = false;

    private bool checkGround = true;
    private bool stopCheckGround = false;

    public System.Action<float> OnPlayerGetDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = transform.GetChild(0).transform;
        rightWallCheck = transform.GetChild(1).transform;
        rightWallCheck = transform.GetChild(2).transform;
    }

    void Update()
    {
        CheckInput();
        CountdownToCheckGround();
        CheckMovementDirection();
        ApplyMovement();
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
        CheckPlayerState();
        DebugDrawBox(leftWallCheck.position, wallCheckSize, 0, UnityEngine.Color.blue, 1f);
        DebugDrawBox(rightWallCheck.position, wallCheckSize, 0, UnityEngine.Color.red, 1f);
    }

    void CheckInput()
    {
        xDirection = Input.GetAxis("Horizontal") * movementSpeed;
        yDirection = rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Fly();

        if(xDirection != 0)
        {
            isRunnig = true;
        }
        else
        {
            isRunnig = false;
        }

        if (onWall == true)
        {
            if(isWallLeft == true)
            {
                if(xDirection < 0)
                {
                    xDirection = 0;
                }
            }
            if(isWallRight == true) 
            {
                if (xDirection > 0)
                {
                    xDirection = 0;
                }
            }
        }
    }

    private void CountdownToCheckGround()
    {   
        if(stopCheckGround == true)
        { 
            if (timeToChekGround > 0)
            {
                timeToChekGround -= Time.deltaTime;
            }
            else
            {
                checkGround = true;
                stopCheckGround = false;
                timeToChekGround = 0.15f;
            }
        }
    }

    private void CheckPlayerState()
    {   
        if(isHit == true)
        {
            playerState = State.Hit;
        }
        else if (onWall == true)
        {
            playerState = State.WallJump;
        }
        else if(isJump == true)
        {
            playerState = State.Jump;
        }
        else if (isDoubleJump == true)
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
        if (onWall == false)
        {
            if (isFasingRight == true)
            {
                isFasingRight = false;
                //playerSprite.flipX = true;
                Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                //rightWallCheck.position -= rightWallCheckShift;
                //leftWallCheck.position -= leftWallCheckShift;
            }
            else
            {
                isFasingRight = true;
                //playerSprite.flipX = false;
                Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
               // rightWallCheck.position += rightWallCheckShift;
                //leftWallCheck.position += leftWallCheckShift;
            }
        }
    }

    void CheckSurroundings()
    {
        if (checkGround == true)
        {
            if(Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,checkLayer) == true)
            {
                onGround = true;
                isFall = false;
                isJump = false;
                isDoubleJump = false;
                if(onWall == false)
                {
                    jumpCount = 2;
                }
            }
            else
            {
                onGround = false;
                if(yDirection > 0)
                {
                    isFall = false;
                }
                else
                {
                    isFall = true;
                    isJump = false;
                    isDoubleJump = false;
                }
            }
        }
        else
        {
            onGround = false;
        }

        if (Physics2D.OverlapBox(leftWallCheck.position, wallCheckSize, 0, checkLayer) == true)
        {
            if (onGround == false)
                onWall = true;

            if (leftWalljumpBonus > 0)
            {
                if(jumpCount < 2)
                {
                    jumpCount++;
                }
                leftWalljumpBonus--;
            }
            isWallLeft = true;
        }
        else
        {
            isWallLeft = false;
            leftWalljumpBonus = 1;
        }

        if (Physics2D.OverlapBox(rightWallCheck.position, wallCheckSize, 0, checkLayer) == true)
        {
            if (onGround == false)
            onWall = true;

            if (rightWalljumpBonus > 0)
            {
                if (jumpCount < 2)
                {
                    jumpCount++;
                }
                rightWalljumpBonus--;
            }
            isWallRight = true;
        }        
        else
        {
            isWallRight = false;
            rightWalljumpBonus = 1;
        }

        if (isWallLeft == false && isWallRight == false)
        {
            onWall = false;
        }

        if (onWall == true)
        {
            isFall = false;
            if(yDirection < 0)
            {
                rb.gravityScale = 0.75f;
                if(yDirection < -4 )
                {
                    yDirection *= 0.4f;
                }
            }
            else
            {
                if (yDirection > 5)
                {
                    onWall = false;
                }
                    rb.gravityScale = 4;
            }
        }
        else
        {
            rb.gravityScale = 4;
        }

        if(isFly == true)
        {
            onWall = false;
            isJump = true;
            isFall = false;
        }

        
    }

    void ApplyMovement()
    {
        Direction = new Vector2(xDirection, yDirection);
        rb.velocity = Direction;
    }

    public void GetDamage(int damageValue)
    {
        if(isHit == false)
        {
            health -= damageValue;
            OnPlayerGetDamage?.Invoke(health);
            isHit = true;
            if (health < 0)
            {
                KillPlayer();
            }
            else
            {
                Invoke("StopHit", 2);
            }
        }
    }

    private void KillPlayer()
    {
        gameObject.SetActive(false);
    }

    private void StopHit()
    {
        isHit = false;
    }

    void Jump()
    {
        if(jumpCount == 2)
        {
            isJump = true;
            checkGround = false;
            stopCheckGround = true;
            yDirection = jumpForce;
            jumpCount--;
        }
        else if(jumpCount == 1)
        {
            isDoubleJump = true;
            yDirection = jumpForce;
            jumpCount--;
        }
    }

    private  void Fly()
    {
        if(isFly == true)
        {
            if (yDirection < 0)
            {
                yDirection = 0.5f;
            }
            if (yDirection < 8)
            {
                yDirection += Time.deltaTime * flyForce;
            }
        }
    }


    public void ThrowCharacterUp(Vector2 force)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(force);
        /*yDirection = rb.velocity.y;
        if(yDirection > 20f)
        {
            yDirection = 20f;
        }*/
        isJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
        
    void DebugDrawBox(Vector2 point, Vector2 size, float angle, UnityEngine.Color color, float duration)
    {

        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
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
