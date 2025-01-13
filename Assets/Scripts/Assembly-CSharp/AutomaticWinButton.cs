using System.Collections;
using UnityEngine;

public class AutomaticWinButton : MonoBehaviour
{
	public GameController gameController;

	private int numClicks;

	private void Start()
	{
		StartCoroutine(CheckIfActive());
	}

	private IEnumerator CheckIfActive()
	{
		yield return new WaitForSeconds(4f);
		if (PlayerPrefs.GetInt("AUTO_WIN_ENABLED") == 0 && !gameController.InTutorial)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void OnClick()
	{
		numClicks++;
		if (numClicks >= 5)
		{
			if (gameController.InTutorial)
			{
				PlayerPrefsHelper.SetInt("STARTED_SCRIM", 1);
				PlayerPrefsHelper.SetInt("COMPLETED_SCRIM", 1);
			}
			gameController.AddGameSeconds(25f + 3f * Random.Range(1f, 4f));
			Stats.numMakes = Random.Range(4, 7);
			Stats.numShots = Random.Range(8, 12);
			Stats.num3PtMakes = Random.Range(0, 1);
			Stats.num3PtShots = Random.Range(1, 4);
			Stats.numRebounds = Random.Range(0, 6);
			Stats.numSteals = Random.Range(0, 3);
			Stats.numBlocks = Random.Range(0, 5);
			Stats.numPoints = Random.Range(11, 14);
			Stats.p1Score = Stats.numPoints;
			Stats.p2Score = Random.Range(0, 11);
			gameController.score.player1Score = Stats.p1Score;
			gameController.score.player2Score = Stats.p2Score;
			gameController.EndGame(false);
		}
	}
}
