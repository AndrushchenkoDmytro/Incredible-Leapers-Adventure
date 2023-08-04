using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().ThrowCharacterUp(Vector2.up * 900);
            Invoke("PlatformPush", 0.02f);
        }
    }

    private void PlatformPush()
    {
        animator.Play("JumpPlatformOn");
    }
}
