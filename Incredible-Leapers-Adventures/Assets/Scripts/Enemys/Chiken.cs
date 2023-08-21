using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiken : MonoBehaviour, IDamageCheck
{
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Vector3 moveDirection;
    private float moveScale = 1f;
    private bool isIdle = true;
    [SerializeField] private bool moveLeft = false;
    private bool canGetDamage = true;
    private float immunityTime = 0.25f;
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("/Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (moveLeft == true)
        {
            if (moveDirection.x > 0)
            {
                moveDirection.x *= -1;
            }
        }
        else
        {
            if (moveDirection.x < 0)
            {
                moveDirection.x *= -1;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isIdle)
        {
            if (moveLeft)
            {
                if (transform.position.x + Vector3.right.x > player.position.x)
                {
                    transform.position += moveDirection * moveScale * Time.fixedDeltaTime;
                }
                else
                {
                    moveLeft = false;
                    moveDirection *= -1;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                if (transform.position.x + Vector3.left.x < player.position.x)
                {
                    transform.position += moveDirection * moveScale * Time.fixedDeltaTime;
                }
                else
                {
                    moveLeft = true;
                    moveDirection *= -1;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
        if (canGetDamage == false)
        {
            if (immunityTime > 0)
            {
                immunityTime -= Time.fixedDeltaTime;
            }
            else
            {
                canGetDamage = true;
                immunityTime = 0.25f;
            }
        }
    }

    IEnumerator KillChiken()
    {
        isIdle = false;
        animator.SetInteger("State", 2);
        GetComponent<CompositeCollider2D>().isTrigger = true;
        rb.AddForce(new Vector2(Random.Range(-350f, 350f), 2000f));
        yield return new WaitForSeconds(0.8f);
        animator.SetInteger("State", 1);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (playerPosition.y > transform.position.y + 0.275f)
        {
            if (canGetDamage)
            {
                StartCoroutine(KillChiken());
            }
            return 0;
        }
        else
        {
            immunityTime = 0.25f;
            canGetDamage = false;
            return 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (moveLeft)
            {
                if (transform.position.x + Vector3.right.x < player.position.x)
                {
                    moveLeft = false;
                    moveDirection *= -1;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                if (transform.position.x + Vector3.left.x > player.position.x)
                {
                    moveLeft = true;
                    moveDirection *= -1;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            isIdle = false;
            animator.SetInteger("State", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isIdle = true;
            animator.SetInteger("State", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            moveScale = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveScale = 1;
        }
    }
}
