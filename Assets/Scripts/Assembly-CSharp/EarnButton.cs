using System;
using UnityEngine;

[Serializable]
public class EarnButton : MonoBehaviour
{
    public virtual void Start()
    {
    }

    public virtual void OnEnable()
    {
        bool active = false;
        if (AdMediation.IsTjpOfferWallAvail())
        {
            active = true;
        }
        base.gameObject.SetActive(active);
    }

    public virtual void DisplayOffers()
    {
        AdMediation.ShowTjpOfferWall();
    }
}