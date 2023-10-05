using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioClip flag;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.Play("StartFlagIdle");
            AudioManager.Instance.PlayAudioEffect(flag, 1);
        }
    }
}
