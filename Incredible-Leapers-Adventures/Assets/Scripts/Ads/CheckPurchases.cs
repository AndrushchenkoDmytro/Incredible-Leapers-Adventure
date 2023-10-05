using UnityEngine.Purchasing;
using UnityEngine;

public class CheckPurchases : MonoBehaviour
{
    [SerializeField] PurchasesInfo pinfo;
    public void OnPurchasesCompleted(Product product)
    {
        if (product.definition.id == "com.TorioNyx_Games.IncredibleLeapersAdventures.removeAds")
        {
            RemoveAds();
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("removeAds", 1);
        if (pinfo!= null)
        {
            pinfo.RemoveAdsButton();
        }
    }
}
