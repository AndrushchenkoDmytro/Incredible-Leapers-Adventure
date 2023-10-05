using UnityEngine;

public class Fan : MonoBehaviour
{
    private Animator animator;
    private ParticleSystem fanParticles;
    private ParticleSystem.MainModule fanParticlesMain;
    private BoxCollider2D fanCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool isActive = true;
    [SerializeField] private float workTime = 4;
    [SerializeField] private float sleepTime = 2;
    [SerializeField] private bool blowUp = true;
    private float currentTime = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fanParticles = GetComponentInChildren<ParticleSystem>();
        fanParticlesMain = fanParticles.main;
        fanCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if(isActive == true)
        {
            if(currentTime > workTime) 
            {
                fanCollider.enabled = false;
                fanParticlesMain.loop = false;
                isActive = false;
                animator.SetBool("isActive", isActive);
                currentTime = 0;
                if (audioSource.isPlaying)
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
                fanCollider.enabled = true;
                fanParticlesMain.loop = true;
                fanParticles.Play();
                isActive = true;
                animator.SetBool("isActive", isActive);
                currentTime = 0;
                if(!audioSource.isPlaying)
                audioSource.Play();
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
            collision.gameObject.GetComponent<PlayerController>().SetFly(true,blowUp);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().SetFly(false, blowUp);
        }
    }

}
