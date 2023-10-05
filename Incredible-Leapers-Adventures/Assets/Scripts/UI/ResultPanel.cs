using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 2;
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
        if (sceneIndex > DataPersistenceManager.Instance.GetPassedLevelsCount())
        {
            if(sceneIndex == 13)
            {
                GPGSManager.Instance.AchievementCompleted("CgkI2463hYUOEAIQAg");
            }
            else if(sceneIndex == 25)
            {
                GPGSManager.Instance.AchievementCompleted("CgkI2463hYUOEAIQAw");
            }
            else if(sceneIndex == 37)
            {
                GPGSManager.Instance.AchievementCompleted("CgkI2463hYUOEAIQBA");
                if(DataPersistenceManager.Instance.GetPassedGameCount() == 1)
                {
                    GPGSManager.Instance.AchievementCompleted("CgkI2463hYUOEAIQBg");
                }
                DataPersistenceManager.Instance.AddPassedGameCount();
            }
 
            DataPersistenceManager.Instance.AddPassedLevelsCount();
            DataPersistenceManager.Instance.SaveGame();
        }
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
        InterstitialAds.Instance.ShowAdd(OnAdWatched, sceneIndex + 1);
    }

    public void Restart()
    {
        InterstitialAds.Instance.ShowAdd(OnAdWatched, sceneIndex);
    }

    public void ToMainMenu()
    {
        InterstitialAds.Instance.ShowAdd(OnAdWatched,1);
    }

    public void OnAdWatched(int index)
    {
        bgPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        LevelLoadManager.Instance.LoadScene(index);
    }
}
