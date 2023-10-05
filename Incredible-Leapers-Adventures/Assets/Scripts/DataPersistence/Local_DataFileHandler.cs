using UnityEngine;
using System;
using System.IO;

public class Local_DataFileHandler : IDataFileHandler
{
    public void SaveData<T>(string saveName, T data)
    {
        Debug.Log(data);
        Debug.Log("data = " + data.ToString() );
        string filePath = string.Concat(Application.persistentDataPath,saveName);

        using(FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using(StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(data.ToString());
            }
        }
    }

    public void LoadData(string saveName, Action<string> onDataLoaded)
    {
        string dataToload = "";
        string filePath = string.Concat(Application.persistentDataPath, saveName);
        if (File.Exists(filePath))
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    dataToload = streamReader.ReadToEnd();
                }
            }
        }
        onDataLoaded?.Invoke(dataToload);
    }
}
