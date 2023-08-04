using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string animationClipName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationClipName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.Play("Collect");

            Invoke("DestroyFruit", 1f);
        }

    }

    private void DestroyFruit()
    {
        this.gameObject.SetActive(false);
    }

    
}
