using System;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip doorOpening;
    public Action OnDoorButtonPressed;
    [SerializeField] private ButtonColor buttonColor;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(animator != null)
            {
                animator.Play(((int)buttonColor).ToString());
                Invoke("OpenDoor", 0.5f);
                GetComponent<BoxCollider2D>().enabled = false;
                AudioManager.Instance.PlayAudioEffect(buttonClick, 0.75f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (animator == null)
            {
                OpenDoor();
                GetComponent<BoxCollider2D>().enabled = false;
                AudioManager.Instance.PlayAudioEffect(buttonClick, 0.75f);
            }
        }
    }
    private void OpenDoor()
    {
        OnDoorButtonPressed?.Invoke();
        AudioManager.Instance.PlayAudioEffect(doorOpening, 0.75f);
    }
}

public enum ButtonColor
{
    blue,
    green
}
