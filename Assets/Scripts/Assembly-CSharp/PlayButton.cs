using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayButton : MonoBehaviour
{
	public Localize text;

	public Image image;

	public Color enabledColor;

	public Color disabledColor;

	public Image icon;

	public Sprite enabledIcon;

	public Sprite disabledIcon;

	public TournamentView tournamentView;

	private bool playAgain;

	private bool disabled;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void DisableButton()
	{
		disabled = true;
		image.color = disabledColor;
		icon.sprite = disabledIcon;
		text.SetTerm("PLAY", null);
	}

	public virtual void EnableButton()
	{
		disabled = false;
		image.color = enabledColor;
		icon.sprite = enabledIcon;
	}

	public virtual void SetToTryAgain()
	{
		text.SetTerm("TRY AGAIN", null);
		playAgain = true;
	}

	public virtual void SetToPlayAgain()
	{
		text.SetTerm("PLAY AGAIN", null);
		playAgain = true;
	}

	public virtual void SetToPlay()
	{
		text.SetTerm("PLAY", null);
		playAgain = false;
	}

	public virtual void SetToEntriesRemaining(int num)
	{
		if (num > 0)
		{
			EnableButton();
			SetToPlay();
		}
		else
		{
			DisableButton();
		}
		if (num < 0)
		{
			num = 0;
		}
		text.SetTerm(num + " entries remaining", null);
	}

	public virtual void OnClick()
	{
		if (disabled)
		{
			if (tournamentView != null && tournamentView.gameObject.activeInHierarchy)
			{
				StartCoroutine(tournamentView.AnimateUpgradeReq());
			}
			gameSounds.Play_select();
			return;
		}
		GameObject gameObject = GameObject.Find("AdMediation");
		if (gameObject != null)
		{
			gameObject.GetComponent<AdMediation>().MakeAudioOn();
		}
		sessionVars.twoPlayerMode = false;
		sessionVars.goToPractice = false;
		if (!tournamentView.gameObject.activeInHierarchy)
		{
			gameSounds.Play_select();
			StartCoroutine(SetTournamentViewActive());
		}
		else if (playAgain)
		{
			gameSounds.Play_select();
			StartCoroutine(tournamentView.ReloadBracket());
		}
		else
		{
			gameSounds.Play_one_dribble();
			Application.LoadLevel("Matchup");
		}
	}

	private IEnumerator SetTournamentViewActive()
	{
		if (GameObject.Find("CoachRewardBox") == null)
		{
			Time.timeScale = 1f;
		}
		yield return new WaitForSeconds(0.12f);
		tournamentView.gameObject.SetActive(true);
		int tournamentNum = Tournaments.GetCurrentTournamentNum();
		tournamentView.SetViewToTournament(tournamentNum);
		Tournaments.SetCurrentTournamentNum(tournamentNum);
	}
}
