using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Vector2 throwDirection;
    [SerializeField] AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.GetDamage(20);
            playerController.ThrowCharacter(throwDirection);
        }
    }
}
