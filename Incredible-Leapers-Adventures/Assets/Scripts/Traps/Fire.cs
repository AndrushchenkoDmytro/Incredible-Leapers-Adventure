using UnityEngine;

public class Fire : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D fireCollider;
    private bool isActive = true;
    private float workTime = 3;
    private float sleepTime = 2;
    private float currentTime = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fireCollider = GetComponent<BoxCollider2D>();
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
