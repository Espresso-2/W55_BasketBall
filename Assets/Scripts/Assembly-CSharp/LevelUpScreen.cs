using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LevelUpScreen : MonoBehaviour
{
	public GameObject rewards;

	public Text prevLevel;

	public Text newLevel;

	public GameObject oneItemPanel;

	public GameObject oneItemPrizeBallPanel;

	public GameObject twoItemPanel;

	public Text goldAmount;

	public Text goldAmount2;

	public Text prizeBallsAmount;

	public Text cashAmount;

	public GameObject continueButton;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	public virtual IEnumerator Start()
	{
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
		newLevel.gameObject.SetActive(false);
		rewards.SetActive(false);
		continueButton.SetActive(false);
		int xpLevel = Currency.GetCurrentXpLevel();
		int gold = Currency.GetCurrentXpLevelGoldReward();
		int cash = Currency.GetCurrentXpLevelCashReward();
		int prizeBalls = Currency.GetCurrentXpLevelPrizeBallsReward();
		if (prizeBalls > 0)
		{
			oneItemPanel.SetActive(false);
			oneItemPrizeBallPanel.SetActive(true);
			twoItemPanel.SetActive(false);
			prizeBallsAmount.text = prizeBalls.ToString("n0");
		}
		else if (cash > 0)
		{
			oneItemPanel.SetActive(false);
			oneItemPrizeBallPanel.SetActive(false);
			twoItemPanel.SetActive(true);
			goldAmount2.text = gold.ToString("n0");
			cashAmount.text = cash.ToString("n0");
		}
		else
		{
			oneItemPanel.SetActive(true);
			oneItemPrizeBallPanel.SetActive(false);
			twoItemPanel.SetActive(false);
			goldAmount.text = gold.ToString("n0");
		}
		prevLevel.text = (xpLevel - 1).ToString();
		newLevel.text = xpLevel.ToString();
		/*FlurryAnalytics.Instance().LogEvent("LEVEL_UP", new string[7]
		{
			"new_level:" + xpLevel + string.Empty,
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"gold:" + Currency.GetCurrentGold() + string.Empty,
			"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES")
		}, false);*/
		gameSounds.Play_trumpet_chime_3();
		yield return new WaitForSeconds(1f);
		gameSounds.Play_ascend_chime_bright();
		prevLevel.gameObject.SetActive(false);
		newLevel.gameObject.SetActive(true);
		LeanTween.scale(newLevel.gameObject, new Vector3(2.5f, 2.5f, 1f), 3.5f).setEase(LeanTweenType.easeOutExpo);
		yield return new WaitForSeconds(1f);
		gameSounds.Play_dunk();
		newLevel.gameObject.SetActive(false);
		rewards.SetActive(true);
		LeanTween.scale(rewards, new Vector3(1.2f, 1.2f, 1f), 0.5f).setEase(LeanTweenType.easeOutExpo);
		continueButton.SetActive(true);
	}

	public virtual void ContinueOnClick()
	{
		gameSounds.Play_one_dribble();
		gameSounds.Play_dunk();
		if (Currency.GetNumPrizeBalls() > 0)
		{
			Application.LoadLevel("PrizeBall");
		}
		else
		{
			Application.LoadLevel("MainUI");
		}
	}
}
