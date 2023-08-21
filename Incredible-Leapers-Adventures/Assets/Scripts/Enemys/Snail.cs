using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Snail : MonoBehaviour, IDamageCheck
{
    [SerializeField] private Vector3 moveDirection = Vector3.left;
    [SerializeField]private bool isSnail = true;
    [SerializeField]private bool moveLeft = true;
    private float moveTime = 2f;
    private float stayTime = 2f;
    private bool isShell = false;
    private bool isDamage = false;
    [SerializeField] private Vector3 leftPoint = Vector3.zero;
    [SerializeField] private Vector3 rightPoint = Vector3.zero;
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject snail;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        snail = transform.GetChild(0).gameObject;
        snail.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isSnail)
        {
            if (moveTime > 0)
            {
                if (moveLeft)
                {
                    if(transform.localPosition.x > leftPoint.x)
                    {
                        transform.position += moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        animator.SetInteger("State",0);
                        moveLeft = false;
                        moveTime = 0;
                        stayTime = Random.Range(1.5f, 2f);
                        moveDirection *= -1;
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                else
                {
                    if (transform.localPosition.x < rightPoint.x)
                    {
                        transform.position += moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        animator.SetInteger("State", 0);
                        moveLeft = true;
                        moveTime = 0;
                        stayTime = Random.Range(1.5f, 2f);
                        moveDirection *= -1;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
                moveTime -= Time.fixedDeltaTime;
            }
            else
            {
                if(stayTime > 0)
                {
                    stayTime -= Time.fixedDeltaTime;
                }
                else
                {
                    moveTime = Random.Range(1.5f, 3f);
                    animator.SetInteger("State", 1);
                }
            }
        }
        else
        {
            if (isShell)
            {
                if (moveLeft)
                {
                    if (transform.localPosition.x > leftPoint.x)
                    {
                        transform.position += moveDirection * 6 * Time.fixedDeltaTime;
                        animator.SetInteger("State", 0);
                    }
                    else
                    {
                        animator.SetInteger("State", 3);
                        moveLeft = false;
                        moveDirection *= -1;
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                else
                {
                    if (transform.localPosition.x < rightPoint.x)
                    {
                        transform.position += moveDirection * 6 * Time.fixedDeltaTime;
                        animator.SetInteger("State", 0);
                    }
                    else
                    {
                        animator.SetInteger("State", 3);
                        moveLeft = true;
                        moveDirection *= -1;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }

                if(isDamage)
                {
                    if(stayTime > 0)
                    {
                        stayTime -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        isDamage = false;
                    }
                }
            }
        }
    }

    IEnumerator KillSnail()
    {
        snail.SetActive(true);
        Rigidbody2D snailRB = snail.GetComponent<Rigidbody2D>();
        snailRB.AddForce(new Vector2(moveDirection.x * 0.25f, 300f));
        yield return new WaitForFixedUpdate();
        snailRB.angularVelocity = Vector3.forward.z;
        yield return new WaitForSeconds(2);
        snail.SetActive(false);
    }

    IEnumerator KillShell()
    {
        isShell = false;
        animator.SetInteger("State", 4);
        GetComponent<CircleCollider2D>().enabled = false;
        rb.AddForce(new Vector2(Random.Range(-1000f, 1000f), Random.Range(800f, 1200f)));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (playerPosition.y > transform.position.y + 0.205f)
        {
            if (isSnail)
            {
                animator.SetInteger("State", 2);
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().offset *= new Vector2(0.4f, -2f);
                StartCoroutine(KillSnail());
                isSnail = false;
                isShell = true;
                isDamage = true;
                stayTime = 0.4f;
                return 0f;
            }
            else
            {
                if(isDamage == false)
                {
                    StartCoroutine(KillShell());
                }
                return 0f;
            }
        }
        else
        {
            if (!isSnail)
            {
                return 10f;
            }
            else
            {
                return 0;
            }
        }
    }
}
