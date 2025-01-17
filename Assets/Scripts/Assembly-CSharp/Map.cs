using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Map : MonoBehaviour
{
	public TournamentPin[] pins;

	public Tournaments tournaments;

	public Players players;

	public PlayButton playButton;

	public CoachMsgBox coachMsgBox;

	public GameObject hintArrow;

	/*public GameObject coachRewardBox;*/

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		players.InstantiatePlayers();
	}

	public virtual void OnEnable()
	{
		ShowCorrectPins();
		sessionVars.numMapViewsThisSession++;
	}

	public virtual IEnumerator Start()
	{
		bool signedStarter = Players.GetActiveStarterNum(false) != -1;
		coachMsgBox.gameObject.SetActive(!signedStarter);
		coachMsgBox.NewTeam();
		/*if (sessionVars.justCompletedMatch || sessionVars.numMapViewsThisSession == 1)
		{
			/*coachRewardBox.SetActive(true);#1#
			if (sessionVars.wonLastGame)
			{
				int numWins = Stats.GetNumWins();
				/*if (numWins == 1)
				{
					AdMediation.ShowTjpTourScreenAfterFirstWin();
				}
				else if (numWins == 2)
				{
					AdMediation.ShowTjpTourScreenAfterSecondWin();
				}
				else if (numWins >= 3)
				{
					AdMediation.ShowTjpTourScreenAfterWin();
				}#1#
			}
			else
			{
				/*int numLosses = Stats.GetNumLosses();
				if (numLosses == 1)
				{
					AdMediation.ShowTjpTourScreenAfterFirstLoss();
				}
				else
				{
					AdMediation.ShowTjpTourScreenAfterLoss();
				}#1#
			}
		}
		else
		{
			/*AdMediation.ShowTjpTourScreen();#1#
		}*/
		Tournament t = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		bool showTournament = t.currentRound > 1 || t.LostLastAttempt() || TournamentView.showLeaderboardPanel;
		if (t.num == 0 && signedStarter)
		{
			showTournament = true;
		}
		GoToPin(t.num, showTournament);
		if (t.num == 0 && signedStarter && t.currentRound <= 1 && !Tournaments.TournamentIsCompleted(0))
		{
			yield return new WaitForSeconds(0.75f);
			hintArrow.SetActive(true);
		}
		/*FlurryAnalytics.Instance().LogEvent("SCREEN_TOUR", new string[4]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
		sessionVars.justCompletedMatch = false;
	}

	private void ShowCorrectPins()
	{
		Player starter = players.GetStarter(false, Players.GetActiveStarterNum(false));
		float statTotal = starter.GetStatTotal();
		Player backup = players.GetBackup(false, Players.GetActiveBackupNum(false));
		float statTotal2 = backup.GetStatTotal();
		Player starter2 = players.GetStarter(true, Players.GetActiveStarterNum(true));
		float statTotal3 = starter2.GetStatTotal();
		Player backup2 = players.GetBackup(true, Players.GetActiveBackupNum(true));
		float statTotal4 = backup2.GetStatTotal();
		TournamentPin[] array = pins;
		foreach (TournamentPin tournamentPin in array)
		{
			if (tournamentPin.isPractice)
			{
				tournamentPin.gameObject.SetActive(Stats.GetNumWins() >= 2);
				continue;
			}
			Tournament tournament = tournaments.GetTournament(tournamentPin.tournamentNum);
			bool flag = tournament.IsVisible();
			tournamentPin.gameObject.SetActive(flag);
			if (flag)
			{
				if (tournament.isFemale)
				{
					tournamentPin.UpdateState((int)tournament.GetReqAbility(0), (int)statTotal3, (int)tournament.GetReqAbility(1), (int)statTotal4);
				}
				else
				{
					tournamentPin.UpdateState((int)tournament.GetReqAbility(0), (int)statTotal, (int)tournament.GetReqAbility(1), (int)statTotal2);
				}
			}
		}
	}

	public virtual void GoToPin(int num, bool showTournament)
	{
		TournamentPin tournamentPin = pins[num + 1];
		if (tournamentPin.gameObject.activeInHierarchy)
		{
			tournamentPin.GoToPin(showTournament);
		}
	}

	public virtual void UpgradeWasCompleted()
	{
		ShowCorrectPins();
	}

	public virtual void MakePinHidden(int num)
	{
		pins[num].MakeHidden();
	}

	public virtual IEnumerator SegueToPin(int num)
	{
		yield return new WaitForSeconds(0.75f);
		TournamentPin pin = pins[num];
		pin.ScrollTo();
		gameSounds.Play_air_pump();
		yield return new WaitForSeconds(0.75f);
		GoToPin(num, false);
		yield return new WaitForSeconds(0.5f);
		playButton.EnableButton();
		playButton.SetToPlay();
	}

	public virtual void DisablePlayButton()
	{
		playButton.DisableButton();
	}

	public virtual void Update()
	{
	}

	public virtual void MakeAllPinsUnselected()
	{
		TournamentPin[] array = pins;
		foreach (TournamentPin tournamentPin in array)
		{
			tournamentPin.MakeUnselected();
		}
	}

	public virtual void PlaySelectSound(bool move)
	{
		gameSounds.Play_select();
		if (move)
		{
			gameSounds.Play_air_pump();
		}
	}
}
