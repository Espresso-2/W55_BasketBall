using System;
using I2.Loc;
using UnityEngine;

[Serializable]
public class HotPurchaseBox : MonoBehaviour
{
	public Localize headingText;

	public TopNavBar topNavBar;

	public SuppliesPack[] suppliesPacks;

	private int selectedItemType;

	public HotPurchaseBox()
	{
		selectedItemType = Supplies.DRINK;
	}

	public virtual void OnEnable()
	{
		UpdateCurrencyDisplay();
	}

	public virtual void UpdateCurrencyDisplay()
	{
		topNavBar.UpdateGoldWithNoAnimationOrAnyDelay();
	}

	public virtual void SelectItemType(int type)
	{
		selectedItemType = type;
		SuppliesPack[] array = suppliesPacks;
		foreach (SuppliesPack suppliesPack in array)
		{
			StartCoroutine(suppliesPack.SetType(type, false));
		}
		if (type == Supplies.DRINK)
		{
			headingText.SetTerm("OUT OF SPORTS DRINKS", null);
		}
		else if (type != Supplies.OXYGEN)
		{
		}
	}

	public virtual void AddedGoldPackage(int pkgNum)
	{
		UpdateCurrencyDisplay();
	}
}
