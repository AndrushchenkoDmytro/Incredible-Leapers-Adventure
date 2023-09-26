using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    private int childCount;
    private AudioSource audioSource;

    private void Awake()
    {
        childCount = transform.childCount;
        audioSource = GetComponent<AudioSource>();
    }

    public void RemoveChild()
    {
        childCount--;
        if (childCount == 0)
        {
            audioSource.Stop();
            audioSource.enabled = false;
            Destroy(gameObject);
        }
    }


}
