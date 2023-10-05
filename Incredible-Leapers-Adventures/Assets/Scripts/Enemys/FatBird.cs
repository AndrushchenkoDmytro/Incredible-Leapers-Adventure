using System.Collections;
using UnityEngine;

public class FatBird : MonoBehaviour, IDamageCheck
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float topPoint;
    [SerializeField] private Vector2 flyForce;
    private float flyForceY = 0;
    private float flyAnimLenth = 0.25f;
    private float flyTime = 1.5f;
    private bool isFly = true;
    private bool isHang = true;
    private bool isFall = false;
    private bool isDie = false;
    private float groundTime = 0.8f;
    private ParticleSystem particleSystem;
    private AudioSource audioSource;
    [SerializeField] private AudioClip falling;
    [SerializeField] private AudioClip ground;
    [SerializeField] private AudioClip flying;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flyForceY = flyForce.y;
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(!isDie)
        {
            if (isFly)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
                if (transform.localPosition.y < topPoint)
                {
                    if (flyAnimLenth > 0)
                    {
                        flyAnimLenth -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        flyAnimLenth = 0.5f;
                        rb.AddForce(flyForce);
                        flyForce.y -= 2.5f;
                    }
                }
                else
                {
                    if (flyTime > 0)
                    {
                        flyTime -= Time.fixedDeltaTime;
                        if (isHang)
                        {
                            rb.gravityScale = 0;
                            rb.velocity *= 0.1f;
                            isHang = false;
                        }
                    }
                    else
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(falling);
                        isFly = false;
                        isFall = true;
                        flyTime = 1.5f;
                        rb.gravityScale = 0.5f;
                        animator.SetInteger("State", 1);
                    }
                }
            }
            else
            {
                if (isFall)
                {
                    rb.gravityScale += 0.025f;
                }
                else
                {
                    if (groundTime > 0)
                    {
                        groundTime -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        audioSource.Play();
                        groundTime = 0.8f;
                        rb.gravityScale = 0.5f;
                        flyForce.y = flyForceY;
                        isFly = true;
                        isHang = true;
                        isFall = false;
                        animator.SetInteger("State", 0);
                    }
                }

            }
        }
    }

    IEnumerator KillBird()
    {
        audioSource.enabled = false;
        AudioManager.Instance.PlayEnemyDeathAudioEffect();
        isDie = true;
        animator.SetInteger("State", 3);
        GetComponent<CompositeCollider2D>().isTrigger = true;
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(Random.Range(-50, 50), Random.Range(0, 100f)));
        rb.angularVelocity = Random.Range(-90f, 90f);
        yield return new WaitForSeconds(0.25f);
        rb.gravityScale = 3f;
        yield return new WaitForSeconds(1.75f);
        Destroy(gameObject);
    }


    public float CheckForDamage(Vector3 playerPosition)
    {
        if (playerPosition.y > transform.position.y + 0.45f)
        {
            StartCoroutine(KillBird());
            return 0;
        }
        else
        {
            isFall = false;
            animator.SetInteger("State", 2);
            return 15f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "TileMap")
        {
            audioSource.PlayOneShot(ground);
            Debug.Log("TIleMap");
            isFall = false;
            animator.SetInteger("State",2);
            particleSystem.Play();
        }
    }

}
