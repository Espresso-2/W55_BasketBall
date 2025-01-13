using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ToggleRecordingGameWinners : MonoBehaviour
{
	public Text text;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		if (PlayerPrefs.GetInt("DISABLE_REPLAYS") == 1)
		{
			text.text = "ENABLE GAME WINNER REPLAYS";
		}
		else
		{
			text.text = "DISABLE REPLAYS: DO THIS IF GAME RUNS SLOW";
		}
	}

	public virtual void OnClick()
	{
		gameSounds.Play_select();
		if (PlayerPrefs.GetInt("DISABLE_REPLAYS") == 1)
		{
			text.text = "GAME WINNER REPLAYS ENABLED!";
			PlayerPrefsHelper.SetInt("DISABLE_REPLAYS", 0);
		}
		else
		{
			text.text = "GAME WINNER REPLAYS DISABLED";
			PlayerPrefsHelper.SetInt("DISABLE_REPLAYS", 1);
		}
	}

	public virtual void Update()
	{
	}
}
