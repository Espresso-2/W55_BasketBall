using System;
using UnityEngine;

[Serializable]
public class GameComputer : MonoBehaviour
{
	public Tournaments tournaments;

	public SpriteRenderer hydrationWarning;

	private Tournament tournament;

	private bool inScrimmage;

	private int computerOffenseLevel = 1;

	private int computerDefenseLevel = 1;

	private bool computerCanWin;

	private bool computerNoWayPhysicallyPossibleToWin;

	private bool computerShouldWin;

	private bool defeatJoeOffense;

	private bool defeatJoeDefense;

	private float xMovementChangeUpdateTimerResetValue;

	private float maxSecondsStuckDribblingIntoDefender;

	private bool hasSetAttributes;

	private int chanceOfGoingForPumpFake = 50;

	private bool isDehydrated;

	private bool neverShoot;

	private int currentHalf = 1;

	public virtual void Start()
	{
		hydrationWarning.gameObject.SetActive(false);
		if (PlayerPrefs.GetInt("SPRINTBARS_OFF") == 1)
		{
			hydrationWarning.enabled = false;
		}
	}

	public virtual int GetComputerNum()
	{
		if (tournament != null)
		{
			if (tournament.type == tournamentTypeEnum.LiveEvent)
			{
				return PlayerPrefs.GetInt("LB_VERSION");
			}
			return tournament.num * 3 + tournament.currentRound;
		}
		return 0;
	}

	public virtual void SetComputerAttributes(int p1Score, int cpuScore, int playToScore, bool inScrimmage, int currentHalf)
	{
		if (!hasSetAttributes)
		{
			this.inScrimmage = inScrimmage;
			tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
			int num = 4;
			int num2 = 13;
			if (tournament.num >= 2 && !this.inScrimmage && tournament.num != num && tournament.num != num2)
			{
				int num3 = UnityEngine.Random.Range(0, 100);
				if (num3 >= 80 || tournament.num >= 40)
				{
					computerShouldWin = true;
					computerCanWin = true;
				}
				else if (num3 >= 20 || tournament.num >= 5)
				{
					computerShouldWin = false;
					computerCanWin = true;
				}
				else
				{
					computerShouldWin = false;
					computerCanWin = false;
				}
				if (UnityEngine.Random.Range(0, 100) >= 50 || tournament.num >= 25)
				{
					chanceOfGoingForPumpFake = 30;
				}
				else
				{
					chanceOfGoingForPumpFake = 60;
				}
			}
			else
			{
				computerShouldWin = false;
				computerCanWin = false;
				computerNoWayPhysicallyPossibleToWin = true;
				chanceOfGoingForPumpFake = 100;
			}
			int numCompletions = Tournaments.GetNumCompletions(Tournaments.GetCurrentTournamentNum());
			if (numCompletions >= 2)
			{
				computerShouldWin = true;
				computerCanWin = true;
				computerNoWayPhysicallyPossibleToWin = false;
				chanceOfGoingForPumpFake = 10;
			}
			else if (numCompletions >= 1)
			{
				computerCanWin = true;
				computerNoWayPhysicallyPossibleToWin = false;
				chanceOfGoingForPumpFake = 30;
			}
			if (tournament.currentRound == 1)
			{
				xMovementChangeUpdateTimerResetValue = -0.15f;
			}
			else if (tournament.currentRound == 2)
			{
				xMovementChangeUpdateTimerResetValue = 0.14f;
			}
			else
			{
				xMovementChangeUpdateTimerResetValue = -0.05f;
			}
			hasSetAttributes = true;
		}
		neverShoot = false;
		if (inScrimmage && p1Score + 3 >= playToScore)
		{
			Debug.Log("TURN ON neverShoot to FORCE USER TO LEARN HOW TO STEAL");
			neverShoot = true;
		}
		int num4 = p1Score - cpuScore;
		int num5 = tournament.num * tournament.currentRound;
		int num6 = tournament.num * tournament.currentRound;
		if (tournament.type == tournamentTypeEnum.LiveEvent)
		{
			num5 = 1;
			num6 = 1;
		}
		if (num4 >= 3)
		{
			num5 += num4 * 20;
			if (p1Score + 2 >= playToScore)
			{
				num5 += 20;
			}
			num6 += num4;
		}
		if (num4 < 0)
		{
			num5 += num4 * 10;
			num6 += num4 * 20;
		}
		if (this.currentHalf != currentHalf)
		{
			isDehydrated = false;
			this.currentHalf = currentHalf;
		}
		if (computerShouldWin)
		{
			if (p1Score + 9 >= playToScore)
			{
				num5 += 80;
				num6 += 80;
			}
			if (cpuScore >= 10 && UnityEngine.Random.Range(0, 100) >= 75)
			{
				isDehydrated = true;
			}
		}
		else if (cpuScore >= 8 && UnityEngine.Random.Range(0, 100) >= 50)
		{
			isDehydrated = true;
		}
		else if (currentHalf == 1 && cpuScore >= 4 && UnityEngine.Random.Range(0, 100) >= 50)
		{
			isDehydrated = true;
		}
		if (num5 > 100)
		{
			num5 = 100;
		}
		if (num6 > 100)
		{
			num6 = 100;
		}
		if (num5 < 1)
		{
			num5 = 1;
		}
		if (num6 < 1)
		{
			num6 = 1;
		}
		defeatJoeOffense = ShouldTurnOnDefeatJoeOffense();
		if (defeatJoeOffense)
		{
			computerCanWin = true;
			num5 = 100;
		}
		defeatJoeDefense = ShouldTurnOnDefeatJoeDefense();
		if (defeatJoeDefense)
		{
			computerCanWin = true;
			num6 = 100;
			chanceOfGoingForPumpFake = 6;
		}
		computerOffenseLevel = num5;
		computerDefenseLevel = num6;
		hydrationWarning.gameObject.SetActive(isDehydrated);
		if (Tournaments.TournamentIsCompleted(0))
		{
			if (currentHalf == 1)
			{
				if (tournament.currentRound == 1)
				{
					maxSecondsStuckDribblingIntoDefender = 0.5f;
				}
				else if (tournament.currentRound == 2)
				{
					maxSecondsStuckDribblingIntoDefender = 3f;
				}
				else
				{
					maxSecondsStuckDribblingIntoDefender = 1f;
				}
			}
			else if (tournament.currentRound == 2)
			{
				maxSecondsStuckDribblingIntoDefender = 0.1f;
			}
			else
			{
				maxSecondsStuckDribblingIntoDefender = 3f;
			}
		}
		else
		{
			maxSecondsStuckDribblingIntoDefender = 6f;
		}
		if (tournament.type == tournamentTypeEnum.LiveEvent)
		{
			SetForLiveEvent(p1Score);
		}
	}

