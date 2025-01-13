using UnityEngine;

public class PrevWinnersPanel : MonoBehaviour
{
	public LeaderboardEntries leaderboardEntries;

	private void OnEnable()
	{
		if (leaderboardEntries != null)
		{
			leaderboardEntries.SendMessage("HideCells");
		}
		if (PlayFabLeaderboard.leaderBoardVersion <= 0)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			PlayFabLeaderboard.GetPrevEventTopThree(leaderboardEntries, true, false);
		}
	}
}
