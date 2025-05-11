using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaddleController : MonoBehaviour, ICustomUpdateable
{
    public static PaddleController Instance { get; private set; }
    private Renderer paddleRenderer;
    private Material originalMaterial;
    [SerializeField] private Material powerUpMaterial; 

    [SerializeField] private float speed = 10f;
    [SerializeField] private float boundaryX = 2f;
    [SerializeField] private int lives = 3;
    [SerializeField] private TextMeshProUGUI livesText;

    public WinoLose winoLose;

    [SerializeField] public Transform topPaddle; // asignar desde el Inspector
    [SerializeField] private float topPaddleY = 8f; // altura fija
    [SerializeField] private float topPaddleZ = 0f;
    [SerializeField] private float topBoundaryX = 1f;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

      
    }



    private void Start()
    {
        paddleRenderer = GetComponent<Renderer>();
        originalMaterial = paddleRenderer.material;
        CustomUpdateManager.Instance.AddObject(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        CustomUpdateManager.Instance.RemoveObject(this);
    }

    public void CustomStart() 
    {
        UpdateLivesText();

    }

    private float targetX; // posición objetivo del toque
    private bool hasTarget = false; // si hay un destino asignado
    public float touchDistanceFromCamera = 10f;


    public void CustomUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;
            touchPos.z = touchDistanceFromCamera;

            Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(touchPos);

            // Movimiento para paddle inferior (entre -2 y 2)
            targetX = Mathf.Clamp(worldTouchPos.x, -boundaryX, boundaryX);
            hasTarget = true;

            // Movimiento para paddle superior (entre -1 y 1)
            float topTargetX = Mathf.Clamp(worldTouchPos.x, -topBoundaryX, topBoundaryX);
            if (topPaddle != null)
            {
                float step = speed * Time.deltaTime;
                float newTopX = Mathf.MoveTowards(topPaddle.position.x, topTargetX, step);
                topPaddle.position = new Vector3(newTopX, topPaddleY, topPaddleZ);
            }
        }

        // Movimiento de paddle inferior
        if (hasTarget)
        {
            float step = speed * Time.deltaTime;
            float newX = Mathf.MoveTowards(transform.position.x, targetX, step);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (Mathf.Approximately(transform.position.x, targetX))
            {
                hasTarget = false;
            }
        }

    }






    //Para cambiar el material de la paleta tras powerup disparo
    public void ApplyPowerUpMaterial()
    {
        paddleRenderer.material = powerUpMaterial;
    }

    public void RestoreOriginalMaterial()
    {
        paddleRenderer.material = originalMaterial;
    }

    public void CustomFixedUpdate() { }
    public void CustomLateUpdate() { }
    public void CustomOnPause() { }
    public void ReduceLives()
    {
        if (lives > 0)
        {
            lives--;
            UpdateLivesText();
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }
    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives\n" + lives.ToString();
        }
    }

    private void GameOver()
    {
        winoLose.Lose();
    }
}






