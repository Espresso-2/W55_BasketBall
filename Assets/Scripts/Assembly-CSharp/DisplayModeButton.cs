using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DisplayModeButton : MonoBehaviour
{
	public GameObject onePlayerButton;

	public GameObject twoPlayerButton;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual IEnumerator OnClick()
	{
		GameObject.Find("GameSounds").SendMessage("Play_light_click");
		yield return new WaitForSeconds(0.25f);
		onePlayerButton.SetActive(true);
		twoPlayerButton.SetActive(true);
		base.gameObject.SetActive(false);
	}
}
