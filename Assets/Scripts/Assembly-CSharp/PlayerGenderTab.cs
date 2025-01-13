using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerGenderTab : MonoBehaviour
{
	public PlayerDetails[] playerDetails;

	public bool female;

	public GameObject otherTab;

	public Text text;

	private GameSounds gameSounds;

	private bool tabDisabled;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		if (female && Players.GetActiveStarterNum(true) == -1)
		{
			text.gameObject.SetActive(false);
			tabDisabled = true;
		}
		else
		{
			text.gameObject.SetActive(true);
			tabDisabled = false;
		}
	}

	public virtual void OnClick()
	{
		if (!tabDisabled)
		{
			gameSounds.Play_select();
			SetGender();
		}
	}

	public virtual void SetGender()
	{
		PlayerDetails[] array = this.playerDetails;
		foreach (PlayerDetails playerDetails in array)
		{
			playerDetails.showingFemales = female;
			playerDetails.ShowCorrectPlayer();
		}
		otherTab.SetActive(true);
		base.gameObject.SetActive(false);
	}

	public virtual void Update()
	{
	}
}
