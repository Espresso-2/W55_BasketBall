using System;
using UnityEngine;

[Serializable]
public class RestartButton : MonoBehaviour
{
	public GameController gameController;

	public bool isTwoPlayerMode;

	public bool useLastPlayedMode;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnClick()
	{
		GameObject.Find("GameSounds").SendMessage("Play_light_click");
		StartCoroutine(gameController.NewGame());
	}
}
