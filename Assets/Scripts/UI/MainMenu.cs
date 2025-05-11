
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioSource audioSource;
    public AudioSource backgroundMusicSource;

    public string sceneName;

    public Button continueButton;
    public Button deleteDataButton;

    private async void Start()
    {
        CheckSaveDataAvailability();

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

    public async void DeleteSaveData()
    {
        // Borrar local
        EncryptedJSONSaveSystem.DeleteSaveData();

        // Borrar en la nube
        await CloudSaveSystem.DeleteAsync();

        CheckSaveDataAvailability();


    }


    private async void CheckSaveDataAvailability()
    {
        // Esperar hasta que esté inicializado
        while (!CloudSaveInitializer.IsSignedIn)
        {
            await Task.Yield();
        }

        bool hasLocal = EncryptedJSONSaveSystem.HasSaveData();
        bool hasCloud = await CloudSaveSystem.HasCloudSaveAsync();

        bool hasAnySave = hasLocal || hasCloud;

        if (continueButton != null)
            continueButton.interactable = hasAnySave;

        if (deleteDataButton != null)
            deleteDataButton.interactable = hasAnySave;

        Debug.Log($"[MainMenu] Save availability — Local: {hasLocal}, Cloud: {hasCloud}, Any: {hasAnySave}");
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
