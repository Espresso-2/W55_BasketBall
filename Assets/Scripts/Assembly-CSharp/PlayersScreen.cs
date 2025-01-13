using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayersScreen : MonoBehaviour
{
	public Image[] typeButtonImages;

	public Text[] playerNames;

	public Text[] starValues;

	public Text[] bballValues;

	public GameObject[] playerTabs;

	public HeadVisual[] headVisuals;

	public Image[] playerPictures;

	public GameObject[] playerStatuses;

	public GameObject[] playerDeactiveStatuses;

	public PlayerDetails playerDetails;

	public Players players;

	private CharacterSprites characterSprites;

	private int selectedNum;

	public Sprite unselectedTypeGraphic;

	public Color unselectedTypeColor;

	public Sprite selectedTypeGraphic;

	public Color selectedTypeColor;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private bool tonyNoelIsPurchased;

	public GameObject[] hintArrows;

	public virtual void Awake()
	{
		characterSprites = (CharacterSprites)GameObject.Find("CharacterSprites").GetComponent(typeof(CharacterSprites));
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		players.InstantiatePlayers();
	}

	public virtual void Start()
	{
	}

	public virtual void OnEnable()
	{
		tonyNoelIsPurchased = Players.IsStarterPurchased(false, 0);
		if (sessionVars.showingBackups)
		{
			if (sessionVars.showingFemales)
			{
				SelectPlayer(sessionVars.selectedBackupFemaleNum);
			}
			else
			{
				SelectPlayer(sessionVars.selectedBackupNum);
			}
			return;
		}
		if (!tonyNoelIsPurchased)
		{
			sessionVars.selectedStarterNum = 0;
			ShowHintArrow(0);
		}
		if (sessionVars.showingFemales)
		{
			SelectPlayer(sessionVars.selectedStarterFemaleNum);
		}
		else
		{
			SelectPlayer(sessionVars.selectedStarterNum);
		}
	}

	public virtual void SelectPlayer(int num)
	{
		bool showingFemales = sessionVars.showingFemales;
		selectedNum = num;
		Image[] array = typeButtonImages;
		foreach (Image image in array)
		{
			image.sprite = unselectedTypeGraphic;
			image.color = unselectedTypeColor;
		}
		List<Player> list = null;
		int num2 = 0;
		if (sessionVars.showingBackups)
		{
			list = players.GetBackups(showingFemales);
			if (showingFemales)
			{
				sessionVars.selectedBackupFemaleNum = num;
			}
			else
			{
				sessionVars.selectedBackupNum = num;
			}
			num2 = Players.GetActiveBackupNum(showingFemales);
		}
		else
		{
			list = players.GetStarters(showingFemales);
			if (showingFemales)
			{
				sessionVars.selectedStarterFemaleNum = num;
			}
			else
			{
				sessionVars.selectedStarterNum = num;
			}
			num2 = Players.GetActiveStarterNum(showingFemales);
		}
		int num3 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			Player player = list[j];
			playerTabs[num3].SetActive(true);
			num3++;
			if (player.num == num2)
			{
				playerStatuses[player.num].SetActive(true);
				playerDeactiveStatuses[player.num].SetActive(false);
			}
			else
			{
				playerStatuses[player.num].SetActive(false);
				playerDeactiveStatuses[player.num].SetActive(player.IsOwned());
			}
			headVisuals[player.num].SetVisual(player, characterSprites, 0);
			if (Player.playerNamesMayHaveChanged)
			{
				string fullNameKey = player.fullNameKey;
				player.fullName = ((!(PlayerPrefs.GetString(fullNameKey) != string.Empty)) ? fullNameKey : PlayerPrefs.GetString(fullNameKey));
			}
			playerNames[player.num].text = player.fullName;
			starValues[player.num].text = "X " + player.GetStarValue();
			bballValues[player.num].text = player.GetStatTotal().ToString();
		}
		typeButtonImages[num].sprite = selectedTypeGraphic;
		typeButtonImages[num].color = selectedTypeColor;
		playerDetails.showingBackups = sessionVars.showingBackups;
		playerDetails.SetPlayerAndGender(selectedNum, showingFemales);
	}

	public virtual void BuyPlayer()
	{
		if (playerDetails.BuyPlayer())
		{
			gameSounds.Play_swoosh();
			gameSounds.Play_coin_glow_2();
			SelectPlayer(selectedNum);
			if (!tonyNoelIsPurchased)
			{
				ShowHintArrow(1);
			}
		}
	}

	public virtual void PlaySelectSound()
	{
		gameSounds.Play_select();
	}

	public virtual void ActivatePlayer()
	{
		gameSounds.Play_air_pump();
		if (sessionVars.showingBackups)
		{
			Players.SetActiveBackupNum(sessionVars.showingFemales, selectedNum);
		}
		else
		{
			Players.SetActiveStarterNum(sessionVars.showingFemales, selectedNum);
		}
		SelectPlayer(selectedNum);
	}

	private void ShowHintArrow(int num)
	{
		for (int i = 0; i < hintArrows.Length; i++)
		{
			hintArrows[i].SetActive(i == num);
		}
	}

	public virtual void Update()
	{
	}
}
