using UnityEngine;

public class PlayerPrefsHelper : MonoBehaviour
{
	public static void SetInt(string key, int val, bool submitToPlayFab = false)
	{
		PlayerPrefs.SetInt(key, val);
		if (submitToPlayFab)
		{
			PlayFabManager.Instance().SetUserDataForKey(key, val);
		}
	}

	public static void SetString(string key, string val, bool submitToPlayFab = false)
	{
		PlayerPrefs.SetString(key, val);
		if (submitToPlayFab)
		{
			PlayFabManager.Instance().SetUserDataForKey(key, val);
		}
	}
}
