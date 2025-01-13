using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StatsScreen : MonoBehaviour
{
	public Text gamesNum;

	public Text winRate;

	public Text wins;

	public Text losses;

	public Text avgPts;

	public Text totPts;

	public Text avgReb;

	public Text totReb;

	public Text avgBlk;

	public Text totBlk;

	public Text avgSho;

	public Text totSho;

	public Text avg3PtSho;

	public Text tot3PtSho;

	public Text avgSte;

	public Text totSte;

	public Text avgSec;

	public Text totSec;

	public Text curStreak;

	public Text bestStreak;

	public Text numTrophies;

	public Text numChampionships;

	public Text playFabId;

	public virtual void Start()
	{
		/*FlurryAnalytics.Instance().LogEvent("SCREEN_STATS", new string[4]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
	}

	public virtual void OnEnable()
	{
		int num = Stats.GetNumWins() + Stats.GetNumLosses();
		gamesNum.text = string.Empty + num;
		if (num > 0)
		{
			float num2 = (float)Stats.GetNumWins() * 1f / ((float)num * 1f);
			float num3 = Mathf.Round(num2 * 100f) / 100f;
			string text = Regex.Replace(num3 + string.Empty, "0.", ".");
			while (text.Length < 4)
			{
				text += "0";
			}
			winRate.text = text;
		}
		else
		{
			winRate.text = "0.000";
		}
		wins.text = string.Empty + Stats.GetNumWins();
		losses.text = string.Empty + Stats.GetNumLosses();
		curStreak.text = Stats.GetWinStreak().ToString("n0") + ((Stats.GetWinStreak() != 1) ? " WINS" : " WIN");
		bestStreak.text = Stats.GetBestWinStreak().ToString("n0") + ((Stats.GetBestWinStreak() != 1) ? " WINS" : " WIN");
		avgPts.text = GetAverage(Stats.GetNumPoints(), num).ToString("n1");
		totPts.text = Stats.GetNumPoints() + string.Empty;
		avgReb.text = GetAverage(Stats.GetNumRebounds(), num).ToString("n1");
		totReb.text = Stats.GetNumRebounds() + string.Empty;
		avgBlk.text = GetAverage(Stats.GetNumBlocks(), num).ToString("n1");
		totBlk.text = Stats.GetNumBlocks() + string.Empty;
		avgSho.text = GetAverage(Stats.GetNumShots(), num).ToString("n1");
		totSho.text = Stats.GetNumShots() + string.Empty;
		if (num > 0)
		{
			avgSho.text = Mathf.Round((float)Stats.GetNumMakes() * 1f / (float)Stats.GetNumShots() * 1f * 100f) + "%";
		}
		else
		{
			avgSho.text = "0%";
		}
		totSho.text = Stats.GetNumMakes() + "/" + Stats.GetNumShots();
		avg3PtSho.text = GetAverage(Stats.GetNum3PtShots(), num).ToString("n1");
		tot3PtSho.text = Stats.GetNum3PtShots() + string.Empty;
		if (num > 0 && Stats.GetNum3PtShots() > 0)
		{
			avg3PtSho.text = Mathf.Round((float)Stats.GetNum3PtMakes() * 1f / (float)Stats.GetNum3PtShots() * 1f * 100f) + "%";
		}
		else
		{
			avg3PtSho.text = "0%";
		}
		tot3PtSho.text = Stats.GetNum3PtMakes() + "/" + Stats.GetNum3PtShots();
		avgSte.text = GetAverage(Stats.GetNumSteals(), num).ToString("n1");
		totSte.text = Stats.GetNumSteals() + string.Empty;
		avgSec.text = DoubleTapUtils.GetTimeFromSeconds((int)GetAverage(Stats.GetNumSeconds(), PlayerPrefs.GetInt("NUM_GAME_SECONDS_RECORDED"))) + string.Empty;
		totSec.text = DoubleTapUtils.GetTimeFromSeconds(Stats.GetNumSeconds()) + string.Empty;
		numTrophies.text = Tournaments.numTrophies.ToString("n0");
		numChampionships.text = Tournaments.numChampionships.ToString("n0");
		playFabId.text = "#" + PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
	}

	private float GetAverage(float statTotal, float numGames)
	{
		if (numGames <= 0f)
		{
			return 0f;
		}
		return Mathf.Round(statTotal * 10f / (numGames * 10f) * 10f) / 10f;
	}
}
