using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using System;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;

public static class CloudSaveSystem
{
    private const string key = "save";

    public static async Task DeleteAsync()
    {
        try
        {
            var options = new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions(); // por ahora no hay propiedades necesarias
            await CloudSaveService.Instance.Data.Player.DeleteAsync(key, options);
            Debug.Log("Guardado en la nube eliminado (nueva API).");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al borrar los datos en la nube: " + e.Message);
        }
    }


    public static async Task<bool> HasCloudSaveAsync()
    {
        try
        {
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            bool exists = result.ContainsKey(key);
            Debug.Log($"[CloudSaveSystem] HasCloudSaveAsync(): result.ContainsKey => {exists}");
            return exists;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CloudSaveSystem] HasCloudSaveAsync() failed: {e.Message}");
            return false;
        }
    }




    public static async Task SaveRawAsync(string encryptedJson)
    {
        try
        {
            var data = new Dictionary<string, object>
        {
            { "save", encryptedJson }
        };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Cloud save successful.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving to cloud: " + e.Message);
        }
    }




    public static async Task<string> LoadRawAsync()
    {
        var result = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "save" });
        if (result.TryGetValue("save", out var item))
            return item.Value.GetAsString();

        return null;
    }
}

