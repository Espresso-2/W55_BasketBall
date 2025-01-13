using UnityEngine;

public class LoadAppPrizeBall : MonoBehaviour
{
	public GameObject hintMsg;

	private void Start()
	{
		Application.targetFrameRate = 60;
		int num = GetNumSessions() + 1;
		SetNumSessions(num);
		if (num == 1)
		{
			hintMsg.gameObject.SetActive(false);
			SetupSplitTests();
			//FlurryAnalytics.Instance().LogEvent("FIRST_APP_LAUNCH");
		}
		else
		{
			string text = string.Empty;
			switch (num)
			{
			case 5:
				text = "session_05";
				break;
			case 10:
				text = "session_10";
				break;
			case 20:
				text = "session_20";
				break;
			case 50:
				text = "session_50";
				break;
			case 100:
				text = "session_100";
				break;
			}
			if (text != string.Empty)
			{
				/*FlurryAnalytics.Instance().LogEvent(text, new string[4]
				{
					"int_ads:" + PlayerPrefs.GetInt("NUM_INT_ADS"),
					"video_ads:" + PlayerPrefs.GetInt("NUM_VIDEO_ADS"),
					"wins:" + PlayerPrefs.GetInt("NUM_WINS") + string.Empty,
					"num_wins_milestone:" + GetNumWinsMilestone()
				}, false);*/
			}
		}
		if (PlayerPrefs.GetInt("SOUND_OFF") == 1)
		{
			AudioListener.volume = 0f;
		}
		else
		{
			AudioListener.volume = 10f;
		}
		Application.LoadLevel("PrizeBall");
	}

	public static int GetNumSessions()
	{
		return PlayerPrefs.GetInt("NUM_SESSIONS");
	}

	private static void SetNumSessions(int num)
	{
		PlayerPrefs.SetInt("NUM_SESSIONS", num);
	}

	public static int GetNumWinsMilestone()
	{
		int result = 0;
		int @int = PlayerPrefs.GetInt("NUM_WINS");
		if (@int >= 1000)
		{
			result = 1000;
		}
		else if (@int >= 200)
		{
			result = 200;
		}
		else if (@int >= 100)
		{
			result = 100;
		}
		else if (@int >= 50)
		{
			result = 50;
		}
		else if (@int >= 20)
		{
			result = 20;
		}
		else if (@int >= 10)
		{
			result = 10;
		}
		else if (@int >= 5)
		{
			result = 5;
		}
		else if (@int >= 1)
		{
			result = 1;
		}
		return result;
	}

	private void SetupSplitTests()
	{
		int num = Random.Range(0, 100);
		int num2 = ((num >= 75) ? 120 : ((num >= 50) ? 90 : ((num < 25) ? 30 : 60)));
		PlayerPrefs.SetInt("MIN_SEC_BETWEEN_AD", num2);
		//FlurryAnalytics.Instance().LogEvent("MIN_SEC_BETWEEN_AD_" + num2, new string[1] { "asdf:" + 1 }, false);
	}
}
