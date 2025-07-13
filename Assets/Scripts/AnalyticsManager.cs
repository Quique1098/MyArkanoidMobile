using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Threading.Tasks;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    async void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional, por si querés que persista entre escenas

        await InitializeAnalytics();
        AnalyticsService.Instance.StartDataCollection();

    }

    async Task InitializeAnalytics()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Analytics inicializado.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al inicializar Analytics: {e.Message}");
        }
    }

    public void EnviarEventoPlay()
    {
        CustomEvent evt = new CustomEvent("PlayButton")
{
    { "Play_Count", 1 },
};

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();

        Debug.Log("Evento PlayButton enviado.");
    }


}

