using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public static class CloudSaveInitializer
{
    public static bool IsSignedIn { get; private set; } = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async void Initialize()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("[CloudSave] Signed in anonymously");
            }

            IsSignedIn = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("[CloudSave] Initialization failed: " + e.Message);
        }
    }
}

