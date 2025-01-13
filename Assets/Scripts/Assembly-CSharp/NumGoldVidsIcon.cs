using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NumGoldVidsIcon : MonoBehaviour
{
	public Image bg;

	public Text num;

	public virtual void OnEnable()
	{
		UpdateNum();
	}

	public virtual void UpdateNum()
	{
		int numGoldVids = WatchVideoButton.GetNumGoldVids();
		if (numGoldVids > 0)
		{
			bg.gameObject.SetActive(true);
			num.text = numGoldVids.ToString();
		}
		else
		{
			bg.gameObject.SetActive(false);
			num.text = string.Empty;
		}
	}
}
