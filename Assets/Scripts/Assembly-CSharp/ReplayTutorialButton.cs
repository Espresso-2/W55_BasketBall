using System;
using UnityEngine;

[Serializable]
public class ReplayTutorialButton : MonoBehaviour
{
	public string scene;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnClick()
	{
		SessionVars instance = SessionVars.GetInstance();
		instance.goToTutorial = true;
		instance.goToScrimmage = false;
		instance.twoPlayerMode = false;
		instance.goToPractice = false;
		Time.timeScale = 1f;
		Application.LoadLevel(scene);
	}
}
