using UnityEngine;

public class PingPongRockHead : MonoBehaviour
{
    [SerializeField] private bool moveHorizontal;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    private Vector3 moveDirection;
    private bool moveFromStartToEnd;
    private bool isStop = false;
    [SerializeField] private float stopTime = 2f;

    private AudioSource audioSource;
    private Animator animator;

    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (moveHorizontal) moveDirection = Vector3.right;
        else moveDirection = Vector3.up;

        if(moveSpeed > 0) moveFromStartToEnd = true;
        else moveFromStartToEnd = false;
    }

    private void FixedUpdate()
    {

        if (isStop == false)
        {
            if (moveHorizontal)
            {
                if (moveFromStartToEnd == true)
                {
                    if (transform.localPosition.x < endPoint.x)
                    {
                        transform.localPosition += moveDirection * moveSpeed * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        audioSource.Play();
                        ChangeAnimationToHit();
                        Invoke("ChangeAnimationToMove", 0.355f);
                    }
                }
                else
                {
                    if (transform.localPosition.x > startPoint.x)
                    {
                        transform.localPosition += moveDirection * moveSpeed * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        audioSource.Play();
                        ChangeAnimationToHit();
                        Invoke("ChangeAnimationToMove", 0.355f);
                    }
                }
            }
            else
            {
                if (moveFromStartToEnd == true)
                {
                    if (transform.localPosition.y < endPoint.y)
                    {
                        transform.localPosition += moveDirection * moveSpeed * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        audioSource.Play();
                        ChangeAnimationToHit();
                        Invoke("ChangeAnimationToMove", 0.355f);
                    }
                }
                else
                {
                    if (transform.localPosition.y > startPoint.y)
                    {
                        transform.localPosition += moveDirection * moveSpeed * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        audioSource.Play();
                        ChangeAnimationToHit();
                        Invoke("ChangeAnimationToMove", 0.355f);
                    }
                }
            }
           
        }
        else
        {
            if (stopTime > 0)
            {
                stopTime -= Time.fixedDeltaTime;
            }
            else
            {
                isStop = false;
                moveFromStartToEnd = !moveFromStartToEnd;
                moveSpeed *= -1;
                stopTime = 2;
            }
        }
    }

    private void ChangeAnimationToHit()
    {      
        if (moveHorizontal)
        {
            if (moveFromStartToEnd)
            {
                animator.SetInteger("pointIndex", 2);
            }
            else
            {
                animator.SetInteger("pointIndex", 4);
            }
        }
        else
        {
            if (moveFromStartToEnd)
            {
                animator.SetInteger("pointIndex", 1);
            }
            else
            {
                animator.SetInteger("pointIndex", 3);
            }
        }
    }
    private void ChangeAnimationToMove()
    {
        animator.SetInteger("pointIndex", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(playerController == null)
            {
                playerController = collision.gameObject.GetComponent<PlayerController>();
            }
            playerController.GetDamage(20);
            isStop = true;
            stopTime = 0.4f;
            audioSource.Play();
            ChangeAnimationToHit();
            Invoke("ChangeAnimationToMove", 0.355f);
        }
    }
}

