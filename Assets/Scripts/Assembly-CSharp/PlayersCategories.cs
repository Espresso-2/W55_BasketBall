using System;
using UnityEngine;

[Serializable]
public class PlayersCategories : MonoBehaviour
{
	public PlayersScreen playersScreen;

	public GameObject maleHintArrow;

	public GameObject femaleHintArrow;

	public GameObject tourTabHintArrow;

	public GameObject[] femaleLocks;

	public TopNavBar topNavBar;

	public TabChanger tabChanger;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	public virtual void Awake()
	{
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
	}

	public virtual void OnEnable()
	{
		if (Tournaments.TournamentIsCompleted(0))
		{
			GameObject[] array = femaleLocks;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(false);
			}
		}
		if (!Players.IsStarterPurchased(false, 0))
		{
			maleHintArrow.SetActive(true);
		}
		else if (!Tournaments.TournamentIsCompleted(0))
		{
			tourTabHintArrow.SetActive(true);
		}
		else if (Tournaments.TournamentIsCompleted(0) && !Tournaments.TournamentIsCompleted(4) && Players.GetActiveStarterNum(true) == -1)
		{
			femaleHintArrow.SetActive(true);
		}
	}

	public virtual void ShowStarters(bool female)
	{
		gameSounds.Play_one_dribble();
		sessionVars.showingBackups = false;
		sessionVars.showingFemales = female;
		TabChanger.currentBackAction = backAction.CurrentTab;
		tabChanger.SetToScreen(screenEnum.Players);
		topNavBar.SetTitleTerm("STARTERS");
		/*AdMediation.ShowTjpStartersScreen();*/
	}

	public virtual void ShowBackups(bool female)
	{
		gameSounds.Play_one_dribble();
		sessionVars.showingBackups = true;
		sessionVars.showingFemales = female;
		TabChanger.currentBackAction = backAction.CurrentTab;
		tabChanger.SetToScreen(screenEnum.Players);
		topNavBar.SetTitleTerm("BACKUPS");
		/*AdMediation.ShowTjpBackupsScreen();*/
	}
}
