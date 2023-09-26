using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject bgPanel;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mobileInput;
    [SerializeField] private int SceneIndex = 2;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        bgPanel.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        progressBar.SetActive(false);
        pauseButton.SetActive(false);
        mobileInput.SetActive(false);
        bgPanel.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        bgPanel.SetActive(false);
        pauseMenu.SetActive(false);
        progressBar.SetActive(true);
        pauseButton.SetActive(true);
        mobileInput.SetActive(true);
    }

    public void RestartLevel()
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        Time.timeScale = 1;      
        LevelLoadManager.Instance.LoadScene(SceneIndex);
    }

    public void ExitLevel()
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        Time.timeScale = 1;
        LevelLoadManager.Instance.LoadScene(1);
    }
}
