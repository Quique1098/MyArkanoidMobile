using UnityEngine;
using TMPro; // Para mostrar el puntaje en pantalla

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceso global
    public int score; // Puntaje actual
    public TextMeshProUGUI scoreText; // Texto para mostrar el puntaje

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIUpdate();

    }

    // Método para sumar puntos
    public void AddScore(int points)
    {
        score += points;
        UIUpdate();
    }

    public void UIUpdate()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
