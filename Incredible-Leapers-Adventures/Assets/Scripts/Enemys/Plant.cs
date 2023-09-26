using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour, IDamageCheck
{
    [SerializeField] private GameObject peaBullet;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float reloadTime;
    private float currentTime = 0;
    private bool isAttack = false;
    private bool readyToAttack = false;
    private Transform target;
    private Rigidbody2D capsuleRB;
    private Rigidbody2D boxRB;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        currentTime = reloadTime * 0.75f;
        capsuleRB = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxRB = transform.GetChild(0).GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetChild(0).GetComponent<BoxCollider2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(isAttack == true)
        {
            if(transform.position.x < target.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if(currentTime > reloadTime - 0.3f)
            {
                if (readyToAttack == false)
                {
                    readyToAttack = true;
                    animator.SetBool("readyToAttack", true);
                }
            }

            if(currentTime > reloadTime)
            {
                Instantiate(peaBullet, spawnPos.position, transform.rotation);
                audioSource.Play();
                currentTime = 0;
                readyToAttack = false;
                animator.SetBool("readyToAttack", false);
            }
            else
            {
                currentTime += Time.fixedDeltaTime;
            }
        }
    }

    public float CheckForDamage(Vector3 playerPosition)
    {
        StartCoroutine(KillPlant());
        return 0;
    }

    IEnumerator KillPlant()
    {
        isAttack = false;
        audioSource.Stop();
        AudioManager.Instance.PlayEnemyDeathAudioEffect();
        capsuleCollider.enabled = false;
        boxCollider2D.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        boxRB.simulated = false;
        capsuleRB.bodyType = RigidbodyType2D.Dynamic;
        capsuleRB.AddForce(new Vector2(Random.Range(-75, 75), 300f));
        animator.SetBool("isHit", true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.transform;
            isAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAttack = false;
        }
    }
}
