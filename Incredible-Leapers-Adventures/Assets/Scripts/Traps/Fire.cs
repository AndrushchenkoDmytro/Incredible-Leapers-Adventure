using UnityEngine;

public class Fire : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D fireCollider;
    private AudioSource audioSource;
    [SerializeField] private bool isActive = true;
    [SerializeField] private float workTime = 3;
    [SerializeField] private float sleepTime = 2;
    private float currentTime = 0;
    [SerializeField] private AudioClip fire;
 
    private void Awake()
    {
        animator = GetComponent<Animator>();
        fireCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponentInParent<AudioSource>();
        if(isActive == false)
        {
            animator.SetBool("isActive", false);
        }

    }

    private void FixedUpdate()
    {
        if (isActive == true)
        {
            if (currentTime > workTime)
            {
                fireCollider.enabled = false;
                isActive = false;
                animator.SetBool("isActive", isActive);
                currentTime = 0;
                audioSource.Stop();
            }
            else
            {
                currentTime += Time.fixedDeltaTime;
            }
        }
        else
        {
            if (currentTime > sleepTime)
            {
                fireCollider.enabled = true;
                isActive = true;
                animator.SetBool("isActive", isActive);
                currentTime = 0;
                audioSource.PlayOneShot(fire);
            }
            else
            {
                currentTime += Time.fixedDeltaTime;
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
}
