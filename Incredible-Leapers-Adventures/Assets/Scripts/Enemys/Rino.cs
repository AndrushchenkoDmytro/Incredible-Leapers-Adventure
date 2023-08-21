using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rino : MonoBehaviour, IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private AnimationCurve curve;

    public float leftPointX;
    public float rightPointX;
    [SerializeField] private Transform overlapPOs;
    [SerializeField] private Vector3 moveDirection = Vector3.left;
    [SerializeField] private bool moveLeft = true;

    public float stayTime = 0;
    public float moveTime = 0;

    [SerializeField] private bool isIdle = true;
    public bool hasTarget = false;
    [SerializeField] private bool isWallHit = false;
    private bool isDie = false;

    private void Awake()
    {
        stayTime = Random.Range(2f, 3f);

        if (!moveLeft)
        {
            if(moveDirection.x < 0)
            moveDirection *= -1;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isIdle)
        {
            if (moveLeft)
            {
                if (transform.localPosition.x > leftPointX)
                {
                    moveTime += Time.fixedDeltaTime;
                    if (hasTarget == true)
                    {
                        transform.position += moveDirection * curve.Evaluate(moveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (moveTime < 2)
                        {
                            transform.position += moveDirection * 0.1f * Time.fixedDeltaTime;
                        }
                        else
                        {
                            animator.SetInteger("State", 0);
                            moveTime = 0;
                            stayTime = Random.Range(2f, 3f);
                            isIdle = true;                 
                        }

                    }
                }
                else
                {
                    moveTime = 0;
                    stayTime = Random.Range(1.5f, 2f);
                    moveLeft = false;
                    isIdle = true;
                    moveDirection *= -1;

                    if (Physics2D.OverlapCircle(transform.position + Vector3.Normalize(moveDirection) * 1.5f, 0.5f, LayerMask.NameToLayer("Ground")) == true)
                    {
                        isWallHit = true;
                        if (hasTarget)
                        {
                            rb.constraints = RigidbodyConstraints2D.None;
                            rb.AddForce(new Vector2(650f, 13000f));
                            animator.SetInteger("State", 2);
                        }
                        else
                        {
                            animator.SetInteger("State", 0);
                        }
                    }
                    else
                    {
                        animator.SetInteger("State", 0);
                    }
                }
            }
            else
            {
                if (transform.localPosition.x < rightPointX)
                {
                    moveTime += Time.fixedDeltaTime;
                    if (hasTarget)
                    {
                        transform.position += moveDirection * curve.Evaluate(moveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (moveTime < 2)
                        {
                            transform.position += moveDirection * 0.1f * Time.fixedDeltaTime;
                        }
                        else
                        {
                            animator.SetInteger("State", 0);
                            moveTime = 0;
                            stayTime = Random.Range(2f, 3f);
                            isIdle = true;
                        }

                    }
                }
                else
                {
                    moveTime = 0;
                    stayTime = Random.Range(1.5f, 2f);
                    moveLeft = true;
                    isIdle = true;
                    moveDirection *= -1;


                    if (Physics2D.OverlapCircle(overlapPOs.position, 0.2f, LayerMask.NameToLayer("Ground")) == true)
                    {
                        isWallHit = true;
                        rb.constraints = RigidbodyConstraints2D.None;
                        rb.AddForce(new Vector2(650f, 13000f));
                        animator.SetInteger("State", 2);
                    }
                    else
                    {
                        animator.SetInteger("State", 0);
                    }
                }
            }
        }
        else
        {
            if(stayTime > 0)
            {
                stayTime -= Time.fixedDeltaTime;
            }
            else
            {
                if(moveLeft == true)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                isWallHit = false;
                isIdle = false;
                animator.SetInteger("State", 1);
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }

    IEnumerator KillRinoCoroutine()
    {
        stayTime = 4f;
        isDie = true;
        isIdle = true;
        animator.SetInteger("State", 3);
        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(new Vector2(Random.Range(-3000, 3000), 12000f));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (playerPosition.y > transform.position.y + 0.275f)
        {
            if (stayTime <= 0.25)
            {
                StartCoroutine(KillRinoCoroutine());
            }
            return 0;
        }
        else
        {
            if (stayTime <= 0)
            {
                if (moveLeft)
                {
                    if (transform.position.x > playerPosition.x)
                    {
                        if (transform.position.y + 0.45f > playerPosition.y)
                        {
                            return 26;
                        }
                    }
                    return 0;
                }
                else
                {
                    if (transform.position.x < playerPosition.x)
                    {
                        if (transform.position.y + 0.45f > playerPosition.y)
                        {
                            return 26;
                        }
                    }
                    return 0;
                }
            }
            return 0;
        }
    }
}
