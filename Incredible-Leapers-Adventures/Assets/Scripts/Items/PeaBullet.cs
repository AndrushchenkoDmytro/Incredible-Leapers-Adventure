using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(10);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "TileMap")
        {
            Destroy(gameObject);
        }
    }
}
