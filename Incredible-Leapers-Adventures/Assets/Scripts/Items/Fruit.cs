using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool isActive = true;
    private Animator animator;
    private FruitCounter fruitCounter;
    [SerializeField] private string animationClipName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationClipName);
        fruitCounter = GetComponentInParent<FruitCounter>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(isActive == true)
            {
                animator.Play("Collect");
                fruitCounter.OnFruitCollect();
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
