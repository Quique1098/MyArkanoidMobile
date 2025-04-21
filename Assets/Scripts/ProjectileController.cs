using UnityEngine;

public class ProjectileController : MonoBehaviour, ICustomUpdateable
{
    private Vector2 velocity;
    [SerializeField] protected float initialForce = 5f;
    private static ProjectileController activePowerupInstance = null;



    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    private void Start()
    {
        CustomUpdateManager.Instance.AddObject(this);

    }



    private void OnDestroy()
    {
        CustomUpdateManager.Instance.RemoveObject(this);
    }




    public void CustomUpdate()
    {
        velocity = new Vector2(0, 1).normalized * initialForce;
        transform.Translate(velocity * Time.deltaTime);

        CheckCollisions();
    }


    private void CheckCollisions()
    {
        //Colision con ladrillos
        for (int i = 0; i < BrickController.activeBricks.Count; i++)
        {
            if (CustomPhysics.SphereRectangleCollision(BrickController.activeBricks[i].GetComponent<Collider>(), GetComponent<SphereCollider>()))
            {
                BrickController.activeBricks[i].TakeDamage();

                Destroy(gameObject);
                break; //Se detiene en el primer brick que colisiona
            }
        }

        //Colision con techo
        if (transform.position.y >= 5.2f)
        {
            Destroy(gameObject);

        }
    }

  



    public void CustomStart()
    {

    }

    public void CustomFixedUpdate()
    {
    }

    public void CustomLateUpdate()
    {
    }

    public void CustomOnPause()
    {
    }
}
