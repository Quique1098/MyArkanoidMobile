using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Authentication;
public class WinoLose : MonoBehaviour
{
    public static event Action OnWinEvent;
    public AdsManager adsManager;

    private void OnEnable()
    {
        OnWinEvent += Win;
    }

    private void OnDisable()
    {
        OnWinEvent -= Win;
    }

    public static void TriggerWin()
    {
        OnWinEvent?.Invoke();
    }

    public void Win()
    {
        EnviarEventoWin();
        SceneManager.LoadScene("WinMenu");
    }

    public void Lose()
    {
        SceneManager.LoadScene("LoseMenu");
    }

    public void Main()
    {
        adsManager.ShowRewarded();
        SceneManager.LoadScene("MainMenu");
    }

    private void EnviarEventoWin()
    {
        int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
        string playerId = AuthenticationService.Instance.PlayerId;

        var evento = new CustomEvent("PlayerWon")
        {
            { "score", finalScore }
        };

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush(); // Opcional
        Debug.Log($"Evento PlayerWon enviado: player_id={playerId}, score={finalScore}");
    }
}

