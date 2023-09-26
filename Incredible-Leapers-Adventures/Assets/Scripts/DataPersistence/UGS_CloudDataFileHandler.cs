using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Services.CloudSave;
using UnityEngine;

public class UGS_CloudDataFileHandler : IDataFileHandler
{
    public async void SaveData<T>(string saveName, T data)
    {
        Dictionary<string, object> saveData = new Dictionary<string, object> { { saveName, data } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(saveData);
    }

    public async void LoadData(string saveName, Action<string> onDataLoaded)
    { 

            Dictionary<string, string> loadData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { saveName });
            if (loadData != null && loadData.ContainsKey(saveName))
                onDataLoaded?.Invoke(loadData[saveName]);
    }
}
