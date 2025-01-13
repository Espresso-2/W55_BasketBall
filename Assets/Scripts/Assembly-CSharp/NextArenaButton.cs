using UnityEngine;
using UnityEngine.UI;

public class NextArenaButton : MonoBehaviour
{
	public Tournaments tournaments;

	private static int num;

	public Text text;

	public bool changeActualTournament;

	private void Start()
	{
		if (PlayerPrefs.GetInt("NEXT_ARENA_BUTTON_ENABLED") == 0)
		{
			base.gameObject.SetActive(false);
		}
		if (changeActualTournament)
		{
			num = Tournaments.GetCurrentTournamentNum();
		}
		else
		{
			num = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum()).arena;
		}
		UpdateDisplay();
	}

	public void OnClick(bool prev)
	{
		if (prev)
		{
			num--;
			if (num < 0)
			{
				num = 99;
			}
		}
		else
		{
			num++;
		}
		if (changeActualTournament)
		{
			Tournaments.SetCurrentTournamentNum(num);
			Application.LoadLevel("GamePlay");
		}
		else
		{
			ArenaSkinController.Instance.UpdateArenaSkin(num);
			UpdateDisplay();
		}
	}

	private void UpdateDisplay()
	{
		if (changeActualTournament)
		{
			Tournament tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
			text.text = string.Empty + num + ": " + tournament.name;
		}
		else
		{
			text.text = string.Empty + num;
		}
	}
}
