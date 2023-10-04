using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.InputSystem;
using UnityEngine.Advertisements;
using Unity.VisualScripting;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static InterstitialAds Instance { get; private set; }
    public bool isPremium = false;
    public string androidAdUnityID;
    public string iosAdUnityID;
    private string adUnityID;
    [SerializeField] private int addsCount = 0;
    System.Action<int> OnAdShowedCallback;
    private int lastIndex = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        #if UNITY_IOS
            adUnityID = iosAdUnityID;
        #elif UNITY_ANDROID
            adUnityID = androidAdUnityID;
        #endif
    }

    public void ShowAdd(System.Action<int> OnAdShowed, int index)
    {
        if(isPremium == false)
        {
            OnAdShowedCallback = OnAdShowed;
            lastIndex = index;
            addsCount++;
            if (addsCount % 4 == 3)
            {
                LoadInterstitialAd();
            }
            else if (addsCount % 4 == 0)
            {
                ShowInterstitialAd();
                return;
            }
        }
        OnAdShowedCallback.Invoke(lastIndex);
        return;
    }
    private void LoadInterstitialAd()
    {
        Advertisement.Load(adUnityID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial_Ad_Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Interstitial_Ad_Failed");
    }

    private void ShowInterstitialAd()
    {
        Advertisement.Show(adUnityID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        OnAdShowedCallback?.Invoke(lastIndex);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Interstitial_Ad_Started");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Interstitial_Ad_Click");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        OnAdShowedCallback?.Invoke(lastIndex);
    }
}
