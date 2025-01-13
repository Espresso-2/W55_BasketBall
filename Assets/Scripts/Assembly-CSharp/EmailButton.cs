using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EmailButton : MonoBehaviour
{
	public bool isPrizeBall;

	public Text supportIdText;

	public bool isSuggestion;

	private string userTag;

	public virtual void Start()
	{
		userTag = PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
		if (supportIdText != null)
		{
			supportIdText.text = "ID: " + userTag;
		}
	}

	public virtual void OnClick()
	{
		string text = "Basketball%20Battle";
		if (isSuggestion)
		{
			text = "SUGGESTION";
		}
		string text2 = "(" + Stats.GetNumWins() + "%20" + Stats.GetNumLosses() + "%20t" + PlayerPrefs.GetInt("IAP_TOTAL_IN_USD") + ")";
		if (isPrizeBall)
		{
			Application.OpenURL("mailto:prizeBallAndroid@DoubleTapSoftware.com?Subject=" + text + "%20(ID:" + userTag + ")" + text2);
		}
		else
		{
			Application.OpenURL("mailto:battleHelpDroid@DoubleTapSoftware.com?Subject=" + text + "%20(ID:" + userTag + ")" + text2);
		}
	}
}
