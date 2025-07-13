using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonUI : MonoBehaviour
{
    public AdsManager adsManager;

    public void ClickMostrarRewarded() => adsManager.ShowRewarded();
    public void ClickMostrarInterstitial() => adsManager.ShowInterstitial();
}

