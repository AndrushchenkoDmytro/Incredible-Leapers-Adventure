using System.Collections;
using UnityEngine;

public class Turtle : MonoBehaviour, IDamageCheck
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int state = 0;
    [SerializeField] private float spikesTime = 2.1f;
    [SerializeField] private float turtleTime = 1.4f;
    private float animTransitionTime = 0.4f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool isSpikesOut = true;
    private bool isDie = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(isSpikesOut)
        {
            animator.Play("TurtleSpikeIdle");
        }
        else
        {
            animator.Play("TurtleIdle");
            animator.SetInteger("State", 1);
        }
    }

    private void FixedUpdate()
    {
        if (!isDie)
        {
            if(isSpikesOut)
            {
                if(spikesTime > 0)
                {
                    spikesTime -= Time.fixedDeltaTime;
                }
                else
                {
                    if(animTransitionTime > 0)
                    {
                        if(state == 0)
                        {
                            audioSource.Play();
                            animator.SetInteger("State", 1);
                            state = 1;
                        }
                        animTransitionTime -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        spikesTime = 2.1f;
                        animTransitionTime = 0.1f;
                        isSpikesOut = false;
                    }
                }
            }
            else
            {
                if (turtleTime > 0)
                {
                    turtleTime -= Time.fixedDeltaTime;
                }
                else
                {
                    if (animTransitionTime > 0)
                    {
                        if (state == 1)
                        {
                            audioSource.Play();
                            animator.SetInteger("State", 0);
                            state = 0;
                        }
                        animTransitionTime -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        turtleTime = 1.4f;
                        animTransitionTime = 0.4f;
                        isSpikesOut = true;
                        spriteRenderer.flipX = !spriteRenderer.flipX;
                    }
                }
            }
        }
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        if (isSpikesOut)
        {
            return 20f;
        }
        else
        {
            if(transform.position.y < playerPosition.y)
            {
                StartCoroutine(KillTurtle());
            }
            return 0;
        }
    }

    IEnumerator KillTurtle()
    {
        isDie = true;
        animator.SetInteger("State", 2);
        AudioManager.Instance.PlayEnemyDeathAudioEffect();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = false;
        GetComponent<EdgeCollider2D>().enabled = false;
        rb.AddForce(new Vector2(Random.Range(-150f, 150f), Random.Range(150f, 250f)));
        rb.angularVelocity = Random.Range(20f, 120f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
