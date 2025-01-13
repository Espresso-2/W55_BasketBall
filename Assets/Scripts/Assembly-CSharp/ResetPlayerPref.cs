using UnityEngine;

public class ResetPlayerPref : MonoBehaviour
{
	public string playerPref;

	public void OnClick()
	{
		PlayerPrefs.SetInt(playerPref, 0);
		PlayerPrefs.SetString(playerPref, string.Empty);
	}
}
