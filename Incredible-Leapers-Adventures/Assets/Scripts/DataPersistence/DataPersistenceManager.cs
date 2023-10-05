using UnityEngine;
using System;
using System.Threading.Tasks;


public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private const string saveName = "saveName.save";
    private IDataFileHandler cloudDataFileHandler = new GPGS_CloudDataFileHandler();
    private IDataFileHandler localDataFileHandler = new Local_DataFileHandler();

    private GameData localData = new GameData();
    private GameData cloudData = new GameData();
    private GameData gameData = new GameData();

    public Action OnGameDataReload;

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
    public async void LoadDataFromCloud(Action callback)
    {
        if (GPGSManager.Instance.CheckAuthentication() == true)
        {
            cloudData = await LoadCloudDataAsync(); 
        }
        LoadLocalData();
        if (cloudData != null)
        {
            if (cloudData.passedLevelsCount > localData.passedLevelsCount)
            {
                localData.passedLevelsCount = cloudData.passedLevelsCount;
            }
        }
        else
        {

        }
        OnGameDataReload?.Invoke();
        callback?.Invoke();
    }

    public void LoadDataFromLocal()
    {
        LoadLocalData();
    }

    private async Task<GameData> LoadCloudDataAsync()
    {
        GameData gameData = null;
        var tcs = new TaskCompletionSource<GameData>();
        cloudDataFileHandler.LoadData(saveName, (loadedString) =>
        {
            if (loadedString != null && loadedString != string.Empty)
            {
                gameData = JsonUtility.FromJson<GameData>(loadedString);
                Debug.Log(" FromJson<GameData>(loadedString) = " + gameData.passedLevelsCount.ToString());
            }
            else
            {
                Debug.Log(" cloudDataFileHandler.loadedString = null ");
            }
            tcs.SetResult(gameData);
        });
        return await tcs.Task;
    }

    private void LoadLocalData()
    {
        localDataFileHandler.LoadData(saveName, (loadedString) =>
        {
            if (loadedString == string.Empty || loadedString == null)
            {
                StartNewGame();
            }
            else
            {
                gameData = JsonUtility.FromJson<GameData>(loadedString);
            }
            if(gameData != null)
            {
                localData.passedGameCount = gameData.passedGameCount;
                localData.passedLevelsCount = gameData.passedLevelsCount;
            }
        });
    }

    public void StartNewGame()
    {
        if(localData == null)
        {
            localData = new GameData();
        }
        localData.passedLevelsCount = 1;
        SaveGame();
        OnGameDataReload?.Invoke();
    }

    public void SaveGame()
    {
        string saveData = JsonUtility.ToJson(localData);
        Debug.Log(saveData);

        localDataFileHandler.SaveData(saveName, saveData);

        if (GPGSManager.Instance.CheckAuthentication() == true)
        {
            cloudDataFileHandler.SaveData(saveName, saveData);
        }
        else
        {
            GPGSManager.Instance.Authenticate(result =>
            {
                if(result == true)
                {
                    cloudDataFileHandler.SaveData(saveName, saveData);
                }
                else
                {
                    Debug.Log("cloudSaveFailed");  
                }
            });
        }
        Debug.Log(localData.passedLevelsCount);
    }

    public int GetPassedLevelsCount()
    {
        return localData.passedLevelsCount;
    }
    public void AddPassedLevelsCount()
    {
        localData.passedLevelsCount++;
    }
    public int GetPassedGameCount()
    {
        return localData.passedGameCount;
    }
    public void AddPassedGameCount()
    {
        localData.passedLevelsCount++;
    }
}
