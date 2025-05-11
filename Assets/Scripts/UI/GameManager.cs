using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using System.Linq;
using System.Collections.Generic;

public static class GameManager
{
    private static bool isGamePaused = false;

    private static GameObject _ballPrefab;

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
            paddlePosition = new SerializableVector3(PaddleController.Instance.transform.position),
            topPaddlePosition = new SerializableVector3(PaddleController.Instance.topPaddle.position),

            bricks = new List<BrickState>()
        };

        // Guardar estado de las pelotas

        foreach (var ball in BallController.activeBalls)
        {
            data.balls.Add(new BallState
            {
                position = new SerializableVector3(ball.transform.position),
                velocity = new SerializableVector3(new Vector3(ball.velocity.x, ball.velocity.y, 0f)),
                isLaunched = ball.isLaunched
            });
        }

        // Guardar estado de los bricks
        foreach (var brick in BrickController.allBricks)
        {
            data.bricks.Add(new BrickState
            {
                id = brick.id,
                isDestroyed = !brick.gameObject.activeSelf,
                health = brick.health,
                position = new SerializableVector3(brick.transform.position)
            });
        }

        Debug.Log("Intentando salvar datos...");

        // Guardado local encriptado
        EncryptedJSONSaveSystem.Save(data);

        // Guardado en la nube
        _ = SaveGameToCloudAsync(data);
    }


    private static async Task SaveGameToCloudAsync(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string encrypted = EncryptedJSONSaveSystem.EncryptRaw(json);
        await CloudSaveSystem.SaveRawAsync(encrypted);
    }


    public static async void LoadGame()
    {
        try
        {

            GameObject bricksParent = GameObject.Find("BricksParent");
            if (bricksParent != null)
                bricksParent.SetActive(false);

            GameData data = null;

            // Primero  cargar desde la nube
            string encryptedJson = await CloudSaveSystem.LoadRawAsync();

            if (!string.IsNullOrEmpty(encryptedJson))
            {
                string decryptedJson = EncryptedJSONSaveSystem.DecryptRaw(encryptedJson);
                data = JsonUtility.FromJson<GameData>(decryptedJson);
                Debug.Log("[LoadGame] Datos cargados desde la nube.");
            }
            else
            {
                // Si no hay en la nube, intentamos local
                data = EncryptedJSONSaveSystem.Load();
                if (data != null)
                    Debug.Log("[LoadGame] Datos cargados desde local.");
            }

            if (data == null)
            {
                Debug.LogWarning("[LoadGame] No se encontraron datos de guardado.");
                return;
            }

            // Restaurar score
            ScoreManager.Instance.score = data.score;
            ScoreManager.Instance.UIUpdate();


            // Limpia pelotas existentes
            foreach (var ball in BallController.activeBalls.ToList())
            {
                UnityEngine.Object.Destroy(ball.gameObject);
            }
            BallController.activeBalls.Clear();

            // Instanciar bolas guardadas
            foreach (var ballState in data.balls)
            {
                GameObject prefab = GetBallPrefab();
                if (prefab == null) continue;

                GameObject newBall = UnityEngine.Object.Instantiate(prefab, ballState.position.ToVector3(), Quaternion.identity);
                BallController ballController = newBall.GetComponent<BallController>();

                ballController.velocity = new Vector2(ballState.velocity.x, ballState.velocity.y);
                ballController.isLaunched = ballState.isLaunched;

                if (!ballState.isLaunched)
                {
                    ballController.ResetBallPosition();
                }
            }



            // Restaurar posición de la paleta
            if (PaddleController.Instance != null)
            {
                PaddleController.Instance.transform.position = data.paddlePosition.ToVector3();
                PaddleController.Instance.topPaddle.position = data.topPaddlePosition.ToVector3();


            }

            // Restaurar estado de los ladrillos
            foreach (var brickState in data.bricks)
            {
                var brick = BrickController.allBricks.FirstOrDefault(b => b.id == brickState.id);

                if (brick != null)
                {
                    Debug.Log($"[LoadGame] Procesando Brick ID: {brick.id} | isDestroyed: {brickState.isDestroyed} | Health: {brickState.health}");


                    if (brickState.isDestroyed)
                    {
                        brick.gameObject.SetActive(false);
                        Debug.Log($"[LoadGame] Desactivando brick {brick.id}");



                    }
                    else
                    {
                        brick.gameObject.SetActive(true);
                        Debug.Log($"[LoadGame] Activando brick {brick.id}");

                        // Restaurar vida
                        brick.health = brickState.health;

                        // Restaurar posición (si corresponde)
                        brick.transform.position = brickState.position.ToVector3();

                    }

                }
                else
                {
                    Debug.LogWarning($"[LoadGame] Brick con ID {brickState.id} no encontrado en la escena.");
                }
            }

            if (bricksParent != null)
                bricksParent.SetActive(true);


            Debug.Log("[LoadGame] Carga de datos completada.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[LoadGame] Error al cargar datos: {e.Message}");
        }
    }





    private static GameObject GetBallPrefab()
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
            if (_ballPrefab == null)
            {
                Debug.LogError("[GameManager] No se encontró el prefab de la pelota en Resources/Prefabs/Ball");
            }
        }
        return _ballPrefab;
    }
}
