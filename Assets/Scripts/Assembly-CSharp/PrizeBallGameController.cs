using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrizeBallGameController : MonoBehaviour
{
	public static bool basketballBattleBuild = true;

	public GameObject[] basketballBattleOnly;

	public GameObject[] prizeBallOnly;

	public GameObject exitBox;

	public PrizeBall prizeBall;

	public PrizeSlotHolder prizeSlotHolder;

	public Text headingText;

	public Button dropBallButton;

	public Button newGameButton;

	public Button continueButton;

	/*public Button buyMoreButton15;

	public Button buyMoreButton5;*/

	public GameObject scoreBox;

	public NumTallyUpper scoreAmtText;

	public Text bestScoreText;

	public GameObject largePrizeEffect;

	public PrizeBallGameResults gameResults;

	public BattlePrizeReward battlePrizeReward;

	private int score;

	private int ballNum;

	private GameSounds gameSounds;

	private void Start()
	{
		GameObject[] array = basketballBattleOnly;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(basketballBattleBuild);
		}
		GameObject[] array2 = prizeBallOnly;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(!basketballBattleBuild);
		}
		gameSounds = GameSounds.GetInstance();
		gameResults.gameObject.SetActive(false);
		NewGame();
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !basketballBattleBuild)
		{
			exitBox.SetActive(true);
		}
	}

	public void NewGameOnClick()
	{
		gameSounds.Play_select();
		NewGame();
	}

	private void NewGame()
	{
		score = 0;
		ballNum = 0;
		headingText.text = "奖励球";
		StartCoroutine(SetGameResultsActive(false));
		UpdateScore(0, 0);
		UpdateBestText();
		if (basketballBattleBuild && Currency.GetNumPrizeBalls() < 1)
		{
			StartCoroutine(ShowOutOfBalls());
			return;
		}
		NextBall();
		StartCoroutine(SetDropButtonActive(true));
	}

	private void NextBall()
	{
		continueButton.gameObject.SetActive(false);
		/*buyMoreButton15.gameObject.SetActive(false);
		buyMoreButton5.gameObject.SetActive(false);*/
		ballNum++;
		prizeBall.Reset();
		gameSounds.Play_whoose_low();
		gameSounds.Play_air_pump();
		if (!basketballBattleBuild && ballNum >= 2)
		{
			headingText.text = "BALL " + ballNum;
		}
		prizeSlotHolder.NextBall(ballNum);
		StartCoroutine(SetDropButtonActive(true));
	}

	public void DropBall()
	{
		gameSounds.Play_select();
		gameSounds.Play_whoose_tennis_racket();
		Currency.UsePrizeBall();
		prizeBall.Drop();
		StartCoroutine(SetDropButtonActive(false));
		AchievementsManager.Instance.DroppedPrizeBall();
	}

	public void GameOver()
	{
		gameSounds.Play_chime_2_beeps();
		StartCoroutine(ShowGameOverResults());
	}

	private IEnumerator ShowGameOverResults()
	{
		yield return new WaitForSeconds(0.75f);
		gameSounds.Play_bball_buzzer();
		yield return new WaitForSeconds(1f);
		gameResults.gameObject.SetActive(true);
		gameResults.SetScore(score);
	}

	private void UpdateScore(int amountAdded, int speed)
	{
		if (scoreAmtText.isActiveAndEnabled)
		{
			scoreAmtText.UpdateNum(score - amountAdded, score, speed, true);
		}
	}

	private void UpdateBestText()
	{
		bestScoreText.text = "BEST: " + PlayerPrefs.GetInt("PRIZEBALL_BEST").ToString("n0");
	}

	public IEnumerator ReachedSlot(PrizeSlot prizeSlot)
	{
		if (basketballBattleBuild)
		{
			gameSounds.Play_swoosh();
			yield return new WaitForSeconds(0.75f);
			battlePrizeReward.gameObject.SetActive(true);
			battlePrizeReward.SetReward(prizeSlot);
			if (prizeSlot.type == PrizeSlot.SlotType.GoldPrize || prizeSlot.type == PrizeSlot.SlotType.BagPrize)
			{
				StartCoroutine(ShowLargePrizeEffect());
				yield break;
			}
			gameSounds.Play_chime_shimmer();
			gameSounds.Play_coin_glow_2();
		}
		else
		{
			AddPrizeBallPrize(prizeSlot.amount);
			yield return new WaitForSeconds(2f);
			NextBall();
		}
	}

	private void AddPrizeBallPrize(int amt)
	{
		int speed = 0;
		if (amt >= 5000)
		{
			StartCoroutine(ShowLargePrizeEffect());
			speed = 30;
		}
		else if (amt >= 1000)
		{
			gameSounds.Play_trumpet_chime_2();
			gameSounds.Play_coin_glow_2();
			gameSounds.Play_crowd_long_cheer_01();
			speed = 15;
		}
		else if (amt >= 500)
		{
			gameSounds.Play_chime_shimmer();
			gameSounds.Play_coin_glow_2();
			speed = 10;
		}
		else if (amt >= 1)
		{
			gameSounds.Play_coin_glow_2();
			speed = 3;
		}
		else
		{
			gameSounds.Play_crowd_boo_01();
		}
		score += amt;
		UpdateScore(amt, speed);
	}

	public void ClosedBattlePrizeReward()
	{
		if (Currency.GetNumPrizeBalls() > 0)
		{
			NextBall();
		}
		else
		{
			StartCoroutine(ShowOutOfBalls());
		}
	}

	public void PurchasedPrizeBalls()
	{
		gameSounds.Play_chime_shimmer();
		gameSounds.Play_coin_glow_2();
		NextBall();
	}

	private IEnumerator ShowOutOfBalls()
	{
		dropBallButton.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.75f);
		continueButton.gameObject.SetActive(true);
		// if (PlayerPrefs.GetInt("PRIZE_BALL_AMOUNT") == 15)
		// {
		// 	buyMoreButton15.gameObject.SetActive(true);
		// }
		// else
		// {
		// 	buyMoreButton5.gameObject.SetActive(true);
		// }
	}

	private IEnumerator SetDropButtonActive(bool active)
	{
		yield return new WaitForSeconds(0.1f);
		if (active)
		{
			yield return new WaitForSeconds(0.75f);
		}
		dropBallButton.gameObject.SetActive(active);
	}

	private IEnumerator SetGameResultsActive(bool active)
	{
		yield return new WaitForSeconds(0.1f);
		gameResults.gameObject.SetActive(active);
	}

	private IEnumerator ShowLargePrizeEffect()
	{
		gameSounds.SendMessage("Play_coin_glow_2");
		gameSounds.SendMessage("Play_crowd_long_cheer_01");
		gameSounds.SendMessage("Play_trumpet_chime_2");
		Handheld.Vibrate();
		yield return new WaitForSeconds(0.25f);
		largePrizeEffect.SetActive(true);
		yield return new WaitForSeconds(1f);
		yield return new WaitForSeconds(5f);
		largePrizeEffect.SetActive(false);
	}
}
