using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_PanelShowHideShowAd : MonoBehaviour
{
    private void OnEnable()
    {
        if (AdTotalManager.Instance != null)
        {
            AdTotalManager.Instance.ShowWhiteAd();
        }
    }

    private void OnDisable()
    {
        if (AdTotalManager.Instance != null)
        {
            AdTotalManager.Instance.ShowBlackAd();
        }
    }
}