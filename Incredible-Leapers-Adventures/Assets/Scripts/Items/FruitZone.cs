using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitZone : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip collect;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFruitCollectSound()
    {
        audioSource.PlayOneShot(collect,1.5f);
    }
}
