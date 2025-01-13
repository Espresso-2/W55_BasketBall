using System;
using UnityEngine;

[Serializable]
public class SoundPlayer : MonoBehaviour
{
	public virtual void Start()
	{
	}

	public virtual void Play_select()
	{
		GameObject gameObject = GameObject.Find("GameSounds");
		if (gameObject != null)
		{
			((GameSounds)gameObject.GetComponent(typeof(GameSounds))).Play_select();
		}
	}
}
