using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private int levelIndex = 2;
    [SerializeField] private GameObject bgPanel;
    [SerializeField] private FinishPoint finishPoint;
    [SerializeField] private GameObject mobileInput;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject pauseButon;

    private void Awake()
    {
        finishPoint.OnLevelComplete += ShowWinPanel;
        GameObject.Find("Player").GetComponent<PlayerController>().OnPlayerDie += ShowLosePanel;
    }

    private void OnDestroy()
    {
        finishPoint.OnLevelComplete -= ShowWinPanel;
    }

    public void ShowWinPanel()
    {
        mobileInput.SetActive(false);
        healthBar.SetActive(false);
        pauseButon.SetActive(false);
        bgPanel.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowLosePanel()
    {
        mobileInput.SetActive(false);
        healthBar.SetActive(false);
        pauseButon.SetActive(false);
        bgPanel.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LevelLoadManager.Instance.LoadScene(levelIndex+1);
    }

    public void Restart()
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LevelLoadManager.Instance.LoadScene(levelIndex);
    }

    public void ToMainMenu()
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LevelLoadManager.Instance.LoadScene(1);
    }
}
