using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerPrefToggle : MonoBehaviour
{
	public Text text;

	public string pref;

	public int numOptions;

	public PlayerPrefToggle()
	{
		numOptions = 1;
	}

	public virtual void Start()
	{
		text.text = pref + " = " + PlayerPrefs.GetInt(pref);
	}

	public virtual void OnClick()
	{
		int @int = PlayerPrefs.GetInt(pref);
		@int = ((@int < numOptions) ? (@int + 1) : 0);
		PlayerPrefsHelper.SetInt(pref, @int);
		text.text = pref + " = " + @int;
	}
}
