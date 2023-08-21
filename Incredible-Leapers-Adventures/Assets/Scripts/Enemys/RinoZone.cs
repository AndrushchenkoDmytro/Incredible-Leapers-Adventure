using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoZone : MonoBehaviour
{
    private Rino rino;

    private void Awake()
    {
        rino = GetComponentInChildren<Rino>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            rino.hasTarget = true;
            rino.stayTime = 0;
            rino.moveTime = 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rino.hasTarget = false;
        }
    }
}
