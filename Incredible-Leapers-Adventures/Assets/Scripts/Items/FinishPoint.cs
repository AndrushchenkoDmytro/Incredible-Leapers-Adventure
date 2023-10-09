using System;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator animator;
    public Action OnLevelComplete;

    [SerializeField] private AudioClip victory;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.Play("FinishFlagTaken");
            AudioManager.Instance.PlayAudioEffect(victory, 1);
            Invoke("StopPlayer", 0.2f);
            Invoke("ShowWinPanel",1.5f);
        }
    }

    private void ShowWinPanel()
    {
        OnLevelComplete?.Invoke();        
    }
    private void StopPlayer()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().PlayerWin();
    }

}