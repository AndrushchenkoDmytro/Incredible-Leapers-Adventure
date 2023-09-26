using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;

    [SerializeField] private AudioClip buttonClick;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("audioSettings", 0.5f);
    }

    public void audioSettings()
    {
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            effectSlider.value = AudioManager.Instance.GetEffectVolume();
        }
        else
        {
            musicSlider.value = 0.5f;
            effectSlider.value = 0.5f;
        }
    }

    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void ToMap()
    {
        //DataPersistenceManager.Instance.SaveGame();
        AudioManager.Instance.PlayAudioEffect(buttonClick,1);
        animator.Play("ToMap");
    }
    public void NewGame()
    {
        AudioManager.Instance.PlayAudioEffect(buttonClick, 1);
        DataPersistenceManager.Instance.StartNewGame();
    }

    public void ToMainMenu()
    {
        AudioManager.Instance.PlayAudioEffect(buttonClick, 1);
        animator.Play("ToMainMenu");
    }

    public void ShowAchievementsBoard()
    {
        GPGSManager.Instance.AchievementCompleted("CgkI2463hYUOEAIQBQ");
        GPGSManager.Instance.ShowAchivmentUI();
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlayAudioEffect(buttonClick, 1);
        animator.Play("OpenSettings");
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void SetEffectVolume()
    {
        AudioManager.Instance.SetEffectVolume(effectSlider.value);
    }

    public void CloseSettings()
    {
        AudioManager.Instance.PlayAudioEffect(buttonClick, 1);
        animator.Play("CloseSettings");
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayAudioEffect(buttonClick, 1);
        Application.Quit();
    }
}