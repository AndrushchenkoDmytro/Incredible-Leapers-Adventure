using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Advertisements;
public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    public string iosGameId;
    public bool isTestingMode = true;
    string gameId;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        Initialize();
    }

    private void Initialize()
    {

    #if UNITY_IOS
        gameId = iosGameId;
    #elif UNITY_ANDROID
        gameId = androidGameId;
    #elif UNITY_EDITOR
        gameId = androidGameId;
    #endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);
        }
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Ads is Working");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ads NotWorking");
    }
}
