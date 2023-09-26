using System;

public interface IDataFileHandler
{
    public void SaveData<T>(string saveName, T data);
    public void LoadData(string saveName, Action<string> onDataLoaded); 
}
