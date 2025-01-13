using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LeaderboardPanel : MonoBehaviour
{
	public Text leagueNumText;

	public Image[] tabs;

	public ScrollRect scrollRect;

	public LeaderboardEntries leaderboardEntries;

	public GameObject curSeasonHeader;

	public GameObject prevSeasonHeader;

	public GameObject backButton;

	public Transform entryHolder;

	private GameSounds gameSounds;

	private Color tabSelectedColor;

	private Color tabUnselectedColor;

	public LeaderboardPanel()
	{
		tabSelectedColor = new Color32(37, 39, 49, byte.MaxValue);
		tabUnselectedColor = new Color32(66, 67, 74, byte.MaxValue);
	}

	public virtual void Start()
	{
	}

	public virtual void OnEnable()
	{
		if (gameSounds == null)
		{
			gameSounds = GameSounds.GetInstance();
		}
		gameSounds.Play_select();
		leagueNumText.text = "LEAGUE #" + PlayerPrefs.GetInt("LEAGUE_NUM");
		ShowGlobal(false);
	}

	public virtual void ShowGlobal(bool prevVersion)
	{
		gameSounds.Play_select();
		tabs[0].color = tabSelectedColor;
		tabs[1].color = tabUnselectedColor;
		curSeasonHeader.SetActive(!prevVersion);
		prevSeasonHeader.SetActive(prevVersion);
		backButton.SetActive(prevVersion);
		if (prevVersion)
		{
			entryHolder.localPosition = new Vector3(0f, 900f, 0f);
		}
		else
		{
			entryHolder.localPosition = new Vector3(0f, 863f, 0f);
		}
		PlayFabLeaderboard.GetEventLeaderboard(leaderboardEntries, prevVersion, false);
		scrollRect.verticalNormalizedPosition = 1f;
		HideCells();
	}

	public virtual void ShowFriends()
	{
		gameSounds.Play_select();
		tabs[0].color = tabUnselectedColor;
		tabs[1].color = tabSelectedColor;
		curSeasonHeader.SetActive(false);
		prevSeasonHeader.SetActive(false);
		backButton.SetActive(false);
		entryHolder.localPosition = new Vector3(0f, 1017f, 0f);
		scrollRect.verticalNormalizedPosition = 1f;
		HideCells();
	}

	private void HideCells()
	{
		if (leaderboardEntries != null)
		{
			leaderboardEntries.SendMessage("HideCells");
		}
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			base.gameObject.SetActive(false);
		}
	}
}
