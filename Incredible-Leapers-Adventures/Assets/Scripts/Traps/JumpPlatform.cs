using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private Vector2 throwForce = Vector2.up * 950;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSource.Play();
            collision.gameObject.GetComponent<PlayerController>().ThrowCharacter(throwForce);
            Invoke("PlatformPush", 0.04f);
        }
    }

    private void PlatformPush()
    {
        animator.Play("JumpPlatformOn");
    }
}
