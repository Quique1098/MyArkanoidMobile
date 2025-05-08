
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioSource audioSource;
    public AudioSource backgroundMusicSource;

    public string sceneName;

    public Button continueButton;
    public Button deleteDataButton;

    void Start()
    {
        // Desactiva el botón Continue si no hay guardado
        if (continueButton != null)
            continueButton.interactable = EncryptedJSONSaveSystem.HasSaveData();

        if (deleteDataButton != null)
            deleteDataButton.interactable = EncryptedJSONSaveSystem.HasSaveData();
    }


    public void PlayNewGame()
    {
        StartCoroutine(PlayScene(withLoad: false));
    }

    public void ContinueGame()
    {
        StartCoroutine(PlayScene(withLoad: true));
    }

    private IEnumerator PlayScene(bool withLoad)
    {
        if (backgroundMusicSource != null)
            backgroundMusicSource.Stop();

        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);

        yield return new WaitForSeconds(buttonSound.length);

        if (withLoad)
        {
            // Suscribirse a evento para cargar datos una vez cargada la escena
            SceneManager.sceneLoaded += OnSceneLoadedLoadGame;
        }

        SceneManager.LoadScene(sceneName);
    }

    // Este método se ejecuta automáticamente cuando la escena termina de cargarse
    private void OnSceneLoadedLoadGame(Scene scene, LoadSceneMode mode)
    {
        GameManager.LoadGame();
        SceneManager.sceneLoaded -= OnSceneLoadedLoadGame; // Desuscribirse para no duplicar
    }

    public void DeleteSaveData()
    {
        EncryptedJSONSaveSystem.DeleteSaveData();

        // Actualizar el estado del botón Continue
        if (continueButton != null)
            continueButton.interactable = false;

        if (deleteDataButton != null)
            deleteDataButton.interactable = false;
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
