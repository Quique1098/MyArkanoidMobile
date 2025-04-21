using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class WinoLose : MonoBehaviour
{

    public static event Action OnWinEvent;

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

    public void Win(){
        SceneManager.LoadScene("WinMenu");
    }

    public void Lose(){
        SceneManager.LoadScene("LoseMenu");
    }

    public void Main(){
        SceneManager.LoadScene("MainMenu");
    }

}
