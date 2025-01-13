using UnityEngine;
using UnityEngine.UI;

public class PrizeBallGameResults : MonoBehaviour
{
	public Text scoreAmtText;

	public Text bestScoreText;

	public GameObject newBestBox;

	public GameObject coachRatePrompt;

	private GameObject gameSounds;

	private void Awake()
	{
		gameSounds = GameObject.Find("GameSounds");
	}

	public void SetScore(int score)
	{
		int num = PlayerPrefs.GetInt("NUM_WINS") + 1;
		PlayerPrefs.SetInt("NUM_WINS", num);
		scoreAmtText.text = score.ToString("n0");
		bool flag = false;
		if (SaveNewScore(score))
		{
			gameSounds.SendMessage("Play_trumpet_chime_3");
			gameSounds.SendMessage("Play_crowd_long_cheer_01");
			newBestBox.SetActive(true);
			Handheld.Vibrate();
			if (num >= 5 && PlayerPrefs.GetInt("SHOWED_RATE_PROMPT") == 0)
			{
				flag = true;
			}
		}
		else
		{
			newBestBox.SetActive(false);
			bestScoreText.text = "BEST: " + PlayerPrefs.GetInt("PRIZEBALL_BEST").ToString("n0");
		}
		if (AdMediation.instance != null && AdMediation.instance.GetSecondsSinceIntAd() > (float)PlayerPrefs.GetInt("MIN_SEC_BETWEEN_AD") && AdMediation.IsIntAvail())
		{
			flag = false;
			/*FlurryAnalytics.Instance().LogEvent("SHOW_INT_AD", new string[3]
			{
				"num_wins:" + num + string.Empty,
				"MIN_SEC_BETWEEN_AD:" + PlayerPrefs.GetInt("MIN_SEC_BETWEEN_AD"),
				"num_wins_milestone:" + LoadAppPrizeBall.GetNumWinsMilestone() + string.Empty
			}, false);*/
			AdMediation.ShowInt();
		}
		if (flag)
		{
			coachRatePrompt.SetActive(true);
			PlayerPrefs.SetInt("SHOWED_RATE_PROMPT", 1);
		}
		if (num == 3 || num == 6 || num == 9 || num == 18 || num == 50 || num == 100)
		{
			string eventId = "PLAYED_" + num + "_GAMES";
			/*FlurryAnalytics.Instance().LogEvent(eventId, new string[2]
			{
				"sessions:" + LoadAppPrizeBall.GetNumSessions() + string.Empty,
				"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty
			}, false);*/
		}
	}

	private bool SaveNewScore(int score)
	{
		bool result = false;
		int @int = PlayerPrefs.GetInt("PRIZEBALL_BEST");
		if (score > @int)
		{
			PlayerPrefs.SetInt("PRIZEBALL_BEST", score);
			result = true;
		}
		return result;
	}
}
