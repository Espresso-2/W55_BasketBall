using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameScore : MonoBehaviour
{
	public int player1Score;

	public int player2Score;

	public GameObject visual;

	public Text score1Name;

	public Localize score1NameLocalize;

	public Text score1A;

	public Text score1B;

	public Text score2Name;

	public Localize score2NameLocalize;

	public Text score2A;

	public Text score2B;

	public Text roundName;

	public Localize roundNameLocalize;

	public SlideInEffect slideInEffect;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	private bool playCrowdReactionSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
		if (sessionVars.goToTutorial)
		{
			roundNameLocalize.SetTerm("FIRST TO 7 POINTS", null);
			score1NameLocalize.SetTerm("YOU", null);
			score2NameLocalize.SetTerm("OPPONENT", null);
			visual.SetActive(false);
			return;
		}
		if (sessionVars.twoPlayerMode)
		{
			roundNameLocalize.SetTerm("FIRST TO 12 POINTS", null);
			score1NameLocalize.SetTerm("GREEN", null);
			score2NameLocalize.SetTerm("PINK", null);
			return;
		}
		if (sessionVars.goToPractice)
		{
			score1Name.text = TeamDetails.GetTeamName();
			score2Name.text = string.Empty;
			roundNameLocalize.SetTerm("PRACTICE", null);
			return;
		}
		Tournament currentTournament = sessionVars.currentTournament;
		string teamName = TeamDetails.GetTeamName();
		if (currentTournament.type == tournamentTypeEnum.LiveEvent && !sessionVars.goToPractice && !sessionVars.twoPlayerMode && !sessionVars.goToTutorial)
		{
			score1Name.text = "CURRENT";
			score2Name.text = "BEST ENTRY";
		}
		else
		{
			roundNameLocalize.SetTerm(currentTournament.GetCurrentRoundName(), null);
			score1Name.text = teamName;
			score2Name.text = currentTournament.GetCurrentOpponentName();
		}
	}

	public virtual void Reset()
	{
		player1Score = 0;
		player2Score = 0;
		if (sessionVars.currentTournament.type == tournamentTypeEnum.LiveEvent && !sessionVars.goToPractice && !sessionVars.twoPlayerMode && !sessionVars.goToTutorial)
		{
			UpdateScoreBoard(player1Score, PlayFabLeaderboard.currentEntry);
		}
		else
		{
			UpdateScoreBoard(player1Score, player2Score);
		}
		visual.SetActive(true);
		playCrowdReactionSounds = true;
	}

	public virtual IEnumerator AddShot(bool isEnemyBasket, int pointValue)
	{
		bool cheer = !isEnemyBasket || sessionVars.twoPlayerMode;
		if (pointValue != 3 || cheer)
		{
		}
		/*if (isEnemyBasket)
		{
			FlurryAnalytics.Instance().LogEvent("shot_made_enemy", new string[1] { "pointValue:" + pointValue + string.Empty }, false);
		}
		else
		{
			FlurryAnalytics.Instance().LogEvent("shot_made_player", new string[1] { "pointValue:" + pointValue + string.Empty }, false);
		}*/
		int visualP1Score = player1Score;
		int visualP2Score = player2Score;
		if (isEnemyBasket)
		{
			player2Score += pointValue;
		}
		else
		{
			player1Score += pointValue;
		}
		for (int point = 0; point < pointValue; point++)
		{
			if (isEnemyBasket)
			{
				visualP2Score++;
			}
			else
			{
				visualP1Score++;
			}
			if (playCrowdReactionSounds)
			{
				PlayCrowdReactionSounds(cheer);
			}
			StartCoroutine(PlayScoreboardEffect(cheer));
			yield return new WaitForSeconds(0.15f);
			if (sessionVars.currentTournament.type == tournamentTypeEnum.LiveEvent && !sessionVars.goToPractice && !sessionVars.twoPlayerMode && !sessionVars.goToTutorial)
			{
				UpdateScoreBoard(visualP1Score, PlayFabLeaderboard.currentEntry);
			}
			else
			{
				UpdateScoreBoard(visualP1Score, visualP2Score);
			}
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void PlayCrowdReactionSounds(bool cheer)
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (cheer)
		{
			if (num <= 25)
			{
				gameSounds.Play_crowd_long_cheer_01();
			}
			else if (num <= 50)
			{
				gameSounds.Play_quick_applause();
			}
			else if (num <= 75 || player1Score == 1)
			{
				gameSounds.Play_applause();
			}
		}
		else if (num <= 50 || player2Score == 1)
		{
			gameSounds.Play_crowd_boo_01();
		}
	}

	private IEnumerator PlayScoreboardEffect(bool cheer)
	{
		Vector2 curSbScale = new Vector2(base.gameObject.transform.localScale.x, base.gameObject.transform.localScale.y);
		if (cheer)
		{
			gameSounds.Play_chime_shimmer();
			yield return new WaitForSeconds(0.05f);
			gameSounds.Play_coin_glow();
			LeanTween.scale(base.gameObject, new Vector3(curSbScale.x * 1.03f, curSbScale.y * 1.03f, 1f), 0.05f).setEase(LeanTweenType.easeOutQuad);
			yield return new WaitForSeconds(0.1f);
		}
		else
		{
			yield return new WaitForSeconds(0.05f);
			LeanTween.scale(base.gameObject, new Vector3(curSbScale.x * 1.01f, curSbScale.y * 1.01f, 1f), 0.05f).setEase(LeanTweenType.easeOutQuad);
			yield return new WaitForSeconds(0.1f);
		}
		LeanTween.scale(base.gameObject, new Vector3(curSbScale.x, curSbScale.y, 1f), 0.05f).setEase(LeanTweenType.easeOutQuad);
	}

	public virtual void UpdateScoreBoard(int score1, int score2)
	{
		if (score1A != null)
		{
			score1A.text = string.Empty + Mathf.Floor((float)score1 / 10f);
			score2A.text = string.Empty + Mathf.Floor((float)score2 / 10f);
			score1B.text = string.Empty + score1 % 10;
			score2B.text = string.Empty + score2 % 10;
		}
	}
}
