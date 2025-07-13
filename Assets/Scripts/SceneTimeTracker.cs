using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class SceneTimeTracker : MonoBehaviour
{
    public static SceneTimeTracker Instance { get; private set; }

    private float timeSpent = 0f;
    private string currentScene;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        timeSpent += Time.deltaTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Enviar evento de la escena anterior
        if (!string.IsNullOrEmpty(currentScene) && timeSpent > 0)
        {
            EnviarEventoTiempo(currentScene, timeSpent);
        }

        // Reiniciar para la nueva escena
        currentScene = scene.name;
        timeSpent = 0f;
    }

    private void EnviarEventoTiempo(string sceneName, float seconds)
    {
        var evt = new CustomEvent("SceneTimeSpent")
        {
            { "scene_name", sceneName },
            { "seconds", Mathf.RoundToInt(seconds) }
        };

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush(); // opcional
        Debug.Log($"Evento SceneTimeSpent enviado: {sceneName} - {Mathf.RoundToInt(seconds)}s");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

