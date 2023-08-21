using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BlueBird : MonoBehaviour, IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool moveLeft = true;
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        
        if(moveLeft)
        {
            if(transform.position.x > leftPoint)
            {
                transform.position -= moveDirection * Time.fixedDeltaTime;
            }
            else
            {
                moveLeft = false;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (transform.position.x < rightPoint)
            {
                transform.position += moveDirection * Time.fixedDeltaTime;
            }
            else
            {
                moveLeft = true;
                spriteRenderer.flipX = false;
            }
        }
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (transform.position.y + 0.15f < playerPosition.y)
        {
            StartCoroutine(KIllBird());
            return 0;
        }
        else
        {
            return 10f;
        }      
    }

    IEnumerator KIllBird()
    {
        moveDirection = Vector3.zero;
        animator.SetInteger("State", 1);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<CircleCollider2D>().enabled = false;
        rb.AddForce(new Vector2(Random.Range(-150f, 150f), Random.Range(150f, 250f)));
        rb.angularVelocity = Random.Range(20f, 120f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    
}
