using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GetGoldButton : MonoBehaviour
{
	public GetGoldBox getGoldBox;

	public virtual IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		AdMediation.CheckForEarnedCurrency();
		yield return new WaitForSeconds(0.5f);
		CheckForEarnedCurrency();
	}

	public virtual void ShowGetGoldBox()
	{
		getGoldBox.gameObject.SetActive(true);
	}

	public virtual void CheckForEarnedCurrency()
	{
		int[] intArray = PlayerPrefsX.GetIntArray(AdMediation.GOLD_OFFER_PREF);
		if (intArray.Length > 0 && intArray[0] > 0 && !getGoldBox.gameObject.activeInHierarchy)
		{
			getGoldBox.gameObject.SetActive(true);
		}
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (!pause && !getGoldBox.gameObject.activeInHierarchy)
		{
			AdMediation.ShowTjpAppResume();
		}
	}

	public virtual void Update()
	{
	}
}
