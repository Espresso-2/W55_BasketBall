using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DealsScreen : MonoBehaviour
{
	public Image[] typeButtonImages;

	public DealPack dealPack;

	public DealBag dealBag;

	public ScrollRectScroller scrollRectScroller;

	private int selectedItem;

	public Color unselectedTypeColor;

	public Color selectedTypeColor;

	public GameObject panelForDeals;

	public GameObject panelForBags;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private bool firstTimeEnabling;

	public DealsScreen()
	{
		firstTimeEnabling = true;
	}

	public virtual void OnEnable()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		if (TabChanger.subSection != selectedItem || firstTimeEnabling)
		{
			if (TabChanger.subSection >= 3)
			{
				SelectPack(TabChanger.subSection - 3);
			}
			else
			{
				SelectBag(TabChanger.subSection);
			}
		}
		firstTimeEnabling = false;
	}

	public virtual void SelectPack(int num)
	{
		SelectItem(num, false);
	}

	public virtual void SelectBag(int num)
	{
		SelectItem(num, true);
	}

	private void SelectItem(int num, bool isBag)
	{
		selectedItem = num;
		TabChanger.subSection = num;
		int num2 = num;
		if (!isBag)
		{
			num2 += 3;
		}
		Image[] array = typeButtonImages;
		foreach (Image image in array)
		{
			image.color = unselectedTypeColor;
		}
		typeButtonImages[num2].color = selectedTypeColor;
		LeanTween.scale(dealPack.gameObject, new Vector3(0.25f, 0.25f, 1f), 0.075f).setEase(LeanTweenType.easeOutSine);
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ShowPacksWithDelay(num, isBag));
		}
		else
		{
			ShowPacksWithNoDelay(num, isBag);
		}
	}

	public virtual IEnumerator ShowPacksWithDelay(int num, bool isBag)
	{
		yield return new WaitForSeconds(0.15f);
		ShowPacks(num, isBag);
		yield return new WaitForSeconds(0.15f);
	}

	private void ShowPacksWithNoDelay(int num, bool isBag)
	{
		ShowPacks(num, isBag);
	}

	private void ShowPacks(int num, bool isBag)
	{
		if (isBag)
		{
			panelForDeals.SetActive(false);
			panelForBags.SetActive(true);
		}
		else
		{
			panelForDeals.SetActive(true);
			panelForBags.SetActive(false);
		}
		if (isBag)
		{
			dealBag.SetItem(num);
		}
		else
		{
			dealPack.SetItem(num);
		}
		LeanTween.scale(dealPack.gameObject, new Vector3(1f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeOutSine);
	}

	public virtual void PlaySelectSound()
	{
		gameSounds.Play_select();
	}
}
