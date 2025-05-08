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

    private static float saveInterval = 5f;
    private static float saveTimer = 0f;

    public static void SaveGame()
    {
        if (ScoreManager.Instance == null || PaddleController.Instance == null)
        {
            Debug.LogWarning("No se puede guardar: objetos no cargados aún.");
            return;
        }

        GameData data = new GameData
        {
            score = ScoreManager.Instance.score,
            paddlePosition = new SerializableVector3(PaddleController.Instance.transform.position)
        };

        EncryptedJSONSaveSystem.Save(data);
    }

    public static void LoadGame()
    {
        GameData data = EncryptedJSONSaveSystem.Load();
        if (data == null)
        {
            Debug.LogWarning("No hay datos para cargar.");
            return;
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.score = data.score;

        if (PaddleController.Instance != null)
            PaddleController.Instance.transform.position = data.paddlePosition.ToVector3();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void InitAutoSave()
    {
        GameObject autoSave = new GameObject("AutoSaveUpdater");
        autoSave.AddComponent<AutoSaveUpdater>();
        GameObject.DontDestroyOnLoad(autoSave);
    }
}
