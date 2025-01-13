using System;
using UnityEngine;

[Serializable]
public class RateButton : MonoBehaviour
{
	public bool isPrizeBall;

	public virtual void OnClick()
	{
		/*FlurryAnalytics.Instance().LogEvent("RATE_APP_CLICKED", new string[4]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
		GameObject.Find("GameSounds").SendMessage("Play_light_click");
		string text = "itms-apps://itunes.apple.com/app/id1086806819?action=write-review";
		text = ((!IsAmazon()) ? "market://details?id=com.doubletapsoftware.basketballbattle" : "amzn://apps/android?p=com.doubletapsoftware.basketballbattle");
		if (isPrizeBall)
		{
			text = "market://details?id=com.doubletapsoftware.prizeballgame";
		}
		Application.OpenURL(text);
	}

	public static bool IsAmazon()
	{
		bool result = false;
		string text = SystemInfo.deviceModel.ToLower();
		if (text.Contains("kindle") || text.Contains("amazon"))
		{
			result = true;
		}
		return result;
	}
}
