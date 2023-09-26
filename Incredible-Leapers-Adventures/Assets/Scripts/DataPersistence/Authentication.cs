using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Authentication : MonoBehaviour
{
    /*public static Authentication Instance { get; private set; }

    public bool isAuthentication { get; private set; }

    public PlayGamesPlatform platform { get; private set; }
    public string gpgToken = "";
    public string error = "";

    public TextMeshProUGUI text;

    private async void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
            platform = null;
            Debug.Log("HEEEEEEELLLLLO");
            await UnityServices.InitializeAsync();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        await Authenticate();
        DataPersistenceManager.Instance.LoadGame();
    }

    public async Task Authenticate()
    {
        if(platform == null)
        {
            platform = PlayGamesPlatform.Activate();
        }
        if(AuthenticationService.Instance == null)
        {
            await UnityServices.InitializeAsync();
        }

        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google was successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log($"Auth code is {code}");
                    gpgToken = code;
                });
            }
            else
            {
                error = "Failed to retrieve GPG auth code";
                Debug.Log("Login Unsuccessful");
            }
        });
        await AuthenticateWithUnity();

        DataPersistenceManager.Instance.LoadGame();
    }

    /*private PlayGamesPlatform BuildPlatform()
    {
        var builder = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .RequestIdToken()
        .RequestServerAuthCode(false);
        PlayGamesPlatform.InitializeInstance(builder.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        return PlayGamesPlatform.Activate();
    }*/

    /*
    private async Task AuthenticateWithUnity()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(gpgToken);
        }
        catch (AuthenticationException ex)
        {
            Debug.Log(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.Log(ex);
        }
    }
    */
}
