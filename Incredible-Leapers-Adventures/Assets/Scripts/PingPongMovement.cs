using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;

    private bool isHorizontal = true;
    private bool isStop = false;
    private bool goToEndPoint = true;
    [SerializeField] private float stopTime = 2f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if(isHorizontal == true)
        {
            moveDirection.x = moveSpeed;
        }
        else
        {
            moveDirection.y = moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isStop == false)
        {
            if (isHorizontal == true)
            {
                if (goToEndPoint == true)
                {
                    if (transform.position.x < endPoint.x)
                    {
                        transform.position += moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        goToEndPoint = false;
                        animator.SetBool("isStop", isStop);
                    }

                }
                else
                {
                    if (transform.position.x > startPoint.x)
                    {
                        transform.position -= moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        goToEndPoint = true;
                        animator.SetBool("isStop", isStop);
                    }
                }
            }
            else
            {
                if (goToEndPoint == true)
                {
                    if (transform.position.y < endPoint.y)
                    {
                        transform.position += moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        goToEndPoint = false;
                        animator.SetBool("isStop", isStop);
                    }

                }
                else
                {
                    if (transform.position.y > startPoint.y)
                    {
                        transform.position -= moveDirection * Time.fixedDeltaTime;
                    }
                    else
                    {
                        isStop = true;
                        goToEndPoint = true;
                        animator.SetBool("isStop", isStop);
                    }
                }
            }
        }
        else
        {
            if(stopTime > 0)
            {
                stopTime -= Time.fixedDeltaTime;
            }
            else
            {
                isStop = false;
                animator.SetBool("isStop", isStop);
                stopTime = 2;
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
