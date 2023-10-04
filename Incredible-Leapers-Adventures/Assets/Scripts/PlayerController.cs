using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] private PlayerStats ps;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private float health = 100f;
    [SerializeField] private float movementSpeed = 10f;
    private float moveScale = 0.15f;
    private float fallTime = 0;
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
    [SerializeField] private int leftWalljumpBonus = 1;
    [SerializeField] private int rightWalljumpBonus = 1;

    private Rigidbody2D rb;
    private Animator animator;

    private bool canMove = true;
    private float xDirection = 0;
    private float yDirection = 0;
    private Vector2 moveDirection = Vector2.zero;
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

    private AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    private bool checkGround = true;
    private bool stopCheckGround = false;

    public System.Action<float> OnPlayerGetDamage;
    public System.Action OnPlayerDie;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<InputHandler>();
        audioSource = GetComponent<AudioSource>();
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
        if (canMove)
        {
            if (inputHandler.xInput != 0)
            {
                if (moveScale < 1)
                    moveScale += 0.04f;
                if (onGround == true)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = clips[6];
                        audioSource.Play();
                    }
                }
            }
            else
            {
                if (moveScale > 0.25f)
                    moveScale -= 0.05f;
            }
        }

        xDirection = inputHandler.xInput * moveScale * movementSpeed; //Input.GetAxis("Horizontal")
        yDirection = rb.velocity.y;

        if (inputHandler.jumpInput) //Input.GetKeyDown(KeyCode.Space)
        {
            inputHandler.jumpInput = false;
            Jump();
        }

        CheckFly();

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

        if (isFall)
        {
            fallTime += Time.deltaTime;
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
                timeToChekGround = 0.1f;
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
            Vector3 rotator;
            if (isFasingRight == true)
            {
                isFasingRight = false;
                rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
            }
            else
            {
                isFasingRight = true;
                rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
            }
            rotator = leftWallCheck.position;
            leftWallCheck.position = rightWallCheck.position;
            rightWallCheck.position = rotator;
        }
    }

    void CheckSurroundings()
    {
        if (checkGround == true)
        {
            if(Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,checkLayer) == true)
            {
                if(onGround == false)
                {
                    if(fallTime > 0.8f)
                    {
                        audioSource.PlayOneShot(clips[8]);
                        fallTime = 0;
                    }
                }
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
        if(xDirection == 0 || isHit == true)
        {
            xDirection = rb.velocity.x;
        }
        moveDirection = new Vector2(xDirection, yDirection);
        rb.velocity = moveDirection;
    }

    public void GetDamage(float damageValue)
    {
        if(isHit == false)
        {
            health -= damageValue;
            jumpCount++;
            audioSource.PlayOneShot(clips[Random.Range(0, 3)]);
            OnPlayerGetDamage?.Invoke(health);
            isHit = true;
            if (health < 0)
            {
                AudioManager.Instance.PlayAudioEffect(clips[7], 2.25f);
                KillPlayer();
            }
            else
            {
                Invoke("StopHit", 0.40f);
            }
        }
    }

    private void KillPlayer()
    {
        StartCoroutine(DestroyPlayer());
    }

    IEnumerator DestroyPlayer()
    {
        animator.Play("Disappearing");
        canMove = false;
        moveScale = 0;
        yield return new WaitForSeconds(0.355f);
        OnPlayerDie?.Invoke();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        gameObject.SetActive(false);

    }

    private void StopHit()
    {
        isHit = false;
    }

    void Jump()
    {
        if(jumpCount > 1)
        {
            isJump = true;
            checkGround = false;
            stopCheckGround = true;
            yDirection = jumpForce;
            jumpCount--;
            if (onWall)
                audioSource.PlayOneShot(clips[3]);
            else
                audioSource.PlayOneShot(clips[Random.Range(4,6)]);
        }
        else if(jumpCount == 1)
        {
            isDoubleJump = true;
            yDirection = jumpForce;
            jumpCount--;
            if (onWall)
                audioSource.PlayOneShot(clips[3]);
            else
                audioSource.PlayOneShot(clips[Random.Range(4, 6)]);
        }
    }

    private void CheckFly()
    {
        if(isFly == true)
        {
            if (yDirection < 0)
            {
                yDirection = 0.9f;
            }
            if (yDirection < 8)
            {
                yDirection += Time.deltaTime * flyForce;
            }
        }
    }

    public void SetFly(bool isFly, bool up)
    {
        this.isFly = isFly;
        if (isFly)
        {
            if (up)
            {
                if (flyForce < 0)
                    flyForce *= -1;
            }
            else
            {
                if (flyForce > 0)
                    flyForce *= -1;
            }
        }
    } 
    public void ThrowCharacter(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            float damage = collision.gameObject.GetComponent<IDamageCheck>().CheckForDamage(transform.position);
            if (damage <= 0)
            {
                rb.AddForce(new Vector2(0, Random.Range(180, 250)));
            }
            else
            {
                if(transform.position.x > collision.transform.position.x)
                {
                    rb.AddForce(new Vector2(Random.Range(150, 250), Random.Range(250, 350)));
                }
                else
                {
                    rb.AddForce(new Vector2(Random.Range(-250, -150), Random.Range(250, 350)));
                }
                isJump = true;
                GetDamage(damage);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy") 
        {
            GetDamage(10f);
            if(transform.position.x > other.transform.position.x)
            {
                ThrowCharacter(new Vector2(Random.Range(200f, 350f), Random.Range(200f, 350f)) );
            }
            else
            {
                ThrowCharacter(new Vector2(-Random.Range(200f, 350f), Random.Range(200f, 350f)));
            }
        }
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
