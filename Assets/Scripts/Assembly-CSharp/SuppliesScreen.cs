using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SuppliesScreen : MonoBehaviour
{
	public Localize typeUseDesc;

	public Image[] typeButtonImages;

	public Text[] typeAmounts;

	public SuppliesPack[] suppliesPacks;

	public string[] typeUseDescs;

	public ScrollRectScroller scrollRectScroller;

	private int selectedItemType;

	public Color unselectedTypeColor;

	public Color selectedTypeColor;

	public GameObject drinkExclamationIcon;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public SuppliesScreen()
	{
		selectedItemType = Supplies.OXYGEN;
	}

	public virtual void OnEnable()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		for (int i = 0; i < typeAmounts.Length; i++)
		{
			typeAmounts[i].text = Supplies.GetItemAmount(i).ToString();
		}
		if (sessionVars.selectedSupply == 2 || sessionVars.selectedSupply == 3)
		{
		}
		drinkExclamationIcon.SetActive(PlayerPrefs.GetInt(Supplies.COLLECTED_FREE_KEY + Supplies.DRINK) == 0);
		SelectItemType(sessionVars.selectedSupply);
	}

	public virtual void SelectItemType(int type)
	{
		selectedItemType = type;
		Image[] array = typeButtonImages;
		foreach (Image image in array)
		{
			image.color = unselectedTypeColor;
		}
		typeButtonImages[type].color = selectedTypeColor;
		SuppliesPack[] array2 = suppliesPacks;
		foreach (SuppliesPack suppliesPack in array2)
		{
			bool isFree = false;
			if (type == Supplies.DRINK && suppliesPack.packNum == 0 && PlayerPrefs.GetInt(Supplies.COLLECTED_FREE_KEY + type) == 0)
			{
				isFree = true;
				drinkExclamationIcon.SetActive(false);
			}
			StartCoroutine(suppliesPack.SetType(type, isFree));
			LeanTween.scale(suppliesPack.gameObject, new Vector2(0.25f, 0.25f), 0.075f).setEase(LeanTweenType.easeOutSine);
		}
		typeUseDesc.SetTerm(typeUseDescs[type], null);
		typeUseDesc.gameObject.SetActive(false);
		StartCoroutine(ShowPacksWithDelay());
		if ((type != Supplies.DRINK || PlayerPrefs.GetInt(Supplies.COLLECTED_FREE_KEY + type) != 0) && Supplies.GetItemAmount(type) == 0)
		{
			AdMediation.ShowTjpOutOfStoreItem();
		}
	}

	public virtual IEnumerator ShowPacksWithDelay()
	{
		yield return new WaitForSeconds(0.15f);
		SuppliesPack[] array = suppliesPacks;
		foreach (SuppliesPack suppliesPack in array)
		{
			LeanTween.scale(suppliesPack.gameObject, new Vector3(1f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeOutSine);
		}
		yield return new WaitForSeconds(0.15f);
		typeUseDesc.gameObject.SetActive(true);
	}

	public virtual void PlaySelectSound()
	{
		gameSounds.Play_select();
	}

	public virtual void Update()
	{
	}
}
