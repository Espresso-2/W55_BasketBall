using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NumBagsIcon : MonoBehaviour
{
	public bool includeDailyBags;

	public bool includeStandardBags;

	public bool includePremiumBags;

	public Image bg;

	public Text num;

	public virtual void OnEnable()
	{
		UpdateNum();
	}

	public virtual void UpdateNum()
	{
		int num = 0;
		if (includeDailyBags)
		{
			num += DealBag.GetNumDailyBags();
		}
		if (includeStandardBags)
		{
			num += DealBag.GetNumStandardBags();
		}
		if (includePremiumBags)
		{
			num += DealBag.GetNumPremiumBags();
		}
		if (num > 0)
		{
			bg.gameObject.SetActive(true);
			this.num.text = num.ToString();
		}
		else
		{
			bg.gameObject.SetActive(false);
			this.num.text = string.Empty;
		}
	}
}
