using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioClip buttonSound; 
    public AudioSource audioSource; 
    public AudioSource backgroundMusicSource;

    public string sceneName;


    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    public void OnButtonClick()
    {
        // Detener la música de fondo
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

        // Reproducir el sonido del botón
        if (audioSource != null && buttonSound != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }

        // Cargar la escena después de que el sonido termine
        StartCoroutine(LoadSceneAfterSound());
    }
    // Método para cargar una escena específica
    private IEnumerator LoadSceneAfterSound()
    {
        // Espera a que el sonido termine
        yield return new WaitForSeconds(buttonSound.length);

        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
}
