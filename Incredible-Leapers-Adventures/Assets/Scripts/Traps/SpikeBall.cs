using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private Vector3 rotateDirection = Vector3.forward;
    [SerializeField] private float rotateAngle = 80;
    [SerializeField] private float currentAngle = 0;
    private bool isTrajectoryCircle = false;
    [SerializeField] private bool isClockWise = false;
    private Vector2 throwDirection;
    private AudioSource audioSource;


    private void Awake()
    {
        if(rotateAngle <= -180 || rotateAngle >= 180)
        {
            isTrajectoryCircle = true;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(isTrajectoryCircle == false)
        {
            if(isClockWise == true)
            {
                if(currentAngle < rotateAngle)
                {
                    currentAngle += rotateDirection.z * Time.fixedDeltaTime;
                    transform.eulerAngles += rotateDirection * Time.fixedDeltaTime;
                }
                else
                {
                    isClockWise = false;
                    audioSource.Play();
                }
            }
            else
            {
                if (currentAngle > -rotateAngle)
                {
                    currentAngle -= rotateDirection.z * Time.fixedDeltaTime;
                    transform.eulerAngles -= rotateDirection * Time.fixedDeltaTime;
                }
                else
                {
                    isClockWise = true;
                    audioSource.Play();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.GetDamage(20);
            if(currentAngle > 0)
            {
                if(transform.position.x < collision.transform.position.x)
                {
                    throwDirection = new Vector2(400, 250);
                }
                else
                {
                    throwDirection = new Vector2(-100, 150);
                }
            }
            else
            {
                if (transform.position.x > collision.transform.position.x)
                {
                    throwDirection = new Vector2(-400, 250);
                }
                else
                {
                    throwDirection = new Vector2(100, 150);
                }
            }
            playerController.ThrowCharacter(throwDirection);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.GetDamage(20);
            playerController.ThrowCharacter(throwDirection);
        }
    }
}
