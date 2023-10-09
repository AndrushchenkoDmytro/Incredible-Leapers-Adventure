using UnityEngine;

public class PurchasesInfo : MonoBehaviour
{
    private void Awake()
    {
        RemoveAdsButton();
    }
    private void Start()
    {
        RemoveAdsButton();
    }

    public void RemoveAdsButton()
    {
        if(PlayerPrefs.GetInt("removeAds") == 1)
        {
            InterstitialAds.Instance.isPremium = true;
            Destroy(gameObject);
        }
    }
}

