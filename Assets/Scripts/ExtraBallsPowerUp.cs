using UnityEngine;

public class ExtraBallsPowerUp : PowerUp
{
    [SerializeField] private GameObject ballPrefab;

    protected override void OnActivate()
    {
        if (PaddleController.Instance != null)
        {
            transform.position = PaddleController.Instance.transform.position + Vector3.up * 0.3f;

            Vector2[] launchDirections = { new Vector2(-0.5f, 1), new Vector2(0.5f, 1) };

            foreach (var direction in launchDirections)
            {
                GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                BallController ballController = newBall.GetComponent<BallController>();

                if (ballController != null)
                {
                    ballController.LaunchImmediately(direction);
                }
            }
        }
    }

    protected override void OnDeactivate()
    {
        Destroy(gameObject);

    }
}
