using UnityEngine;
using System.Collections;

public class ShootingPowerUp : PowerUp
{
    public GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.5f;
    private float fireCooldown = 0f;
    private bool isPowerUpActive = false;
    [SerializeField] private float powerUpDuration = 5f;
    private Material originalMaterial;


    protected override void OnActivate()
    {
     
            isPowerUpActive = true;
            fireCooldown = 0f;

        //Cambiar el material de la paleta cuando se agarre el powerup
        if (PaddleController.Instance != null)
        {
            originalMaterial = PaddleController.Instance.GetComponent<Renderer>().material;
            PaddleController.Instance.ApplyPowerUpMaterial();
        }

        StartCoroutine(DeactivateAfterTime(powerUpDuration));

    }


    private IEnumerator DeactivateAfterTime(float time)
    {
    
        isCoroutineActive = true;

        yield return new WaitForSeconds(time);

        if (PaddleController.Instance != null)
        {
            PaddleController.Instance.RestoreOriginalMaterial();
        }

        OnDeactivate();

        Destroy(gameObject);

        isCoroutineActive = false;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (isPowerUpActive)
        {
            fireCooldown -= deltaTime;
            if (fireCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameEvents.OnShootProyectilePowerUp?.Invoke();
                    FireProjectile();
                    fireCooldown = fireRate;
                }
            }
        }
    }

    private void FireProjectile()
    {
        if (isPowerUpActive)
        {
            //Instancia proyectil 
            GameObject projectile = Instantiate(projectilePrefab, PaddleController.Instance.transform.position, Quaternion.identity);
        
        }
    }



    protected override void OnDeactivate()
    {

        Destroy(gameObject);

    }
}
