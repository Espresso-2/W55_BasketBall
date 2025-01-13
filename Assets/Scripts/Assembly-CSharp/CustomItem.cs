using System;

[Serializable]
public class CustomItem
{
	public static string OWNED_KEY;

	public int type;

	public int num;

	public int color;

	public int overlaySprite;

	public int overlayColor;

	public int goldValue;

	public bool owned;

	static CustomItem()
	{
		OWNED_KEY = "CUSTOM_ITEM_OWNED_";
	}

	public virtual void Claim()
	{
		owned = true;
		PlayerPrefsHelper.SetInt(OWNED_KEY + type + "_" + num, 1, true);
		PlayerPrefsHelper.SetInt("NEW_CUSTOM_ITEM", 1);
	}

	public virtual void SetToActive(bool saveToPlayFab)
	{
		PlayerPrefsHelper.SetInt("CUSTOM_ITEM_" + type + "_ACTIVE", num, saveToPlayFab);
	}
}
