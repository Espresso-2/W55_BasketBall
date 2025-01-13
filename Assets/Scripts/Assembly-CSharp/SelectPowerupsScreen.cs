using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SelectPowerupsScreen : MonoBehaviour
{
	public Matchup matchup;

	public GameObject selectPowerupsMsg;

	public Image[] typeButtonImages;

	public Text[] typeAmounts;

	public Image[] typeAmountIcons;

	public GameObject[] typeCheckMarks;

	public Animator[] typeAnimators;

	public Color unselectedColor;

	public Color selectedColor;

	public Color amountEmptyColor;

	public GameObject[] getMoreButtons;

	public CoachMsgBox coachMsgBox;

	public CoachMsgBox coachPowerupWarningBox;

	public Image startButtonImg;

	private Color startButtonImgOrigColor;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public GameObject[] hintArrows;

	private int currentArrow;

	private bool usingAllPowerups;

	private bool mustUseAllPowerups;

	private bool canStartGame;

	private bool showArrowsToEncourage;

	public virtual IEnumerator Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		int numWins = Stats.GetNumWins();
		if (numWins == 0)
		{
			ShowHelp();
		}
		if (numWins <= 10)
		{
			showArrowsToEncourage = true;
		}
		startButtonImgOrigColor = startButtonImg.color;
		startButtonImg.color = Color.gray;
		canStartGame = false;
		UpdateItemDisplay();
		yield return new WaitForSeconds(1.5f);
		if (!mustUseAllPowerups && !canStartGame)
		{
			canStartGame = true;
			startButtonImg.color = startButtonImgOrigColor;
		}
	}

	public virtual void SelectItem(int type)
	{
		sessionVars.usingPowerups[type] = !sessionVars.usingPowerups[type];
		if (sessionVars.usingPowerups[type])
		{
			gameSounds.Play_select();
			gameSounds.Play_air_pump();
		}
		else
		{
			gameSounds.Play_select();
		}
		UpdateItemDisplay();
	}

	private void UpdateItemDisplay()
	{
		GameObject[] array = hintArrows;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		bool flag = false;
		usingAllPowerups = true;
		for (int j = 0; j < sessionVars.usingPowerups.Length; j++)
		{
			int itemAmount = Supplies.GetItemAmount(Supplies.GetSupplyByPowerupNum(j));
			getMoreButtons[j].SetActive(itemAmount == 0);
			if (itemAmount == 0)
			{
				typeAmountIcons[j].color = amountEmptyColor;
				sessionVars.usingPowerups[j] = false;
			}
			if (sessionVars.usingPowerups[j] && itemAmount > 0)
			{
				typeButtonImages[j].color = selectedColor;
				typeCheckMarks[j].SetActive(true);
				typeAmounts[j].text = (itemAmount - 1).ToString();
				typeAnimators[j].enabled = false;
				continue;
			}
			typeButtonImages[j].color = unselectedColor;
			typeCheckMarks[j].SetActive(false);
			typeAmounts[j].text = itemAmount.ToString();
			typeAnimators[j].enabled = true;
			usingAllPowerups = false;
			if (!flag && showArrowsToEncourage && itemAmount > 0)
			{
				hintArrows[j].SetActive(true);
				flag = true;
			}
		}
		if (!usingAllPowerups)
		{
			selectPowerupsMsg.SetActive(mustUseAllPowerups);
		}
		else
		{
			selectPowerupsMsg.SetActive(false);
			canStartGame = true;
			startButtonImg.color = startButtonImgOrigColor;
		}
		if (!flag && Stats.GetNumWins() == 0)
		{
			hintArrows[hintArrows.Length - 1].SetActive(true);
		}
		matchup.gameObject.SendMessage("ShowComparison");
	}

	public virtual void GetMore(int type)
	{
		switch (type)
		{
		case 0:
			sessionVars.selectedSupply = Supplies.GRIP;
			break;
		case 1:
			sessionVars.selectedSupply = Supplies.CHALK;
			break;
		case 2:
			sessionVars.selectedSupply = Supplies.PROTEIN;
			break;
		}
		gameSounds.Play_one_dribble();
		TabChanger.currentTabNum = tabEnum.Supplies;
		TabChanger.currentBackAction = backAction.Matchup;
		Application.LoadLevel("MainUI");
	}

	public virtual void TryToStartMatch()
	{
		StartCoroutine(TryToStartMatchCoroutine());
	}

	private IEnumerator TryToStartMatchCoroutine()
	{
		if (!usingAllPowerups && mustUseAllPowerups)
		{
			GameObject[] array = hintArrows;
			foreach (GameObject hint in array)
			{
				if (hint.activeInHierarchy)
				{
					gameSounds.Play_pinball_beep();
					Vector2 curScale = new Vector2(hint.transform.localScale.x, hint.transform.localScale.y);
					LeanTween.scale(hint, new Vector3(curScale.x * 1.25f, curScale.y * 1.25f, 1f), 0.25f).setEase(LeanTweenType.easeOutQuad);
					yield return new WaitForSeconds(0.25f);
					LeanTween.scale(hint, new Vector3(curScale.x, curScale.y, 1f), 0.25f).setEase(LeanTweenType.easeOutQuad);
				}
			}
		}
		else if (canStartGame)
		{
			GameObject[] array2 = hintArrows;
			foreach (GameObject gameObject in array2)
			{
				gameObject.SetActive(false);
			}
			matchup.gameObject.SendMessage("StartMatch");
		}
	}

	public virtual void ShowHelp()
	{
		coachMsgBox.gameObject.SetActive(true);
		coachMsgBox.SelectPowerups();
	}
}
