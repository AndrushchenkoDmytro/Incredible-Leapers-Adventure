using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;
using System;
using System.Text;

public class GPGS_CloudDataFileHandler : IDataFileHandler
{
    private string loadedData = string.Empty;
    private bool canInteract = true;
    private Action<string> returnResult;
    private DataSource dataSource = DataSource.ReadCacheOrNetwork;
    private ConflictResolutionStrategy conflictStrategy = ConflictResolutionStrategy.UseLongestPlaytime;

    public void LoadData(string saveName, Action<string> onDataLoaded)
    {
        if (canInteract)
        {
            loadedData = string.Empty;
            returnResult = onDataLoaded;
            canInteract = false;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveName, dataSource, conflictStrategy, OnCloudStorageOpen);
        }
        else
        {
            Debug.Log("Failed to Interact");
        }
    }

    private void OnCloudStorageOpen(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metadata, OnStorageDataReaded);
        }
        else
        {
            Debug.Log(" Failed to open saved game.");
            returnResult?.Invoke(null);
            canInteract = true;
        }
    }

    private void OnStorageDataReaded(SavedGameRequestStatus status, byte[] savedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            loadedData = Encoding.ASCII.GetString(savedData);
            Debug.Log(" Loaded data: " + loadedData + "||");
        }
        else
        {
            loadedData = string.Empty;
            Debug.Log("Failed to read saved game data.");
        }
        returnResult?.Invoke(loadedData);
        canInteract = true;
    }

    public void SaveData<T>(string saveName,T data)
    {
        if (canInteract)
        {
            canInteract = false;

            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveName, dataSource, conflictStrategy, (status, metadata) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    byte[] dataToSave = Encoding.ASCII.GetBytes(data.ToString());
                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                    ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metadata, update, dataToSave, OnStorageDataWritten);
                }
                else
                {
                    Debug.Log("Failed to open saved game for saving.");
                    canInteract = true;

                }
            });
        }
    }

    private void OnStorageDataWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Data saved successfully.");
        }
        else
        {

            Debug.Log("Failed to write saved game data.");
        }
        canInteract = true;
    }
}
