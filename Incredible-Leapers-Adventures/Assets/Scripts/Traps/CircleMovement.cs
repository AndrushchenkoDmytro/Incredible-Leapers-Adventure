using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] MovePoint;
    [SerializeField] private int currentIndex;
    [SerializeField] private bool isClockWise;
    private bool isStop = false;
    [SerializeField] private float stopTime = 2f;

    private Vector3 moveDirection;
    private float[] distancesBetweenPoints = new float[4];
    private float currentDistance = 0;

    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] private AnimationCurve animationCurve;
    private float curveTime = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        transform.position = MovePoint[currentIndex];

        for (int i = 0; i < distancesBetweenPoints.Length; i++)
        {
            if(i != 3)
            {
                distancesBetweenPoints[i] = Vector2.Distance(MovePoint[i], MovePoint[i + 1]);
            }
            else
            {
                distancesBetweenPoints[i] = Vector2.Distance(MovePoint[3], MovePoint[0]);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStop == false)
        {
            if (isClockWise)
            {
                if(currentIndex == 0)
                {
                    if(transform.position.y < MovePoint[1].y)
                    {
                        moveDirection = Vector2.up * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[1]);
                        curveTime = currentDistance / distancesBetweenPoints[0];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 1);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else if(currentIndex == 1)
                {
                    if (transform.position.x < MovePoint[2].x)
                    {
                        moveDirection = Vector2.right * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[2]);
                        curveTime = currentDistance / distancesBetweenPoints[1];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 2);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else if (currentIndex == 2)
                {
                    if (transform.position.y > MovePoint[3].y)
                    {
                        moveDirection = Vector2.down * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[3]);
                        curveTime = currentDistance / distancesBetweenPoints[2];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 3);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else
                {
                    if (transform.position.x > MovePoint[0].x)
                    {
                        moveDirection = Vector2.left * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[0]);
                        curveTime = currentDistance / distancesBetweenPoints[3];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        
                        currentIndex = 0;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 0);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
            }
            else
            {
                if (currentIndex == 0)
                {
                    if (transform.position.y > MovePoint[1].y)
                    {
                        moveDirection = Vector2.down * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[1]);
                        curveTime = currentDistance / distancesBetweenPoints[0];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 2);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else if (currentIndex == 1)
                {
                    if (transform.position.x < MovePoint[2].x)
                    {
                        moveDirection = Vector2.right * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[2]);
                        curveTime = currentDistance / distancesBetweenPoints[1];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 1);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else if (currentIndex == 2)
                {
                    if (transform.position.y < MovePoint[3].y)
                    {
                        moveDirection = Vector2.up * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[3]);
                        curveTime = currentDistance / distancesBetweenPoints[2];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex++;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 0);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
                else
                {
                    if (transform.position.x < MovePoint[0].x)
                    {
                        moveDirection = Vector2.left * moveSpeed;
                        currentDistance = Vector2.Distance(transform.position, MovePoint[3]);
                        curveTime = currentDistance / distancesBetweenPoints[3];
                        transform.position += moveDirection * animationCurve.Evaluate(1 - curveTime) * Time.fixedDeltaTime;
                    }
                    else
                    {
                        currentIndex = 0;
                        transform.position = MovePoint[currentIndex];
                        isStop = true;
                        animator.SetInteger("pointIndex", 3);
                        animator.SetBool("isStop", isStop);
                        audioSource.Play();
                    }
                }
            }
        }
        else
        {
            if (stopTime > 0)
            {
                if (isStop == true)
                {
                    animator.SetBool("isStop", false);
                }
                stopTime -= Time.fixedDeltaTime;
            }
            else
            {
                isStop = false;
                stopTime = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(20);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(20);
        }
    }

}
