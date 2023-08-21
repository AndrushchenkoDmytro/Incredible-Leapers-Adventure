using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime : MonoBehaviour,IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField]  private LayerMask layerMask;
    RaycastHit2D ray;
    private Vector2 jumpDirection;
    [SerializeField] private bool canJump = false;
    [SerializeField] private bool jumpOnSpot = false;
    [SerializeField] private int jumpOnSpotCount = 0;
    [SerializeField] private bool moveLeft = true;
    private float waitTime = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpDirection = new Vector2(Random.Range(-400f, -200f), Random.Range(200f, 310f));
    }

    private void FixedUpdate()
    {
        if (canJump)
        {
            if (jumpOnSpot == true)
            {
                ChangeDirection();
                jumpDirection.x = 0;
                rb.AddForce(jumpDirection);
                canJump = false;
            }
            else
            {
                rb.AddForce(jumpDirection);
                canJump = false;
                jumpOnSpot = true;
            }
        }
        if(rb.velocity.y == 0)
        {
            if(waitTime <= 0)
            {
                ray = Physics2D.Raycast(transform.position, Vector3.down, 0.5f, layerMask);
                Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.blue);
                if (ray.collider != null)
                {
                    canJump = true;
                    if (jumpOnSpot == true)
                    {
                        jumpOnSpotCount++;
                        if (jumpOnSpotCount > 2)
                        {
                            jumpOnSpot = false;
                            jumpOnSpotCount = 0;
                            ChangeDirection();
                        }
                    }
                }
                waitTime = 0.1f;
            }
            else
            {
                waitTime -= Time.fixedDeltaTime;
            }

        }
    }

    private void ChangeDirection()
    {
        if (jumpOnSpot)
        {
            jumpDirection *= 0.7f; 
        }
        else
        {
            if (moveLeft)
            {
                jumpDirection = new Vector2(Random.Range(160f, 320f), Random.Range(200f, 310f));
                transform.eulerAngles = new Vector3(0, 180, 0);
                moveLeft = false;
            }
            else
            {
                jumpDirection = new Vector2(Random.Range(-320f, -160f), Random.Range(200f, 310));
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;
            }
        }
    }

    IEnumerator KillSlime()
    {
        animator.SetInteger("State", 1);
        rb.velocity = Vector2.zero;
        GetComponent<CompositeCollider2D>().isTrigger = true;
        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range(-75f, 75f);
        rb.AddForce(new Vector2(Random.Range(-250f, 250f), Random.Range(200f, 300f)));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if(playerPosition.y > transform.position.y + 0.275f)
        {
            StartCoroutine(KillSlime());
            return 0;
        }
        else
        {
            return 15f;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (collision.transform.position.y > transform.position.y + 0.25f)
            {
                playerController.ThrowCharacter(jumpDirection * 1.5f);
                StartCoroutine(KillSlime());
            }
            else
            {
                playerController.ThrowCharacter(jumpDirection);
                playerController.GetDamage(15);
            }
        }
    }*/
}
