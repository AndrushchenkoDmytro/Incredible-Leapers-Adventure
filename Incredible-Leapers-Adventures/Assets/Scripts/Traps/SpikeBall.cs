using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minRotationAngle = -90;
    [SerializeField] private float maxRotationAngle = 90;
    private float generalRotationAngle = 180;
    [SerializeField] private bool fullRotation = false;
    private float time = 0;
    [SerializeField] private float throwForce = 500;
    [SerializeField] private AudioClip clip;
    private void Awake()
    {
        generalRotationAngle = Mathf.Abs(minRotationAngle) + Mathf.Abs(maxRotationAngle);
        if(generalRotationAngle < 360)
        {
            fullRotation = false;
        }
        else
        {
            fullRotation = true;
            if (maxRotationAngle <= minRotationAngle) rotationSpeed *= -1;
        }
    }

    private void FixedUpdate()
    {
        if (fullRotation)
        {
            float rotationAngle = Mathf.Repeat(Time.time * rotationSpeed, 360);
            transform.localEulerAngles = new Vector3(0, 0, rotationAngle);
        }
        else
        {
            time += Time.deltaTime;

            // Отримуємо відповідне значення кута з квадратичної функції для керування швидкістю
            float t = Mathf.PingPong(time * rotationSpeed, 1.0f);
            float slowedT = Mathf.SmoothStep(0.0f, 1.0f, t);

            // Використовуємо лінійну інтерполяцію між мінімальним і максимальним кутами обертання
            float rotationAngle = Mathf.Lerp(minRotationAngle, maxRotationAngle, slowedT);

            // Оновлюємо локальні ейлерові кути об'єкта
            transform.localEulerAngles = new Vector3(0, 0, rotationAngle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            Vector2 knockbackDirection = collision.GetContact(0).point - new Vector2(transform.position.x, transform.position.y);
            knockbackDirection.y *= -1;
            AudioManager.Instance.PlayAudioEffect(clip,1.25f);
            playerController.GetDamage(20);
            playerController.ThrowCharacter(knockbackDirection.normalized * throwForce);
        }
    }
}
