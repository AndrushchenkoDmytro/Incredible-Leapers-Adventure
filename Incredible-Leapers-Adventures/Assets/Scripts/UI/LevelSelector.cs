using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int SceneIndex;
    [SerializeField] private Sprite lockSprite;

    public void LoadLevel()
    {
        AudioManager.Instance.PlayLevelSelectEffect();
        LevelLoadManager.Instance.LoadScene(SceneIndex);
    }

    public void SetLevelAccess(int passedLevelsCount)
    {
        if (passedLevelsCount + 1 < SceneIndex)
        {
            GetComponent<Image>().sprite = lockSprite;
            GetComponent<Button>().enabled = false;
        }
    }

}
