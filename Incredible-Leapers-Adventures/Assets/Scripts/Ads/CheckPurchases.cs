using UnityEngine.Purchasing;
using UnityEngine;

public class CheckPurchases : MonoBehaviour
{
    [SerializeField] PurchasesInfo pinfo;
    public void OnPurchasesCompleted(Product product)
    {
        if (product.definition.id == "com.torionyx_games.ila.removeads")
        {
            RemoveAds();
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("removeAds", 1);
        InterstitialAds.Instance.isPremium = true;
        if (pinfo!= null)
        {
            pinfo.RemoveAdsButton();
        }
    }
}