	private void SetForLiveEvent(int p1Score)
	{
		if (p1Score >= 60)
		{
			computerNoWayPhysicallyPossibleToWin = false;
			computerCanWin = true;
			computerShouldWin = true;
			maxSecondsStuckDribblingIntoDefender = 0.1f;
			computerOffenseLevel = 100;
			computerDefenseLevel = 100;
			defeatJoeOffense = true;
			defeatJoeDefense = true;
			chanceOfGoingForPumpFake = 5;
			xMovementChangeUpdateTimerResetValue = 0.08f;
		}
		else if (p1Score >= 30)
		{
			computerNoWayPhysicallyPossibleToWin = false;
			computerCanWin = true;
			computerShouldWin = true;
			maxSecondsStuckDribblingIntoDefender = 1f;
			computerOffenseLevel = 90;
			computerDefenseLevel = 90;
			defeatJoeOffense = true;
			defeatJoeDefense = false;
			chanceOfGoingForPumpFake = 10;
			xMovementChangeUpdateTimerResetValue = 0.15f;
		}
		else if (p1Score >= 15)
		{
			computerNoWayPhysicallyPossibleToWin = false;
			computerCanWin = true;
			computerShouldWin = false;
			maxSecondsStuckDribblingIntoDefender = 2f;
			computerOffenseLevel = 30;
			computerDefenseLevel = 30;
			defeatJoeOffense = false;
			defeatJoeDefense = false;
			chanceOfGoingForPumpFake = 50;
			xMovementChangeUpdateTimerResetValue = -0.15f;
		}
		else
		{
			computerNoWayPhysicallyPossibleToWin = true;
			computerCanWin = false;
			computerShouldWin = false;
			maxSecondsStuckDribblingIntoDefender = 6f;
			computerOffenseLevel = 0;
			computerDefenseLevel = 0;
			defeatJoeOffense = false;
			defeatJoeDefense = false;
			chanceOfGoingForPumpFake = 100;
			xMovementChangeUpdateTimerResetValue = -0.25f;
		}
	}

