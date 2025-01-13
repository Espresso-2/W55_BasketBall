using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomItemDisplay : MonoBehaviour
{
	public BagCard[] bagCards;

	public RectTransform scrollSize;

	public ScrollRect scrollRect;

	public TabDisplayer tabDisplayer;

	private CustomItems customItems;

	public static int currentItemType = CustomItems.BALL;

	public virtual void Start()
	{
		PlayerPrefsHelper.SetInt("NEW_CUSTOM_ITEM", 0);
		PlayerPrefsHelper.SetInt("SHOWED_CUSTOMIZE_SCREEN", 1);
	}

	public void OnEnable()
	{
		SetType(currentItemType);
	}

	public void SetType(int type)
	{
		currentItemType = type;
		customItems = (CustomItems)GameObject.Find("CustomItems").GetComponent(typeof(CustomItems));
		CustomItem activeItem = customItems.GetActiveItem(currentItemType);
		Refresh(activeItem.num);
		scrollRect.verticalNormalizedPosition = 1f;
		tabDisplayer.SetActiveTab(currentItemType);
	}

	public void Refresh(int activeItemNum)
	{
		List<CustomItem> ownedItems = customItems.GetOwnedItems(currentItemType);
		for (int i = 0; i < bagCards.Length; i++)
		{
			BagCard bagCard = bagCards[i];
			if (i < ownedItems.Count)
			{
				bagCard.gameObject.SetActive(true);
				CustomItem customItem = ownedItems[i];
				bagCard.SetToItem(customItem, false);
				bagCard.front.SetActive(true);
				if (customItem.num == activeItemNum)
				{
					bagCard.activeLabel.SetActive(true);
					bagCard.activateButton.SetActive(false);
				}
				else
				{
					bagCard.activeLabel.SetActive(false);
					bagCard.activateButton.SetActive(true);
				}
			}
			else
			{
				bagCard.gameObject.SetActive(false);
			}
		}
		float num = 140f;
		int num2 = 5;
		int num3 = Mathf.RoundToInt(ownedItems.Count / num2) + 1;
		if (num3 < 3)
		{
			num3 = 3;
		}
		float y = (float)num3 * num;
		scrollSize.sizeDelta = new Vector2(scrollSize.sizeDelta.x, y);
	}

	public void SetToBall()
	{
		SetType(CustomItems.BALL);
	}

	public void SetToJersey()
	{
		SetType(CustomItems.JERSEY);
	}

	public void SetToArmBand()
	{
		SetType(CustomItems.ARM_BAND);
	}

	public void SetToPants()
	{
		SetType(CustomItems.PANTS);
	}

	public void SetToShoes()
	{
		SetType(CustomItems.SHOES);
	}
}
