using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fan : MonoBehaviour
{
    private Animator animator;
    private ParticleSystem fanParticles;
    private ParticleSystem.MainModule fanParticlesMain;
    private BoxCollider2D fanCollider;
    private bool isActive = true;
    private float workTime = 3;
    private float sleepTime = 2;
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
            collision.gameObject.GetComponent<PlayerController>().isFly = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().isFly = false;
        }
    }

}
