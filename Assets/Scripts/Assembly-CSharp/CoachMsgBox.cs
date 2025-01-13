using System;
using I2.Loc;
using UnityEngine;

[Serializable]
public class CoachMsgBox : MonoBehaviour
{
	public Localize headingText;

	public Localize bodyText;

	public GameObject[] hintArrows;

	private int currentArrow;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		GameObject[] array = hintArrows;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
	}

	public virtual void NewTeam()
	{
		headingText.SetTerm("YOU GOT A TEAM", null);
		bodyText.SetTerm("YOU GOT A TEAM DESC", null);
	}

	public virtual void SignPlayer()
	{
		headingText.SetTerm("SIGN NEW PLAYER", null);
		bodyText.SetTerm("SIGN A STARTER TO TEAM UP WITH JOE HOPKINS", null);
	}

	public virtual void SignFemalePlayer()
	{
		headingText.SetTerm("WOMEN'S TOURNAMENT", null);
		bodyText.SetTerm("SIGN A STARTER TO TEAM UP WITH TINA CLARK", null);
		currentArrow = 999;
	}

	public virtual void UpgradePlayer()
	{
		headingText.SetTerm("UPGRADE YOUR PLAYERS", null);
		bodyText.SetTerm("THE COMPETITION WILL GET TOUGHER", null);
		currentArrow = 1;
		PlayerPrefsHelper.SetInt("SHOWED_COACHMSGBOX_UPGRADEPLAYER", 1, true);
	}

	public virtual void SelectPowerups()
	{
		headingText.SetTerm("YOUR TEAM STATS", null);
		bodyText.SetTerm("YOUR TEAM STATS DESC", null);
	}

	public virtual void MissingPowerupWarnings(bool[] usingPowerups)
	{
	}

	public virtual void WaitForTournament()
	{
		headingText.SetTerm("STARTING SOON", null);
		bodyText.SetTerm("THIS TOURNAMENT WILL START", null);
		currentArrow = 2;
	}

	public virtual void CloseMsg(bool showHintArrow)
	{
		if (showHintArrow && currentArrow < hintArrows.Length && hintArrows[currentArrow] != null)
		{
			hintArrows[currentArrow].SetActive(true);
		}
		currentArrow++;
		gameSounds.Play_select();
		base.gameObject.SetActive(false);
	}

	public virtual void OnModifyBodyTextLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string teamName = TeamDetails.GetTeamName();
			Localize.MainTranslation = Localize.MainTranslation.Replace("{TEAM_NAME}", teamName);
		}
	}

	public virtual void Update()
	{
	}
}
