using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BallController : MonoBehaviour, ICustomUpdateable
{
    [SerializeField] private float restitution = 0.8f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float initialForce = 5f;

    //Bola rotacion efecto visual
    private Renderer ballRenderer;
    private Material material;
    //[SerializeField] private float rotationSpeed = 1f; 
    private Vector2 textureOffset = Vector2.zero;
    private Vector2 rotationSpeed = new Vector2(0.05f, 0.1f);
    public Vector2 velocity;
    public bool isLaunched;

    private bool hasReducedLife = false;

    //Lista para las bolas de powerup
    public static List<BallController> activeBalls = new List<BallController>();

    private BallsPoolObject ballsPoolObject;

    [SerializeField] private Transform topPaddle;
    [SerializeField] private Collider topPaddleCollider;


    private void Start()
    {
        ballRenderer = GetComponent<Renderer>();
        material = GetComponent<Renderer>().material;

        ballsPoolObject = GetComponent<BallsPoolObject>();

        CustomUpdateManager.Instance.AddObject(this);
        activeBalls.Add(this);
    }

    private void OnDestroy()
    {
        if (CustomUpdateManager.Instance != null)
        {
            CustomUpdateManager.Instance.RemoveObject(this);
        }

        if (activeBalls.Contains(this))
        {
            activeBalls.Remove(this);
        }

        if (ballsPoolObject != null)
        {
            ballsPoolObject.ReturnToPool(gameObject);
        }
    }



    public void CustomStart()
    {
        ResetBallPosition();

        if (TopPaddleController.Instance != null)
        {
            topPaddle = TopPaddleController.Instance.transform;
            topPaddleCollider = TopPaddleController.Instance.PaddleCollider;
        }
    }



    public void CustomUpdate()
    {
        if (!isLaunched)
        {
            //Mover la bola con el paddle antes de lanzarla
            AttachToPaddle();
            if (Input.touchCount > 0)
            {
                GameEvents.OnReleaseBall?.Invoke();
                LaunchBall();

            }
        }
        else
        {

            // Obtén el offset actual
            Vector2 offset = material.mainTextureOffset;

            // Calcula el nuevo offset con el tiempo
            offset += rotationSpeed * Time.deltaTime; // Multiplica Vector2 por Time.deltaTime

            // Asegúrate de que el offset esté en el rango válido (0 a 1)
            offset.x = Mathf.Repeat(offset.x, 1);
            offset.y = Mathf.Repeat(offset.y, 1);

            // Aplica el nuevo offset al material
            material.mainTextureOffset = offset;

            ////Efecto visual rotaicon
            //textureOffset.y += rotationSpeed * Time.deltaTime;
            //ballRenderer.material.mainTextureOffset = textureOffset; 

            velocity = CustomPhysics.LimitSpeed(velocity, maxSpeed);
            MoveBall();
            CheckCollisions();
        }
    }

    public void CustomFixedUpdate() { }
    public void CustomLateUpdate() { }
    public void CustomOnPause() { }

    public void LaunchImmediately(Vector2 launchDirection)
    {

        velocity = launchDirection.normalized * initialForce;
        isLaunched = true;
    }

    private void AttachToPaddle()
    {
        // Posicionar la bola sobre el paddle mientras no ha sido lanzada
        if (PaddleController.Instance != null)
        {
            transform.position = PaddleController.Instance.transform.position + Vector3.up * 0.3f;
        }
    }

    private void LaunchBall()
    {
        // Generar un valor aleatorio pequeño en el eje X
        float randomX = Random.Range(-0.5f, 0.5f);

        // Lanzar la bola hacia arriba con una ligera variación en el eje X
        velocity = new Vector2(randomX, 1).normalized * initialForce;
        isLaunched = true;
    }


    public void ResetBallPosition()
    {
        // Reiniciar la posición de la bola en el paddle
        isLaunched = false;
        velocity = Vector2.zero;
        AttachToPaddle();
     
    }


    private void MoveBall()
    {
        // Mover la bola de acuerdo con la velocidad actual
        transform.Translate(velocity * Time.deltaTime);
    }

    private void CheckCollisions()
    {
        // Colisión con los bordes de la pantalla - lado izquierdo y derecho
        if (transform.position.x <= -2.4f || transform.position.x >= 2.4f)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            velocity.x = (-velocity.x + randomX) * restitution;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.4f, 2.4f), transform.position.y, 0);
            GameEvents.OnBallCollision?.Invoke();
        }

        // Colisión con el borde superior
        if (transform.position.y >= 5.8f)
        {
            if (activeBalls.Count > 1)
            {
                //Remover las bolas powerup
                activeBalls.Remove(this);
                CustomUpdateManager.Instance.RemoveObject(this);
                Destroy(gameObject);
                GameEvents.OnLifeLost?.Invoke();
            }
            else if (activeBalls.Count == 1 && !hasReducedLife)
            {
                hasReducedLife = true;
                ResetBallPosition();
                ScoreManager.Instance.AddScore(-20);
                PaddleController.Instance?.ReduceLives();

                //Reiniciamos flag y ponemos un frame de retraso para evitar que se reste mas de una vida por frame
                Invoke(nameof(ResetLifeFlag), 0.1f);
                GameEvents.OnLifeLost?.Invoke();
            }
        }

        // Colisión con el borde inferior (reiniciar si la bola cae)
        if (transform.position.y <= -5.5f)
        {
            if (activeBalls.Count > 1)
            {
                //Remover las bolas powerup
                activeBalls.Remove(this);
                CustomUpdateManager.Instance.RemoveObject(this);
                Destroy(gameObject);
                GameEvents.OnLifeLost?.Invoke();
            }
            else if (activeBalls.Count == 1 && !hasReducedLife)
            {
                hasReducedLife = true;
                ResetBallPosition();
                ScoreManager.Instance.AddScore(-20);
                PaddleController.Instance?.ReduceLives();

                //Reiniciamos flag y ponemos un frame de retraso para evitar que se reste mas de una vida por frame
                Invoke(nameof(ResetLifeFlag), 0.1f);
                GameEvents.OnLifeLost?.Invoke();
            }
        }

        //Colision con la paleta
        // Rebote con paleta inferior
        HandlePaddleCollision(PaddleController.Instance.transform, PaddleController.Instance.GetComponent<Collider>());

        // Rebote con paleta superior
        if (topPaddle != null && topPaddleCollider != null)
            HandlePaddleCollision(topPaddle, topPaddleCollider);




        for (int i = 0; i < BrickController.activeBricks.Count; i++)
        {
            Collider brickCollider = BrickController.activeBricks[i].GetComponent<Collider>();

            if (CustomPhysics.SphereRectangleCollision(brickCollider, GetComponent<SphereCollider>()))
            {
                Bounds brickBounds = brickCollider.bounds;
                Vector3 ballPosition = transform.position;

                Vector3 brickCenter = brickBounds.center;
                Vector3 diff = ballPosition - brickCenter;
                diff.z = 0; // Ignoramos Z porque es 2D visual en 3D

                Vector3 brickExtents = brickBounds.extents;

                float overlapX = brickExtents.x - Mathf.Abs(diff.x);
                float overlapY = brickExtents.y - Mathf.Abs(diff.y);

                if (overlapX < overlapY)
                {
                    // Colisión por izquierda o derecha
                    velocity.x = -velocity.x * restitution;

                    if (diff.x > 0)
                    {
                        // Desde la derecha
                        ballPosition.x = brickBounds.max.x + GetComponent<SphereCollider>().radius * transform.lossyScale.x;
                    }
                    else
                    {
                        // Desde la izquierda
                        ballPosition.x = brickBounds.min.x - GetComponent<SphereCollider>().radius * transform.lossyScale.x;
                    }
                }
                else
                {
                    // Colisión por arriba o abajo
                    velocity.y = -velocity.y * restitution;

                    if (diff.y > 0)
                    {
                        // Desde arriba
                        ballPosition.y = brickBounds.max.y + GetComponent<SphereCollider>().radius * transform.lossyScale.y;
                    }
                    else
                    {
                        // Desde abajo
                        ballPosition.y = brickBounds.min.y - GetComponent<SphereCollider>().radius * transform.lossyScale.y;
                    }
                }

                transform.position = ballPosition;

                BrickController.activeBricks[i].TakeDamage();
                GameEvents.OnBallCollision?.Invoke();

                break; // Solo colisionamos con un ladrillo por frame
            }
        }





        //Evitar colision entre bolas
        foreach (var ball in activeBalls)
        {
            if (ball != this && CustomPhysics.SphereCollision(GetComponent<SphereCollider>(), ball.GetComponent<SphereCollider>()))
            {
                //Ignorar colision
                continue;
            }
        }
    }

    private void ResetLifeFlag()
    {
        hasReducedLife = false; 
    }

    private void HandlePaddleCollision(Transform paddleTransform, Collider paddleCollider)
    {
        // Verificar distancia vertical antes
        float yDistance = Mathf.Abs(transform.position.y - paddleTransform.position.y);
        float maxAllowedDistance = 0.5f; // Ajustá este valor según el tamaño de tu paddle y pelota

        if (yDistance <= maxAllowedDistance)
        {
            if (CustomPhysics.SphereRectangleCollision(paddleCollider, GetComponent<SphereCollider>()))
            {
                float ballScaleX = transform.lossyScale.x;
                float paddleScaleX = paddleTransform.lossyScale.x;

                Vector2 bounceForce = CustomPhysics.CalculatePaddleBounce(transform.position, paddleTransform, velocity.magnitude, restitution, ballScaleX, paddleScaleX);
                velocity = bounceForce;

                // Ajustar dirección Y correctamente según si venís de abajo o de arriba
                velocity.y = Mathf.Sign(transform.position.y - paddleTransform.position.y) * Mathf.Abs(velocity.y);

                GameEvents.OnBallCollision?.Invoke();
            }
        }
    }


}












