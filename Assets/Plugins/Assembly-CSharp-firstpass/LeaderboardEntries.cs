using UnityEngine;

public class LeaderboardEntries : MonoBehaviour
{
	public LeaderboardEntry[] entries;

	public LeaderboardEntry currentUserEntry;

	public GameObject loadingIcon;

	public void HideLoadingIcon()
	{
		loadingIcon.SetActive(false);
	}

	public void HideCells()
	{
		loadingIcon.SetActive(true);
		for (int i = 0; i < entries.Length; i++)
		{
			LeaderboardEntry leaderboardEntry = entries[i];
			leaderboardEntry.UpdateDisplay(i, "HIDEEMPTYCELL", -1, string.Empty, string.Empty, false);
		}
	}

	public void HideCurrentUserCell()
	{
		currentUserEntry.gameObject.transform.parent.gameObject.SetActive(false);
	}
}
