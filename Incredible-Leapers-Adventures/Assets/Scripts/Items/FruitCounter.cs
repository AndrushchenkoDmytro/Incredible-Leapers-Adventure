using TMPro;
using UnityEngine;

public class FruitCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitsCount;
    [SerializeField] ResultPanel resultPanel;
    [SerializeField] private AudioClip collect;
    [SerializeField] private int fruitCount;
    [SerializeField] private AudioClip victory;

    private void Awake()
    {
        fruitsCount.text = string.Concat("x", fruitCount);
    }
    public void OnFruitCollect()
    {
        fruitCount--;
        if ( fruitCount < 1)
        {
            fruitsCount.text = string.Empty;
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerController>().enabled = false;
            player.tag = "Untagged";
            AudioManager.Instance.PlayAudioEffect(victory, 1);
            fruitsCount.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
            resultPanel.ShowWinPanel();
        }
        else
        {
            fruitsCount.text = string.Concat("x", fruitCount);
        }
        AudioManager.Instance.PlayAudioEffect(collect, 1.5f);
    }
}
