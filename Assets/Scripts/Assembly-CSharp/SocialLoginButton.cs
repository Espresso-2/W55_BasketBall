using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class SocialLoginButton : MonoBehaviour
{
	public Localize label;

	public Button button;

	private GameSounds gameSounds;

	public void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public void OnEnable()
	{
		button.gameObject.SetActive(true);
		if (SocialPlatform.Instance.IsLoggedIn())
		{
			label.SetTerm("LOGOUT OF", null);
		}
		else
		{
			label.SetTerm("LOG INTO", null);
		}
	}

	public void OnClick()
	{
		gameSounds.Play_select();
		button.gameObject.SetActive(false);
		StartCoroutine(Login());
	}

	private IEnumerator Login()
	{
		yield return new WaitForSeconds(0.25f);
		if (SocialPlatform.Instance.IsLoggedIn())
		{
			PlayerPrefsHelper.SetInt("USER_LOGGED_OUT_OF_SOCIALPLATFORM", 1);
			SocialPlatform.Instance.LogOut();
		}
		else
		{
			PlayerPrefsHelper.SetInt("USER_LOGGED_OUT_OF_SOCIALPLATFORM", 0);
			SocialPlatform.Instance.LogIn();
		}
	}
}
