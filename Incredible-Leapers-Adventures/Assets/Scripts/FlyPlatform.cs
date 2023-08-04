using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class FlyPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float yDelta = 0.5f;
    private float fallTime = 0;
    private float rotationDirection = 0;

    private bool canFly = true;
    private bool goUp = true;


    private Rigidbody2D rb;

    private void Awake()
    {
        startPosition = transform.position;
        endPosition = startPosition;
        endPosition.y += yDelta;
        rb = GetComponent<Rigidbody2D>();
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
        else
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, fallTime);
            float scale = Mathf.Lerp(1f, 0.25f, fallTime);
            transform.localScale = new Vector3(scale,scale,scale);
            rb.MoveRotation(rb.rotation + 5 );
            fallTime += Time.fixedDeltaTime;
        }
    }

    private void DestroyPlatform()
    {
        Destroy(gameObject);
    }
    private void FallPlatform()
    {
        yDelta = 3;
        transform.position += new Vector3(0, 0, 0.25f);
        startPosition = transform.position;
        endPosition.y -= yDelta;
        endPosition.z = 1.75f;
        rotationDirection = Random.Range(0, 2);
        if (rotationDirection == 0)
        {
            rotationDirection = Random.Range(5f, 7f);
        }
        else
        {
            rotationDirection = -Random.Range(5f, 7f);
        }
        canFly = false;
        rb.freezeRotation = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Invoke("FallPlatform", 0.5f);
            Invoke("DestroyPlatform", 2f);
        }
    }

}
