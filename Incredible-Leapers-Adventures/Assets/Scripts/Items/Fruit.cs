using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool isActive = true;
    private Animator animator;
    private FruitZone fruitZone;
    [SerializeField] private string animationClipName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationClipName);
        fruitZone = GetComponentInParent<FruitZone>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(isActive == true)
            {
                animator.Play("Collect");
                fruitZone.PlayFruitCollectSound();
                isActive = false;
                Invoke("DestroyFruit", 1f);
            }
        }

    }

    private void DestroyFruit()
    {
        this.gameObject.SetActive(false);
    }

    
}