	private bool ShouldTurnOnDefeatJoeOffense()
	{
		bool result = false;
		if (tournament.num >= 100)
		{
			result = true;
		}
		else if (tournament.num >= 75 && currentHalf == 1)
		{
			result = true;
		}
		else if (tournament.num >= 50 && currentHalf == 2 && tournament.currentRound == 2)
		{
			result = true;
		}
		else if (tournament.num >= 25 && currentHalf == 2 && tournament.currentRound == 1)
		{
			result = true;
		}
		else if (tournament.num >= 8 && currentHalf == 1 && tournament.currentRound == 3)
		{
			result = true;
		}
		else if (tournament.num >= 2 && currentHalf == 1 && tournament.currentRound == 2)
		{
			result = true;
		}
		if (UnityEngine.Random.Range(0, 100) >= 95)
		{
			result = false;
		}
		return result;
	}

	private bool ShouldTurnOnDefeatJoeDefense()
	{
		bool result = false;
		if (tournament.num >= 120)
		{
			result = true;
		}
		else if (tournament.num >= 90 && (tournament.currentRound == 2 || currentHalf == 2))
		{
			result = true;
		}
		else if (tournament.num >= 60 && tournament.currentRound == 1 && currentHalf == 2)
		{
			result = true;
		}
		else if (tournament.num >= 30 && tournament.currentRound == 3 && currentHalf == 1)
		{
			result = true;
		}
		else if (tournament.num >= 15 && tournament.currentRound == 2 && currentHalf == 1)
		{
			result = true;
		}
		else if (tournament.num >= 6 && tournament.currentRound == 1 && currentHalf == 1)
		{
			result = true;
		}
		if (UnityEngine.Random.Range(0, 100) >= 90)
		{
			result = false;
		}
		return result;
	}

	public virtual int GetOffenseLevel()
	{
		return computerOffenseLevel;
	}

	public virtual int GetDefenseLevel()
	{
		return computerDefenseLevel;
	}

	public virtual bool CanWin()
	{
		return computerCanWin;
	}

	public virtual bool NoWayPhysicallyPossibleToWin()
	{
		return computerNoWayPhysicallyPossibleToWin;
	}

	public virtual bool IsDefeatJoeOffense()
	{
		return defeatJoeOffense;
	}

	public virtual bool IsDefeatJoeDefense()
	{
		return defeatJoeDefense;
	}

	public virtual bool ShouldWin()
	{
		return computerShouldWin;
	}

	public virtual float XMovementChangeUpdateTimerResetValue()
	{
		return xMovementChangeUpdateTimerResetValue;
	}

	public virtual float MaxSecondsStuckDribblingIntoDefender()
	{
		return maxSecondsStuckDribblingIntoDefender;
	}

	public virtual float GetCurrentPlayerSize(tournamentTypeEnum type, int num, int round, int currentHalf)
	{
		int num2 = 0;
		float num3 = 0.9f;
		float num4 = 1.15f;
		float statByNum = GetStatByNum(Players.SIZE, false, num, round);
		Debug.Log("round: " + round + " currentHalf: " + currentHalf);
		switch (round)
		{
		case 0:
		case 1:
			num2 = -15;
			break;
		case 2:
			num2 = 20;
			break;
		case 3:
			num2 = -10;
			break;
		}
		if (currentHalf == 2)
		{
			num2 *= -1;
		}
		statByNum += (float)num2;
		if (currentHalf == 1 && type == tournamentTypeEnum.LiveEvent)
		{
			statByNum = 15f;
		}
		if (statByNum < 1f)
		{
			statByNum = 1f;
		}
		float num5 = num3 + (num4 - num3) * statByNum / 50f;
		if (num5 > num4)
		{
			num5 = num4;
		}
		Debug.Log("COMPUTER SIZE: " + num5 + " (sizeStat: " + statByNum + ")");
		return num5;
	}

	public virtual float GetCurrentPlayerJumpForce()
	{
		return 450f;
	}

	public virtual float GetEnergyRegenerationSpeed()
	{
		float result = 1.15f;
		if (inScrimmage)
		{
			result = 1.01f;
		}
		else if (computerShouldWin)
		{
			result = 1.5f;
		}
		return result;
	}

	public virtual float GetCurrentDefendedMultiplier(float oppDefStat, int currentHalf)
	{
		int num = 0;
		float num2 = 0.15f;
		float num3 = 1f;
		int currentTournamentNum = Tournaments.GetCurrentTournamentNum();
		int currentRound = Tournaments.GetCurrentRound(currentTournamentNum);
		switch (currentRound)
		{
		case 0:
		case 1:
			num = 15;
			break;
		case 2:
			num = -20;
			break;
		case 3:
			num = 10;
			break;
		}
		if (currentHalf == 2)
		{
			num *= -1;
		}
		float statByNum = GetStatByNum(Players.SPEED, false, currentTournamentNum, currentRound);
		statByNum += (float)num;
		if (statByNum < 1f)
		{
			statByNum = 1f;
		}
		float num4 = statByNum / (statByNum + oppDefStat);
		Debug.Log("COMPUTER DEFENSE MULTIPLIER: " + num4 + " (speedStat: " + statByNum + ")");
		return num4;
	}

