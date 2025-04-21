using UnityEngine;

public abstract class PowerUp : MonoBehaviour, ICustomUpdateable
{
    protected bool isActive;
    protected float duration;
    private float elapsedTime;
    [SerializeField] private float fallSpeed = 2.0f;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(15, 100, 45);
    protected bool isCoroutineActive = false;

    private void Start()
    {
        CustomUpdateManager.Instance.AddObject(this);
    }

    private void OnDestroy()
    {
        CustomUpdateManager.Instance.RemoveObject(this);
    }

    public virtual void Initialize(float duration)
    {
        this.duration = duration;
        elapsedTime = 0f;
        CustomUpdateManager.Instance.AddObject(this);
    }

    public void Activate()
    {
        if (isActive) return;
        isActive = true;
        OnActivate();
    }

    public void Deactivate()
    {
        //No desactivar si la corutina esta activa
        if (!isActive || isCoroutineActive) return; 
        isActive = false;
        OnDeactivate();
        CustomUpdateManager.Instance.RemoveObject(this);
    }

    public void CustomStart() { }

    public void CustomUpdate()
    {
        if (this == null || gameObject == null) return; 

        UpdateVisual();

        if (!isActive && gameObject != null)
        {
            CheckCollisions();
        }

        if (!isActive) return;

        //Solo se incrementa el tiempo si el powerup no esta siendo manejado por la corutina
        if (!isCoroutineActive)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                Deactivate();
            }
        }

        OnUpdate(Time.deltaTime);
    }

    public void CustomFixedUpdate() { }
    public void CustomLateUpdate() { }
    public void CustomOnPause() { }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    protected virtual void OnUpdate(float deltaTime) { }

    private void UpdateVisual()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void CheckCollisions()
    {
        //Colision con la paleta
        if (PaddleController.Instance != null &&
            CustomPhysics.SphereRectangleCollision(PaddleController.Instance.GetComponent<Collider>(), GetComponent<SphereCollider>()))
        {
            Activate();

            //Desactivar solo el componente de renderizado para powerups con timer
            Renderer _renderer = GetComponent<Renderer>();
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }
        }

        //Colision con el borde inferior
        if (transform.position.y <= -4.5f)
        {
            Destroy(gameObject);
        }
    }
}
