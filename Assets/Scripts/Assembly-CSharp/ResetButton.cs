using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ResetButton : MonoBehaviour
{
	public Text buttonText;

	public virtual void DeleteAllPrefs(bool recreateSupplies)
	{
		PlayerPrefs.DeleteAll();
		buttonText.text = "RESET!";
		if (recreateSupplies)
		{
			Supplies.SetStartingAmounts();
			Currency.SetStartingAmounts();
		}
	}

	public virtual void DeleteSupplies()
	{
		PlayerPrefsHelper.SetInt("ITEM_" + Supplies.OXYGEN, 0);
		PlayerPrefsHelper.SetInt("ITEM_" + Supplies.GRIP, 0);
		PlayerPrefsHelper.SetInt("ITEM_" + Supplies.CHALK, 0);
		PlayerPrefsHelper.SetInt("ITEM_" + Supplies.PROTEIN, 0);
		PlayerPrefsHelper.SetInt("ITEM_" + Supplies.DRINK, 0);
	}

	public virtual void DeleteCustomItems()
	{
		for (int i = 0; i < 100; i++)
		{
			PlayerPrefsHelper.SetInt(CustomItem.OWNED_KEY + CustomItems.BALL + "_" + i, 0);
			PlayerPrefsHelper.SetInt(CustomItem.OWNED_KEY + CustomItems.JERSEY + "_" + i, 0);
			PlayerPrefsHelper.SetInt(CustomItem.OWNED_KEY + CustomItems.ARM_BAND + "_" + i, 0);
			PlayerPrefsHelper.SetInt(CustomItem.OWNED_KEY + CustomItems.PANTS + "_" + i, 0);
			PlayerPrefsHelper.SetInt(CustomItem.OWNED_KEY + CustomItems.SHOES + "_" + i, 0);
		}
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + CustomItems.BALL + "_ACTIVE", 0);
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + CustomItems.JERSEY + "_ACTIVE", 0);
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + CustomItems.ARM_BAND + "_ACTIVE", 0);
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + CustomItems.PANTS + "_ACTIVE", 0);
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + CustomItems.SHOES + "_ACTIVE", 0);
		CustomItems customItems = (CustomItems)GameObject.Find("CustomItems").GetComponent(typeof(CustomItems));
		customItems.InstantiateCustomItems();
	}

	public virtual void ClearRemoveAdsPurchase()
	{
		PlayerPrefsHelper.SetInt("IAP_ADS_REMOVED", 0);
		//PlayFabManager.Instance().SetUserDataForKey("IAP_ADS_REMOVED", 0);
	}

	public virtual void ClearPurchases()
	{
		PlayerPrefsHelper.SetInt("NUM_PURCHASES", 0);
		//PlayFabManager.Instance().SetUserDataForKey("IAP_ADS_REMOVED", 0);
	}

	public virtual void ResetLbVersion()
	{
		PlayerPrefsHelper.SetInt("LB_VERSION", 0);
		//PlayFabManager.Instance().SetUserDataForKey("LB_VERSION", 0);
	}

	public virtual void IncrementLbVersion()
	{
		PlayerPrefsHelper.SetInt("LB_VERSION", PlayerPrefs.GetInt("LB_VERSION") + 1);
		//PlayFabManager.Instance().SetUserDataForKey("LB_VERSION", PlayerPrefs.GetInt("LB_VERSION"));
	}

	public virtual void ResetLeaderboardEntriesRemaining()
	{
		PlayerPrefsHelper.SetInt("ENTRIES_REMAINING", 2);
	}
}
