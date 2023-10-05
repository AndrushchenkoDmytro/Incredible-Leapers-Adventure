using UnityEngine;

public class RinoZone : MonoBehaviour
{
    private Rino rino;
    [SerializeField] private AudioClip run;

    private void Awake()
    {
        rino = GetComponentInChildren<Rino>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {         
            rino.hasTarget = true;
            rino.stopPursuit = false;
            rino.audioSource.clip = run;
            rino.stayTime = 0;
            rino.moveTime = 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rino.stopPursuit = true;
        }
    }
}
