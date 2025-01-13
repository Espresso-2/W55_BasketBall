using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SeasonCompletePanel : MonoBehaviour
{
	private enum prizeTypeEnum
	{
		gold = 0,
		standardBag = 1,
		premiumBag = 2
	}

	private GameObject gameSounds;

	public GameObject topNavBar;

	public GameObject[] numBagsIcons;

	public GameObject resultsBox;

	public GameObject prizeBox;

	public GameObject claimButton;

	public Text rankText;

	public Text scoreText;

	public Text prizeText;

	public Text bonusPrizeText;

	public GameObject goldPrizeIcon;

	public GameObject standardBagPrizeIcon;

	public GameObject premiumBagPrizeIcon;

	public GameObject leaderboardPanel;

	private int score;

	private prizeTypeEnum prizeType;

	private int prizeAmt;

	private int bonusPrizeAmt;

	private void Awake()
	{
		gameSounds = GameObject.Find("GameSounds");
	}

	private void OnEnable()
	{
		Debug.Log("SeasonCompletePanel Enabled");
		resultsBox.SetActive(false);
		prizeBox.SetActive(false);
		claimButton.SetActive(false);
		if (leaderboardPanel.activeInHierarchy)
		{
			leaderboardPanel.SetActive(false);
		}
		int @int = PlayerPrefs.GetInt("SEASON_COMPLETED_POS");
		score = PlayerPrefs.GetInt("SEASON_COMPLETED_SCORE");
		int num = @int + 1;
		if (num <= 1)
		{
			prizeType = prizeTypeEnum.gold;
			prizeAmt = 250;
		}
		else if (num <= 10)
		{
			prizeType = prizeTypeEnum.premiumBag;
			prizeAmt = 10;
		}
		else if (num <= 50)
		{
			prizeType = prizeTypeEnum.standardBag;
			prizeAmt = 10;
		}
		else if (num <= 100)
		{
			prizeType = prizeTypeEnum.gold;
			prizeAmt = 20;
		}
		else if (num <= 500)
		{
			prizeType = prizeTypeEnum.gold;
			prizeAmt = 15;
		}
		else
		{
			prizeType = prizeTypeEnum.gold;
			prizeAmt = 1;
		}
		if (num <= 1000)
		{
			bonusPrizeAmt = 10;
		}
		else
		{
			bonusPrizeAmt = 0;
		}
		rankText.text = "#" + num.ToString("n0");
		scoreText.text = "SCORE: " + score.ToString("n0");
		prizeText.text = "X " + prizeAmt.ToString("n0");
		bonusPrizeText.text = "X " + bonusPrizeAmt.ToString("n0");
		goldPrizeIcon.SetActive(prizeType == prizeTypeEnum.gold);
		standardBagPrizeIcon.SetActive(prizeType == prizeTypeEnum.standardBag);
		premiumBagPrizeIcon.SetActive(prizeType == prizeTypeEnum.premiumBag);
		StartCoroutine(ShowToUser());
	}

	private IEnumerator ShowToUser()
	{
		gameSounds.SendMessage("Play_coin_glow");
		gameSounds.SendMessage("Play_trumpet_chime_3");
		resultsBox.SetActive(true);
		yield return new WaitForSeconds(2f);
		prizeBox.SetActive(true);
		gameSounds.SendMessage("Play_coin_glow_2");
		yield return new WaitForSeconds(1f);
		claimButton.SetActive(true);
	}

	public void ClaimOnClick()
	{
		gameSounds.SendMessage("Play_one_dribble");
		gameSounds.SendMessage("Play_dunk");
		if (prizeType == prizeTypeEnum.standardBag)
		{
			PlayerPrefs.SetInt("NUM_STANDARD_BAGS", PlayerPrefs.GetInt("NUM_STANDARD_BAGS") + prizeAmt);
			PlayFabManager.Instance().SetUserDataForKey("NUM_STANDARD_BAGS", PlayerPrefs.GetInt("NUM_STANDARD_BAGS"));
		}
		else if (prizeType == prizeTypeEnum.premiumBag)
		{
			PlayerPrefs.SetInt("NUM_PREMIUM_BAGS", PlayerPrefs.GetInt("NUM_PREMIUM_BAGS") + prizeAmt);
			PlayFabManager.Instance().SetUserDataForKey("NUM_PREMIUM_BAGS", PlayerPrefs.GetInt("NUM_PREMIUM_BAGS"));
		}
		else
		{
			PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD") + prizeAmt);
		}
		PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD") + bonusPrizeAmt);
		PlayFabManager.Instance().SetUserDataForKey("GOLD", PlayerPrefs.GetInt("GOLD"));
		PlayerPrefs.SetInt("SHOW_USER_SEASON_RESULTS", 0);
		gameSounds.SendMessage("Play_dunk");
		topNavBar.SendMessage("UpdateGoldDisplay");
		for (int i = 0; i < numBagsIcons.Length; i++)
		{
			GameObject gameObject = numBagsIcons[i];
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				gameObject.SendMessage("UpdateNum");
			}
		}
		base.gameObject.SetActive(false);
	}
}
