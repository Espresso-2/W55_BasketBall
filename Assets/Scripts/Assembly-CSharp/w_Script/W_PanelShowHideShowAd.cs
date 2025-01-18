using System;
using System.Collections;
using System.Collections.Generic;
using PublicComponentCenter;
using UnityEngine;

public class W_PanelShowHideShowAd : MonoBehaviour
{
    private void OnEnable()
    {
        GameEntry.Ad.ShowInterstitialAd();
    }

    private void OnDisable()
    {
        GameEntry.Ad.ShowInterstitialAd();
    }
}