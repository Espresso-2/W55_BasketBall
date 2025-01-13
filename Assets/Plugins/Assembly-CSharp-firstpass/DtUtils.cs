using UnityEngine;

public class DtUtils : MonoBehaviour
{
	public static bool IsIosDeviceWithiPhoneXStyleScreen()
	{
		bool result = false;
		float num = (float)Screen.width / (float)Screen.height;
		return result;
	}

	public static bool IsWideScreenDevice()
	{
		return (float)Screen.width / (float)Screen.height >= 1.5f;
	}

	public static bool IsSuperWideScreenDevice()
	{
		return (float)Screen.width / (float)Screen.height >= 1.9f;
	}

	public static void SaveToPrefIntOrString(string key, string valStr)
	{
		int result = 0;
		int.TryParse(valStr, out result);
		if (result != 0)
		{
			PlayerPrefs.SetInt(key, result);
		}
		else
		{
			PlayerPrefs.SetString(key, valStr);
		}
	}

	public static bool IsAnIntLargerThanZero(string valStr)
	{
		int result = 0;
		int.TryParse(valStr, out result);
		return result > 0;
	}
}
