using System;
using UnityEngine;

[Serializable]
public class PauseButton : MonoBehaviour
{
	public GameObject pauseDialog;

	public GameNoise gameNoise;

	public GameObject startMsg;

	public GameObject halfTimeBox;

	public GameObject quitButton;

	public GameObject confirmForfeitButton;

	public GameControls gameControls;

	public GameController gameController;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	/*public virtual void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			if (!pauseDialog.activeInHierarchy && !halfTimeBox.activeInHierarchy && Time.timeScale > 0f)
			{
				Pause();
			}
		}
		else if (pauseDialog.activeInHierarchy)
		{
			Time.timeScale = 0f;
		}
	}*/

	public virtual void OnClick()
	{
		if (pauseDialog.activeInHierarchy)
		{
			Time.timeScale = gameController.GetGamePlayTimeScale();
			pauseDialog.SetActive(false);
			gameSounds.Play_select();
			gameControls.gameObject.SetActive(true);
			/*AdMediation.HideTopBanner();
			AdMediation.HideCenterBanner();*/
		}
		else
		{
			Pause();
		}
	}

	private void Pause()
	{
		Time.timeScale = 0f;
		pauseDialog.SetActive(true);
		confirmForfeitButton.SetActive(false);
		startMsg.SetActive(false);
		gameControls.gameObject.SetActive(false);
		gameNoise.PauseBgSqueaks();
		/*AdMediation.ShowTjpGamePlayPause();*/
	}
}
