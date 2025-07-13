using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId = "5898930";
    [SerializeField] private string rewardedAdUnitId = "Rewarded_Android";
    [SerializeField] private string interstitialAdUnitId = "Interstitial_Android";
    [SerializeField] private bool testMode = true;

    private bool interstitialLoaded = false;
    private bool rewardedLoaded = false;

    void Start()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(androidGameId, testMode, this);

#endif

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads inicializado correctamente");
        Advertisement.Load(interstitialAdUnitId, this);
        Advertisement.Load(rewardedAdUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Init Falló: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == interstitialAdUnitId) interstitialLoaded = true;
        if (placementId == rewardedAdUnitId) rewardedLoaded = true;
        Debug.Log($"Ad cargado: {placementId}");
    }

    public void ShowInterstitial()
    {
        if (interstitialLoaded)
        {
            Advertisement.Show(interstitialAdUnitId, this);
        }
        else
        {
            Debug.Log("Interstitial aún no cargado");
        }
    }

    public void ShowRewarded()
    {
        if (rewardedLoaded)
        {
            Advertisement.Show(rewardedAdUnitId, this);
        }
        else
        {
            Debug.Log("Rewarded aún no cargado");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED ||
            showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            //Time.timeScale = 1f;

            if (placementId == interstitialAdUnitId)
            {
                Debug.Log("Interstitial terminado.");
                //SceneManager.LoadScene("MainMenu");
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

            }
            else if (placementId == rewardedAdUnitId)
            {
                Debug.Log("¡Recompensa otorgada!");
                //SceneManager.LoadScene("MainMenu");
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);


            }

            Advertisement.Load(placementId, this);
        }
    }



    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error al cargar ad {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error al mostrar ad {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) => Debug.Log($"Ad empezado: {placementId}");
    public void OnUnityAdsShowClick(string placementId) => Debug.Log($"Ad clickeado: {placementId}");
}

