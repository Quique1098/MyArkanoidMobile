using UnityEngine;
using System.Collections;

public class PaddleSizePowerUp : PowerUp
{
    private Transform paddleTransform;
    private Vector3 originalScale;
    private Vector3 enlargedScale;

    [SerializeField] private Vector3 scaleIncrease = new Vector3(1.0f, 0.0f, 0.0f); 
    [SerializeField] private float powerUpDuration = 5f;
    protected override void OnActivate()
    {
        if (PaddleController.Instance != null)
        {
            GameEvents.OnPaletPowerUp?.Invoke();

            paddleTransform = PaddleController.Instance.transform;
            originalScale = paddleTransform.localScale;
            enlargedScale = originalScale + scaleIncrease;

            paddleTransform.localScale = enlargedScale;

            StartCoroutine(DeactivateAfterTime(powerUpDuration));
        }
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
    
        isCoroutineActive = true;

        yield return new WaitForSeconds(time);

        OnDeactivate();

        Destroy(gameObject);

        isCoroutineActive = false;
    }

    protected override void OnDeactivate()
    {
        if (PaddleController.Instance != null)
        {
            GameEvents.OnPaletPowerUp?.Invoke();

            paddleTransform = PaddleController.Instance.transform;

            paddleTransform.localScale = originalScale;

        }


    }


}
