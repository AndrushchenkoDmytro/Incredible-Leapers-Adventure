using System.Collections;
using UnityEngine;

public class MapPagesController : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private int pageIndex = 0;
    [SerializeField] private int passedLevelsCount = 0;
    [SerializeField] private int enablePagesCount = 0;
    [SerializeField] private AudioClip swap;
    [SerializeField] private AudioClip error;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ReLoadData();
        DataPersistenceManager.Instance.OnGameDataReload += ReLoadData;
    }

    private void OnDestroy()
    {
        DataPersistenceManager.Instance.OnGameDataReload -= ReLoadData;
    }

    public void ReLoadData()
    {
        StartCoroutine(SetPagesData());
    }

    public void NextLevelPage()
    {
        if (enablePagesCount > pageIndex)
        {
            if (pageIndex < transform.childCount - 1)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x - 1280, 0);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMax.x, 0);
                pageIndex++;
                AudioManager.Instance.PlayAudioEffect(swap, 1.25f);
                CheckButtonsState();
            }
        }
        else
        {
            AudioManager.Instance.PlayAudioEffect(error, 1.25f);
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x + 1280, 0);
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMax.x, 0);
            pageIndex--;
            AudioManager.Instance.PlayAudioEffect(swap, 1.25f);
            CheckButtonsState();
        }
    }

    private void StartPageSelect()
    {
        pageIndex = 0;
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.offsetMin = new Vector2(0, 0);
        for (int i = 0; i < transform.childCount-1; i++)
        {
            if (passedLevelsCount > 12 + 12 * i)
            {
                enablePagesCount++;
                NextLevelPage();
            }
            else
            {
                CheckButtonsState();
                break;
            }
        }
    }

    private void CheckButtonsState()
    {
        if (pageIndex == 0)
        {
            prevPageButton.SetActive(false);
        }
        else if (pageIndex == transform.childCount - 1)
        {
            nextPageButton.SetActive(false);
        }
        else
        {
            prevPageButton.SetActive(true);
            nextPageButton.SetActive(true);
        }
    }

    IEnumerator SetPagesData()
    {
        passedLevelsCount = DataPersistenceManager.Instance.GetPassedLevelsCount();
        Transform level;
        for (int i = 0; i < transform.childCount; i++)
        {
            level = transform.GetChild(i);
            for (int j = 0; j < level.childCount; j++)
            {
                level.GetChild(j).GetComponent<LevelSelector>().SetLevelAccess(passedLevelsCount);
            }
            yield return new WaitForFixedUpdate();
        }
        StartPageSelect();
        yield return new WaitForFixedUpdate();
        yield break;
    }

}
