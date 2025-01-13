using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PracticeView : MonoBehaviour
{
	public Text heading;

	public PlayButton playButton;

	public GameObject femalePlayIcon;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		heading.text = string.Empty + TeamDetails.GetTeamName() + " PRACTICE FACILITY";
	}

	public virtual void OnEnable()
	{
		playButton.DisableButton();
	}

	public virtual void Update()
	{
	}

	public virtual void ClosePractice()
	{
		if (gameSounds != null)
		{
			gameSounds.Play_select();
		}
		base.gameObject.SetActive(false);
	}
}
