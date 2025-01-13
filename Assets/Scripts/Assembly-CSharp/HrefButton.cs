using System;
using UnityEngine;

[Serializable]
public class HrefButton : MonoBehaviour
{
	public string googlePlayHref;

	public string amazHref;

	public string iosHref;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnClick()
	{
		GameObject.Find("GameSounds").SendMessage("Play_light_click");
		string empty = string.Empty;
		empty = ((!IsAmazon()) ? googlePlayHref : amazHref);
		if (empty != null && empty != string.Empty)
		{
			Application.OpenURL(empty);
		}
	}

	public static bool IsAmazon()
	{
		bool result = false;
		string text = SystemInfo.deviceModel.ToLower();
		if (text.Contains("kindle") || text.Contains("amazon"))
		{
			result = true;
		}
		return result;
	}
}
