using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Canvas del menú de pausa
    public TextMeshProUGUI pauseText; // Texto "Pause" que parpadea
    public float blinkSpeed = 1f; // Velocidad del parpadeo
    private bool isFadingOut = true; // Control del parpadeo del texto

    void Start()
    {
        // Asegurarse de que el Canvas de pausa esté desactivado al inicio
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Detectar si se presiona la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Mostrar el menú de pausa
    public void PauseGame()
    {
        GameManager.TogglePause(); // Pausar el juego globalmente
        pauseMenuCanvas.SetActive(true); // Activar el Canvas del menú de pausa
        if (pauseText != null)
        {
            StartCoroutine(BlinkPauseText()); // Iniciar el parpadeo del texto
        }
    }

    // Ocultar el menú de pausa
    public void ResumeGame()
    {
        GameManager.TogglePause(); // Reanudar el juego globalmente
        pauseMenuCanvas.SetActive(false); // Desactivar el Canvas del menú de pausa
        if (pauseText != null)
        {
            StopCoroutine(BlinkPauseText()); // Detener el parpadeo del texto
            pauseText.color = new Color(pauseText.color.r, pauseText.color.g, pauseText.color.b, 1); // Asegurar que sea completamente visible
        }
    }

    // Regresar al menú principal
    public void LoadMainMenu()
    {
        GameManager.TogglePause(); // Asegurarse de restaurar el tiempo
        SceneManager.LoadScene("MainMenu"); // Cambiar a la escena del menú principal
    }

    // Parpadeo del texto "Pause"
    private IEnumerator BlinkPauseText()
    {
        while (GameManager.IsGamePaused())
        {
            // Cambiar la opacidad del texto para simular el parpadeo
            Color color = pauseText.color;
            color.a += (isFadingOut ? -1 : 1) * blinkSpeed * Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(color.a); // Limitar el valor de alpha entre 0 y 1
            pauseText.color = color;

            // Cambiar la dirección del parpadeo cuando alcanza los extremos
            if (color.a <= 0) isFadingOut = false;
            if (color.a >= 1) isFadingOut = true;

            yield return null; // Esperar al siguiente frame
        }
    }
}