	public virtual float GetCurrentShootingArch(float oppConStat, int currentHalf)
	{
		int num = 0;
		float num2 = 60f;
		float num3 = 90f;
		int currentTournamentNum = Tournaments.GetCurrentTournamentNum();
		int currentRound = Tournaments.GetCurrentRound(currentTournamentNum);
		switch (currentRound)
		{
		case 0:
		case 1:
			num = 10;
			break;
		case 2:
			num = -15;
			break;
		case 3:
			num = 20;
			break;
		}
		if (currentHalf == 2)
		{
			num *= -1;
		}
		if (Stats.GetNumWins() < 6 && num > 0)
		{
			num = 0;
		}
		float statByNum = GetStatByNum(Players.SHOOTING, false, currentTournamentNum, currentRound);
		statByNum += (float)num;
		if (statByNum < 1f)
		{
			statByNum = 1f;
		}
		float num4 = statByNum / (statByNum + oppConStat);
		float num5 = num2 + (num3 - num2) * num4;
		Debug.Log("COMPUTER SHOOTING ARC: " + num5 + " (shootingStat: " + statByNum + ")");
		return num5;
	}

	public virtual int GetChanceOfGoingForPumpFake()
	{
		return chanceOfGoingForPumpFake;
	}

	public virtual bool IsLowOnHydration()
	{
		return isDehydrated;
	}

	public virtual bool NeverShoot()
	{
		return neverShoot;
	}

	public static float GetStatByNum(int num, bool returnCounterStat, int t, int r)
	{
		int num2 = Tournaments.GetNumCompletions(t);
		if (num2 > 2)
		{
			num2 = 2;
		}
		if (returnCounterStat)
		{
			if (num == Players.SHOOTING)
			{
				num = Players.JUMP;
			}
			else if (num == Players.JUMP)
			{
				num = Players.SHOOTING;
			}
			else if (num == Players.SPEED)
			{
				num = Players.DEFENSE;
			}
			else if (num == Players.DEFENSE)
			{
				num = Players.SPEED;
			}
		}
		if (t == 125)
		{
			t = 30;
		}
		float num3 = (float)t * 1f + (float)r * ((float)t * 0.5f) + 1f;
		if (num == Players.SIZE)
		{
			if (t % 3 == 0 && r == 3)
			{
				num3 += num3 * 0.45f;
			}
			else if (r > 1)
			{
				num3 += num3 * 0.25f;
			}
		}
		else if (num == Players.SPEED)
		{
			if ((t + 2) % 3 == 0 && r == 2)
			{
				num3 += num3 * 0.45f;
			}
			num3 = ((t % 3 != 0 || (r != 1 && r != 3)) ? (num3 + ((num3 + 1f) * 0.15f + 1f)) : (num3 + num3 * 0.55f));
			num3 += (float)num2 * 15f;
		}
		else if (num == Players.JUMP)
		{
			num3 = (((t + 1) % 3 == 0 && r == 3) ? (num3 + num3 * 0.5f) : (((t + 2) % 3 != 0 || r != 1) ? (num3 + num3 * 0.2f) : (num3 + num3 * -0.45f)));
		}
		else if (num == Players.SHOOTING)
		{
			if ((t + 3) % 3 == 0 && r == 2)
			{
				num3 += num3 * 0.65f;
			}
			else if ((t + 4) % 3 == 0 && r == 2)
			{
				num3 += num3 * -0.65f;
			}
			if ((t + 3) % 3 == 0 && r == 1)
			{
				num3 += num3 * -0.65f;
			}
			else if ((t + 4) % 3 == 0 && r == 1)
			{
				num3 += num3 * 0.65f;
			}
			if ((t + 1) % 2 == 0 && (r == 1 || r == 3))
			{
				num3 += num3 * 0.65f;
			}
			if (t == 1 && r == 1)
			{
				num3 = 18f;
			}
			if (t >= 10)
			{
				num3 += num3 * 0.25f;
			}
			num3 += (float)num2 * 20f;
		}
		else if (num == Players.DEFENSE)
		{
			num3 = (((t + 4) % 3 != 0 || (r != 1 && r != 3)) ? (num3 + num3 * -0.45f) : (num3 + num3 * 0.45f));
			if (t <= 1)
			{
				num3 = 1f;
			}
			num3 += (float)num2 * 15f;
		}
		return num3;
	}

	public virtual void SetTournament(Tournament tournament)
	{
		this.tournament = tournament;
	}
}
