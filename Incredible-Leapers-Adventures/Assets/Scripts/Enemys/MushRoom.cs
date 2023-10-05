using System.Collections;
using UnityEngine;

public class MushRoom : MonoBehaviour, IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 moveDirection = Vector3.left * 2 ;
    private Vector3 lastPosition;
    private bool canMove = false;
    private bool moveLeft = true;
    private float speedUpTime = 2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        lastPosition.x += 0.5f;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (moveLeft)
            {
                if (lastPosition.x - transform.position.x < 0.01f)
                {
                    lastPosition = transform.position;
                    lastPosition.x -= 0.5f;
                    moveDirection.x *= -1;
                    moveLeft = false;
                }
                else
                {
                    lastPosition = transform.position;
                    if(speedUpTime > 0)
                    {
                        speedUpTime -= Time.fixedDeltaTime;
                        if(moveDirection.x < -2)
                        {
                            moveDirection.x += Time.fixedDeltaTime;
                        }
                    }
                    transform.Translate(moveDirection * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (transform.position.x - lastPosition.x < 0.01f)
                {
                    lastPosition = transform.position;
                    lastPosition.x += 0.5f;
                    moveDirection.x *= -1;
                    moveLeft = true;
                }
                else
                {
                    lastPosition = transform.position;
                    if (speedUpTime > 0)
                    {
                        speedUpTime += Time.fixedDeltaTime;
                        if (moveDirection.x > 2)
                        {
                            moveDirection.x -= Time.fixedDeltaTime;
                        }
                    }
                    transform.Translate(moveDirection * Time.fixedDeltaTime);
                }
            }
        }
    }

    IEnumerator KillMushRoom()
    {
        AudioManager.Instance.PlayEnemyDeathAudioEffect();
        animator.SetInteger("State", 2);
        speedUpTime = -1;
        rb.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().isTrigger = true;
        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range(-75f, 75f);
        rb.AddForce(new Vector2(Random.Range(-250f, 250f), Random.Range(200f, 300f)));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (playerPosition.y > transform.position.y + 0.1f)
        {
            StartCoroutine(KillMushRoom());
            return 0;
        }
        else
        {
            return 5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(speedUpTime >= 0)
            {
                animator.SetInteger("State", 1);
                canMove = true;
                lastPosition = transform.position;
                speedUpTime = 2f;
                if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    moveLeft = true;
                    lastPosition.x += 0.5f;
                    moveDirection.x = -4f;
                }
                else
                {
                    moveLeft = false;
                    lastPosition.x -= 0.5f;
                    moveDirection.x = 4f;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(speedUpTime >= 0)
            animator.SetInteger("State", 0);
            canMove = false;
        }
    }
}
