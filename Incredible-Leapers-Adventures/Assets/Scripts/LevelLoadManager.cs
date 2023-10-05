using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadManager : MonoBehaviour
{
    public static LevelLoadManager Instance;
    private AsyncOperation scene;
    public int preparationResult = 0;
    [SerializeField] private Image imageProgress;
    [SerializeField] private float loadTarget;
    private int levelIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartPreparation();
    }

    private async void StartPreparation()
    {
        imageProgress = GameObject.Find("UI/LoadingUI/LoadingBar/Progress").GetComponent<Image>();

        imageProgress.fillAmount = 0;
        loadTarget = 0;

        scene = SceneManager.LoadSceneAsync(1);
        scene.allowSceneActivation = false;

        StartCoroutine(UpdateSceneLoadProgressInUI(false));

        GPGSManager.Instance.Authenticate((resualt) =>
        {
            if (resualt == true)
            {
                DataPersistenceManager.Instance.LoadDataFromCloud( () => {
                    preparationResult = 2;
                });
            }
            else
            {
                DataPersistenceManager.Instance.LoadDataFromLocal();
                Instance.preparationResult = 1;
            }
        });

        do
        {
            await Task.Delay(100);
            loadTarget = scene.progress;
        }
        while (scene.progress < 0.9f);

        StartCoroutine(WaitForPreparationResult());
    }
    
    IEnumerator WaitForPreparationResult()
    {
        while (preparationResult < 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 10; i++)
        {
            imageProgress.fillAmount += 0.025f;
            yield return new WaitForSeconds(0.1f);
        }
        scene.allowSceneActivation = true;
        yield break;
    }

    public async void LoadScene(int sceneIndex)
    {
        GameObject UI = GameObject.Find("UI");

        UI.transform.GetChild(1).gameObject.SetActive(true);
        UI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        UI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        imageProgress = UI.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();

        imageProgress.fillAmount = 0;
        loadTarget = 0;
        levelIndex = sceneIndex;

        scene = SceneManager.LoadSceneAsync(levelIndex);
        scene.allowSceneActivation = false;

        StartCoroutine(UpdateSceneLoadProgressInUI(true));
        do
        {
            await Task.Delay(100);
            loadTarget = scene.progress;
        }
        while (scene.progress < 0.9f);
    }

    IEnumerator UpdateSceneLoadProgressInUI(bool activateSceneAfterLoad)
    {

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        yield return new WaitForFixedUpdate();

        while (imageProgress.fillAmount < 0.895f)
        {
            imageProgress.fillAmount = Mathf.MoveTowards(imageProgress.fillAmount, loadTarget, Time.deltaTime * 3.5f);
            yield return new WaitForFixedUpdate();
        }

        if (activateSceneAfterLoad == true)
        {
            for (int i = 0; i < 10; i++)
            {
                imageProgress.fillAmount += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            if (AudioManager.Instance != null)
            {
                if (levelIndex == 1)
                {
                    AudioManager.Instance.PlayMainMenuMusic();
                }
                else if(levelIndex == 37)
                {
                    AudioManager.Instance.PlayBossFightMusic();
                }
                else
                {
                    AudioManager.Instance.PlayRandomMusic();
                }
            }
            imageProgress = null;
            scene.allowSceneActivation = true;
        }
        yield break;
    }

}

