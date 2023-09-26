using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance;
    public PlayGamesPlatform platform;

    Action<bool> OnAuthenticationCompleted;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Authenticate(Action<bool> authenticateCallback)
    {
        OnAuthenticationCompleted = authenticateCallback;
        Initialize();
        if(platform == null)
        {
            OnAuthenticationCompleted?.Invoke(false);
        }
        SignInUserWithPlayGames();
    }
    public void Initialize()
    {
        if (platform == null)
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }
    }
    internal void SignInUserWithPlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInCallback);
    }
    internal void SignInCallback(SignInStatus success)
    {
        if (success == SignInStatus.Success)
        {
            OnAuthenticationCompleted?.Invoke(true);
        }
        else if (success == SignInStatus.InternalError)
        {
            OnAuthenticationCompleted?.Invoke(false);
        }
        else
        {
            OnAuthenticationCompleted?.Invoke(false);
        }
    }

    public bool CheckAuthentication()
    {
        return Social.localUser.authenticated;
    }
    public void AchievementCompleted(string achievementID)
    {
        Social.ReportProgress(achievementID, 100.0f, (bool success) => 
        {
            if (success)
            {
                Debug.Log("AchievementCompleted");
            }   
            else
            {
                Debug.Log("failedGetAchievement");
            }
        });
    }

    public void ShowAchivmentUI()
    {
        Social.ShowAchievementsUI();
    }
}
 
