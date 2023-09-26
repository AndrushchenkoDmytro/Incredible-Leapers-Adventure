using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class FlyPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float yDelta = 1f;

    private bool canFly = true;
    private bool goUp = true;


    private Rigidbody2D rb;
    private TrapZone trapZone;
    [SerializeField] private AudioClip fall;
    [SerializeField] private AudioClip bam;

    private void Awake()
    {
        startPosition = transform.position;
        endPosition = startPosition;
        endPosition.y += yDelta;
        rb = GetComponent<Rigidbody2D>();
        trapZone = GetComponentInParent<TrapZone>();
    }

    private void FixedUpdate()
    {
        if(canFly == true)
        {
            if (goUp == true)
            {
                if (transform.position.y < endPosition.y)
                {
                    rb.velocity = Vector2.up;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    goUp = false;
                }
            }
            else
            {
                if (transform.position.y > startPosition.y)
                {
                    rb.velocity = Vector2.down;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    goUp = true;
                }
            }

        }
    }

    private void DestroyPlatform()
    {
        trapZone.RemoveChild();
        Destroy(gameObject);
    }
    private void FallPlatform()
    {
        AudioManager.Instance.PlayAudioEffect(fall,0.7f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 3f;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.angularVelocity = Random.Range(60f, 160f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (canFly)
            {
                canFly = false;
                AudioManager.Instance.PlayAudioEffect(bam,0.75f);
                Invoke("FallPlatform", 0.55f);
                Invoke("DestroyPlatform", 2f);
            }
        }
    }

}
