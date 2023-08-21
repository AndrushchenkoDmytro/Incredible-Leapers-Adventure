using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject bgPanel;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        bgPanel.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        progressBar.SetActive(false);
        pauseButton.SetActive(false);
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
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitLevel()
    {
        Debug.Log("Exit");
    }

}
