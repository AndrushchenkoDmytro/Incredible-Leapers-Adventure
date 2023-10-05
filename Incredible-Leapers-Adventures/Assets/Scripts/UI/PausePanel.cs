using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject bgPanel;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mobileInput;
    [SerializeField] private int sceneIndex = 2;

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
        InterstitialAds.Instance.ShowAdd(OnAdWatched,sceneIndex);
    }

    public void ExitLevel()
    {
        InterstitialAds.Instance.ShowAdd(OnAdWatched, 1);
    }

    public void OnAdWatched(int index)
    {
        Time.timeScale = 1;
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LevelLoadManager.Instance.LoadScene(index);
    }
}
