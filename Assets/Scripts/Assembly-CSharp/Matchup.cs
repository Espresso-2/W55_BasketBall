using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Matchup : MonoBehaviour
{
	public Tournaments tournaments;

	public Players players;

	private Tournament tournament;

	public Localize tournamentName;

	public GameObject tWomenIcon;

	public GameObject tLiveEventIcon;

	public Image headingBoxImage1;

	public Image headingBoxImage2;

	public Color lightBlueColor;

	public Color redColor;

	public Text player1Name;

	public Text player1Record;

	public Text player2Name;

	public Text player2Record;

	public GameObject[] player1StatBars;

	public GameObject[] player2StatBars;

	public Text[] player1StatVal;

	public Text[] player2StatVal;

	public GameObject tipBox;

	public Localize tipText;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private Music music;

	private float barHighlightedTime;

	private bool isLiveEvent;

	public Matchup()
	{
		barHighlightedTime = 1E+11f;
	}

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		players.InstantiatePlayers();
	}

	public virtual void Start()
	{
		GameObject gameObject = GameObject.Find("Music");
		if (gameObject != null)
		{
			music = (Music)gameObject.GetComponent(typeof(Music));
		}
		tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		isLiveEvent = tournament.type == tournamentTypeEnum.LiveEvent;
		sessionVars.currentTournament = tournament;
		tournamentName.SetTerm(tournament.GetCurrentRoundName(), null);
		tWomenIcon.SetActive(tournament.isFemale);
		tLiveEventIcon.SetActive(isLiveEvent);
		headingBoxImage1.color = ((!isLiveEvent) ? lightBlueColor : redColor);
		headingBoxImage2.color = ((!isLiveEvent) ? lightBlueColor : redColor);
		player1Name.text = TeamDetails.GetTeamName();
		player1Record.text = ((!isLiveEvent) ? (Stats.GetNumWins() + "-" + Stats.GetNumLosses() + string.Empty) : string.Empty);
		player2Name.text = tournament.GetCurrentOpponentName();
		player2Record.text = ((!isLiveEvent) ? tournament.GetCurrentOpponentRecord() : string.Empty);
		if (!PlayFabManager.Instance().IsClientLoggedIn())
		{
			PlayFabManager.Instance().LoginAsGuest(true);
		}
		Player.playerNamesMayHaveChanged = false;
		AdMediation.IsVidAvail();
		int num = PlayerPrefs.GetInt("GAMES_SINCE_WATCHING_AD") + 1;
		PlayerPrefsHelper.SetInt("GAMES_SINCE_WATCHING_AD", num, true);
		if (PlayerPrefs.GetInt("NUM_PURCHASES") == 0 || PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 1)
		{
			if (PlayerPrefs.GetInt("NATIVE_HALFTIME_ADS_ENABLED") == 1)
			{
				AdMediation.RequestNativeAd();
			}
			else
			{
				AdMediation.RequestCenterBanner();
			}
			if (PlayerPrefs.GetInt("ADS_OFF") != 1 && num >= 2 && !AdMediation.IsIntAvail())
			{
				AdMediation.ReqInt();
			}
		}
		PlayFabManager.Instance().SetUserDataForKey("GAMES_SINCE_WATCHING_AD", num);
		AdMediation.ShowTjpMatchupScreen();
	}

	public virtual void StartMatch()
	{
		for (int i = 0; i < sessionVars.usingPowerups.Length; i++)
		{
			if (sessionVars.usingPowerups[i] && !Supplies.UseItem(Supplies.GetSupplyByPowerupNum(i), 1))
			{
				sessionVars.usingPowerups[i] = false;
			}
		}
		gameSounds.Play_ascend_chime_low();
		gameSounds.Play_one_dribble();
		if (music != null)
		{
			music.FadeOutMusic();
		}
		TabChanger.currentTabNum = tabEnum.Tour;
		tipBox.SetActive(true);
		if (isLiveEvent)
		{
			tipText.SetTerm("ENTERING LIVE EVENT!", null);
		}
		else
		{
			tipText.SetTerm("FIRST TO 12 POINTS", null);
		}
		Application.LoadLevel("GamePlay");
	}

	public virtual void ShowComparison()
	{
		if (tournament == null)
		{
			tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		}
		bool flag = PlayerPrefs.GetInt("DEVMODE") == 1;
		Player starter = players.GetStarter(tournament.isFemale, Players.GetActiveStarterNum(tournament.isFemale));
		Player backup = players.GetBackup(tournament.isFemale, Players.GetActiveBackupNum(tournament.isFemale));
		bool flag2 = sessionVars.usingPowerups[1];
		bool flag3 = sessionVars.usingPowerups[0];
		bool flag4 = sessionVars.usingPowerups[2];
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 1; i < player1StatBars.Length; i++)
		{
			float num5 = starter.GetStatByNum(i) + backup.GetStatByNum(i);
			bool flag5 = false;
			if (i == Players.SHOOTING && flag2)
			{
				flag5 = true;
			}
			if (i == Players.SPEED && flag3)
			{
				flag5 = true;
			}
			string text = num5.ToString();
			if (flag)
			{
				player1StatVal[i].text = text;
			}
			float num6 = GameComputer.GetStatByNum(i, true, tournament.num, tournament.currentRound) * 2f;
			if (num6 < 10f)
			{
				num6 += 15.3f;
			}
			else if (num6 < 15f)
			{
				num6 += 10.2f;
			}
			else if (num6 < 20f)
			{
				num6 += 5.1f;
			}
			num6 = Mathf.Round(num6);
			if (flag)
			{
				player2StatVal[i].text = num6.ToString();
			}
			num += num5;
			num2 += num6;
			float num7 = num5 / (num5 + num6);
			float num8 = num6 / (num5 + num6);
			if (flag5)
			{
				num7 += 0.25f;
				num8 -= 0.25f;
				if (num8 < 0f)
				{
					num7 = 0.98f;
					num8 = 0.02f;
				}
			}
			float x = player2StatBars[i].transform.localScale.x;
			if (x < 0.95f && Mathf.Abs(x - num8) > 0.1f)
			{
				HighlightChangingStatBar(player1StatBars[i]);
			}
			LeanTween.scale(player2StatBars[i], new Vector3(num8, 1f, 1f), 0.65f).setEase(LeanTweenType.easeOutExpo);
			num3 += num7;
			num4 += num8;
		}
		if (flag4)
		{
			num3 += 0.4f;
			num4 -= 0.4f;
		}
		float num9 = num3 / (num3 + num4);
		float num10 = num4 / (num3 + num4);
		if (num9 > 1f)
		{
			num9 = 0.99f;
		}
		if (num10 < 0f)
		{
			num10 = 0.01f;
		}
		if (player2StatBars[0].transform.localScale.x < 0.95f)
		{
			HighlightChangingStatBar(player1StatBars[0]);
		}
		LeanTween.scale(player2StatBars[0], new Vector3(num10, 1f, 1f), 0.65f).setEase(LeanTweenType.easeOutExpo);
		if (flag)
		{
			if (flag4)
			{
				player1StatVal[0].text = num + string.Empty;
			}
			else
			{
				player1StatVal[0].text = num.ToString();
			}
			player2StatVal[0].text = num2.ToString();
		}
		else if (flag4)
		{
			player1StatVal[0].text = string.Empty;
		}
		else
		{
			player1StatVal[0].text = string.Empty;
		}
	}

	private void HighlightChangingStatBar(GameObject bar)
	{
		Image image = (Image)bar.GetComponent("Image");
		image.color = new Color(1f, 0.658f, 0.505f, 1f);
		barHighlightedTime = 0.65f;
	}

	public virtual void OnModifyTouramentNameLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string text = tournament.name + ": ";
			Localize.MainTranslation = text + Localize.MainTranslation;
		}
	}

	public virtual void FixedUpdate()
	{
		if (barHighlightedTime < 0f)
		{
			for (int i = 0; i < player1StatBars.Length; i++)
			{
				Image image = (Image)player1StatBars[i].GetComponent("Image");
				image.color = new Color(1f, 0.376f, 0.0941f, 1f);
			}
			barHighlightedTime = 1E+09f;
		}
		else
		{
			barHighlightedTime -= Time.deltaTime;
		}
	}

	public virtual void Update()
	{
	}
}
