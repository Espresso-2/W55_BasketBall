using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameRoster : MonoBehaviour
{
	private int playerInGame;

	public SprintBar sprintBar;

	public Players players;

	public Player starterPlayer;

	public Player benchPlayer;

	private float[] playerHydration;

	public GameObject hydrationBarFill;

	public SpriteRenderer hydrationWarning;

	public TimeoutButton timeoutButton;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private bool isInRed;

	public GameController gameController;

	private bool female;

	public GameRoster()
	{
		playerHydration = new float[2] { 1f, 1f };
	}

	public virtual void OnEnable()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		if (sessionVars.goToPractice)
		{
			female = sessionVars.showingFemales;
		}
		else
		{
			female = sessionVars.currentTournament.isFemale;
		}
		players.InstantiatePlayers();
		starterPlayer = players.GetStarter(female, Players.GetActiveStarterNum(female));
		benchPlayer = players.GetBackup(female, Players.GetActiveBackupNum(female));
	}

	public virtual void Start()
	{
		hydrationWarning.gameObject.SetActive(false);
		timeoutButton.SetIsDehydrated(false);
		if (PlayerPrefs.GetInt("SPRINTBARS_OFF") == 1)
		{
			hydrationWarning.enabled = false;
		}
		StartCoroutine(UpdateBars());
	}

	public virtual int GetPlayerInGame()
	{
		return playerInGame;
	}

	public virtual Player GetPlayerObjectInGame()
	{
		if (playerInGame == 0)
		{
			return starterPlayer;
		}
		return benchPlayer;
	}

	public virtual float GetPlayerHydration(int playerNum)
	{
		if (playerHydration == null || playerHydration.Length < playerNum + 1)
		{
			return 1f;
		}
		return playerHydration[playerNum];
	}

	public virtual float GetCurrentPlayerSize()
	{
		float num = 0f;
		num = ((playerInGame != 0) ? benchPlayer.GetStatByNum(Players.SIZE) : starterPlayer.GetStatByNum(Players.SIZE));
		return GetScaleFromSizeStat((int)num);
	}

	public static float GetScaleFromSizeStat(int sizeStat)
	{
		float num = 0.95f;
		float num2 = 1.05f;
		float num3 = num + (num2 - num) * (float)sizeStat / 50f;
		if (num3 > num2)
		{
			num3 = num2;
		}
		return num3;
	}

	public virtual float GetCurrentPlayerJumpForce()
	{
		return 450f;
	}

	public virtual float GetCurrentShootingArch(float oppConStat)
	{
		float num = 60f;
		float num2 = 90f;
		float stat = GetStat(Players.SHOOTING);
		bool flag = sessionVars.usingPowerups[1] || sessionVars.goToPractice;
		float num3 = stat / (stat + oppConStat);
		if (flag)
		{
			num3 += 0.25f;
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		float num4 = num + (num2 - num) * num3;
		Debug.Log("PLAYER SHOOTING ARC: " + num4);
		return num4;
	}

	public virtual float GetCurrentDefendedMultiplier(float oppDefStat)
	{
		float num = 0.15f;
		float num2 = 1f;
		float stat = GetStat(Players.SPEED);
		bool flag = sessionVars.usingPowerups[0];
		float num3 = stat / (stat + oppDefStat);
		if (flag)
		{
			num3 += 0.25f;
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		float num4 = num + (num2 - num) * num3;
		Debug.Log("PLAYER DEFENSE MULTIPLIER: " + num4);
		return num4;
	}

	public virtual float GetEnergyRegenerationSpeed()
	{
		float result = 1.15f;
		if (sessionVars.usingPowerups[2])
		{
			result = 1.5f;
		}
		return result;
	}

	public virtual void SetPlayerInGame(int player)
	{
		playerInGame = player;
		isInRed = true;
		hydrationWarning.gameObject.SetActive(false);
		timeoutButton.SetIsDehydrated(false);
		isInRed = false;
		StartCoroutine(UpdateBars());
	}

	public virtual void SetStarterPlayerNum(int num)
	{
		starterPlayer = players.GetStarter(female, num);
	}

	public virtual void UsePlayerHydration(float amountUsed)
	{
		playerHydration[playerInGame] -= amountUsed;
		if (playerHydration[playerInGame] < 0.05f)
		{
			playerHydration[playerInGame] = 0.05f;
		}
		StartCoroutine(UpdateBars());
	}

	public virtual bool PlayerHydrationIsFull(int player)
	{
		return playerHydration[player] >= 1f;
	}

	public virtual bool AddPlayerHydration(int player, float amount)
	{
		if (player == playerInGame)
		{
			hydrationWarning.gameObject.SetActive(false);
			timeoutButton.SetIsDehydrated(false);
		}
		if (playerHydration[player] >= 1f)
		{
			return false;
		}
		playerHydration[player] += amount;
		if (playerHydration[player] > 1f)
		{
			playerHydration[player] = 1f;
		}
		StartCoroutine(UpdateBars());
		return true;
	}

	public virtual IEnumerator UpdateBars()
	{
		bool showHydrationWarning = false;
		if (IsLowOnHydration() && !gameController.IsEndOfGame())
		{
			((Image)hydrationBarFill.GetComponent(typeof(Image))).color = Color.red;
			showHydrationWarning = true;
		}
		else
		{
			((Image)hydrationBarFill.GetComponent(typeof(Image))).color = Color.blue;
		}
		hydrationBarFill.transform.localScale = new Vector3(GetPlayerHydration(playerInGame), 1f, 1f);
		if (showHydrationWarning)
		{
			if (!hydrationWarning.gameObject.activeInHierarchy)
			{
				yield return new WaitForSeconds(1.25f);
				if (IsLowOnHydration() && !gameController.gameIsOver)
				{
					hydrationWarning.gameObject.SetActive(true);
					timeoutButton.SetIsDehydrated(true);
					gameSounds.Play_gulp();
				}
			}
		}
		else if (!showHydrationWarning)
		{
			hydrationWarning.gameObject.SetActive(false);
			timeoutButton.SetIsDehydrated(false);
		}
	}

	public virtual bool IsLowOnHydration()
	{
		return GetPlayerHydration(playerInGame) <= 0.05f;
	}

	public virtual void SprintBarInRed(bool isRed)
	{
		isInRed = isRed;
		StartCoroutine(UpdateBars());
	}

	private int GetPlayerOnBench()
	{
		if (playerInGame == 1)
		{
			return 0;
		}
		return 1;
	}

	public virtual float GetStat(int statNum)
	{
		float num = 0f;
		if (playerInGame == 0)
		{
			return starterPlayer.GetStatByNum(statNum);
		}
		return benchPlayer.GetStatByNum(statNum);
	}

	public virtual void Update()
	{
	}
}
