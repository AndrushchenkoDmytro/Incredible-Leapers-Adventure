using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GooglePlayGames.BasicApi;
using System;
using TMPro;
using System.Threading.Tasks;
using Unity.Services.CloudSave;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private const string saveName = "saveName.save";
    private IDataFileHandler cloudDataFileHandler = new GPGS_CloudDataFileHandler();
    private IDataFileHandler localDataFileHandler = new Local_DataFileHandler();

    public GameData localData = new GameData();
    private GameData cloudData = new GameData();

    //private List<ILoadDataObject> loadDataObjects = new List<ILoadDataObject>();
    private List<ISaveDataObject> saveDataObjects = new List<ISaveDataObject>();

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
            Debug.Log("isAuthenticated == true -> LoadCloudGame()");
            cloudData = await LoadCloudDataAsync(); // «м≥нено на асинхронний виклик
        }
        LoadLocalData();

        if(cloudData != null)
        {
            if (cloudData.passedLevelsCount > localData.passedLevelsCount)
            {
                Debug.Log("cloudData.passedLevelsCount > gameData.passedLevelsCount");
                localData.passedLevelsCount = cloudData.passedLevelsCount;
            }
        }
        else
        {
            Debug.Log("cloudData = null");
        }
        Debug.Log(" cloudData - " + cloudData.passedLevelsCount + " | local - " + localData.passedLevelsCount);
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
            localData = JsonUtility.FromJson<GameData>(loadedString);
        });

        if(localData == null)
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        localData = new GameData();
        OnGameDataReload?.Invoke();
    }

    public void SaveGame()
    {
        foreach (ISaveDataObject dataSaveObjcet in saveDataObjects)
        {
            dataSaveObjcet.SaveData(ref localData);
        }
        localData.passedLevelsCount += 2;
        string saveData = JsonUtility.ToJson(localData);

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
    }

    /*public void AddILoadDataPersistence(ILoadDataObject loadObject)
    {
        if (!loadDataObjects.Contains(loadObject))
            loadDataObjects.Add(loadObject);
    }

    public void RemoveILoadDataPersistence(ILoadDataObject loadObject)
    {
        if (loadDataObjects.Contains(loadObject))
            loadDataObjects.Remove(loadObject);
    }*/

    public void AddISaveDataPersistence(ISaveDataObject saveObject)
    {
        if (!saveDataObjects.Contains(saveObject))
            saveDataObjects.Add(saveObject);
    }

    public void RemoveISaveDataPersistence(ISaveDataObject saveObject)
    {
        if (saveDataObjects.Contains(saveObject))
            saveDataObjects.Remove(saveObject);
    }

    private void OnApplicationQuit()
    {

    }
}
