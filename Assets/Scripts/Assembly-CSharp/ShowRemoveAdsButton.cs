using System;
using UnityEngine;

[Serializable]
public class ShowRemoveAdsButton : MonoBehaviour
{
	public virtual void Start()
	{
		if (PlayerPrefs.GetInt("NUM_PURCHASES") >= 1 && PlayerPrefs.GetInt("DEVMODE") != 1)
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void Update()
	{
	}
}
