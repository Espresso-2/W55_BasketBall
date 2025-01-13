using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

[Serializable]
public class LockedButton : MonoBehaviour
{
	public GameObject msg;

	public Localize msgText;

	public int minTournament;

	public int minWins;

	public int minXP;

	public bool mustHaveFemaleStarterSigned;

	public int NeverChangeMinWithoutUpdatingNotificationsToo;

	private bool isLocked;

	public LockedButton()
	{
		isLocked = true;
	}

	public virtual void OnEnable()
	{
		msg.SetActive(false);
		if (PlayerPrefs.GetInt("DEVMODE") == 1)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (Stats.GetNumWins() >= minWins && (minTournament == -1 || Tournaments.TournamentIsCompleted(minTournament)))
		{
			isLocked = false;
		}
		if (!isLocked && mustHaveFemaleStarterSigned && Players.GetActiveStarterNum(true) == -1)
		{
			isLocked = true;
			msgText.SetTerm("ENTER CHARLOTTE TOURNAMENT", null);
		}
		if (!isLocked && minXP > 0 && Currency.GetXpLevelForXp(minXP) > Currency.GetCurrentXpLevel())
		{
			isLocked = true;
			msgText.SetTerm("LVL LVL REQUIRED", null);
		}
		if (!isLocked)
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void ShowMessage()
	{
		msg.SetActive(true);
		StartCoroutine(HideMessage());
	}

	public virtual IEnumerator HideMessage()
	{
		yield return new WaitForSeconds(1f);
		msg.SetActive(false);
	}

	public virtual void OnModifyMsgTextLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string teamName = TeamDetails.GetTeamName();
			Localize.MainTranslation = Localize.MainTranslation.Replace("{LVL}", Currency.GetXpLevelForXp(minXP) + string.Empty);
		}
	}
}
