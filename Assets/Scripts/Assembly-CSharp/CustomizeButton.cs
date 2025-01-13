using System;
using UnityEngine;

[Serializable]
public class CustomizeButton : MonoBehaviour
{
	public GameObject newIcon;

	public virtual void OnEnable()
	{
		newIcon.SetActive(PlayerPrefs.GetInt("NEW_CUSTOM_ITEM") > 0 && PlayerPrefs.GetInt("SHOWED_CUSTOMIZE_SCREEN") == 0);
	}
}
