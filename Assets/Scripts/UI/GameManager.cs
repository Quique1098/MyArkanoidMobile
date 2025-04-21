using UnityEngine;

public static class GameManager
{
    private static bool isGamePaused = false;

    // Método para pausar el juego
    public static void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    // Verificar si el juego está pausado
    public static bool IsGamePaused()
    {
        return isGamePaused;
    }
}
