using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Chameleon : MonoBehaviour, IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private CapsuleCollider2D capsuleCollider2D;
    private AudioSource audioSource;
    [SerializeField] AudioClip attack;

    [SerializeField] private bool isMove = true;
    private bool canMove = true;
    private bool moveLeft = true;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] private float leftPoint = 0;
    [SerializeField] private float rightPoint = 0;

    private int attackFrame = -1;
    private bool isAttack = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isMove == true)
        {
            if(moveLeft)
            {
                if(transform.position.x > leftPoint)
                {
                    rb.velocity = Vector3.left * moveSpeed;
                }
                else
                {
                    moveLeft = false;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    transform.position = new Vector3(transform.position.x + 0.6f, transform.position.y, 0);
                }
            }
            else
            {
                if (transform.position.x < rightPoint)
                {
                    rb.velocity = Vector3.right * moveSpeed;
                }
                else
                {
                    moveLeft = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    transform.position = new Vector3(transform.position.x - 0.6f, transform.position.y, 0);
                }
            }
        }
        if (isAttack == true)
        {
            attackFrame++;
            if (attackFrame < 5)
            {
                if(canMove == true)
                {
                    isMove = true;
                    attackFrame = 0;
                    animator.SetInteger("State", 0);
                    isAttack = false;
                }
  
            }
            else if (attackFrame == 16f)
            {
                audioSource.PlayOneShot(attack,2);
                boxCollider2D.enabled = true;
                capsuleCollider2D.offset = new Vector2(0.3f, -0.1254258f);

            }
            else if (attackFrame == 20)
            {
                capsuleCollider2D.offset = new Vector2(0.5250673f, -0.1254258f);
                boxCollider2D.enabled = false;
            }
            else if (attackFrame == 24)
            {
                attackFrame = -1;
            }
        }
    }

    IEnumerator KillChameleon()
    {
        AudioManager.Instance.PlayEnemyDeathAudioEffect();
        audioSource.enabled = false;
        isAttack = false;
        isMove = false;
        animator.SetInteger("State", 2);
        rb.freezeRotation = false;
        boxCollider2D.enabled = false;
        capsuleCollider2D.enabled = false;
        rb.AddForce(new Vector2(Random.Range(-170f, 170f), Random.Range(250f, 400f)));
        rb.angularVelocity = Random.Range(40f, 160f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (transform.position.x < playerPosition.x)
        {
            if (moveLeft)
            {
                if (transform.position.y + 0.17f < playerPosition.y)
                {
                    StartCoroutine(KillChameleon());
                    return 0;
                }
                return 10;
            }
            else
            {
                if (Mathf.Abs(playerPosition.x - transform.position.x) < 0.34f)
                {
                    if (transform.position.y + 0.17f < playerPosition.y)
                    {
                        StartCoroutine(KillChameleon());
                        return 0;
                    }
                }
                return 10;
            }
        }
        else
        {
            if (moveLeft)
            {
                if (Mathf.Abs(transform.position.x - playerPosition.x) < 0.34f)
                {
                    if (transform.position.y + 0.17f < playerPosition.y)
                    {
                        StartCoroutine(KillChameleon());
                        return 0;
                    }
                }
                return 10;
            }
            else
            {
                if (transform.position.y + 0.17f < playerPosition.y)
                {
                    StartCoroutine(KillChameleon());
                    return 0;
                }
                return 10;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("PlayerEnter");
            audioSource.Stop();
            animator.SetInteger("State", 1);
            isMove = false;
            canMove = false;
            isAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audioSource.Play();
            canMove = true;
        }
    }
}
