using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para contener los datos a guardar
[System.Serializable]
public class GameData
{
    public int score;

    public SerializableVector3 paddlePosition;
    public SerializableVector3 topPaddlePosition;

    public List<BrickState> bricks;

    public List<BallState> balls;

    public List<PowerUpState> activePowerUps;

    public string currentScene;

    public GameData()
    {
        bricks = new List<BrickState>();
        balls = new List<BallState>();
        activePowerUps = new List<PowerUpState>();
    }
}

// Estructuras vectoriales para serialización
[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public class BrickState
{
    public string id; // ID único del brick
    public bool isDestroyed;
    public int health; // Vida actual del ladrillo
    public SerializableVector3 position; // Posición actual
}

[System.Serializable]
public class BallState
{
    public SerializableVector3 position;
    public SerializableVector3 velocity;
    public bool isLaunched;
}

[System.Serializable]
public class PowerUpState
{
    public string type; // tipo del power-up
    public SerializableVector3 position;
    public bool isActive;
}
