/*
using System;
using UnityEngine;

[Serializable]
public class GameVibrations : MonoBehaviour
{
	private static GameVibrations instance;

	private bool isMuted;

	public virtual void Awake()
	{
		Debug.Log("GameVibrations.Awake()");
		instance = this;
	}

	public virtual void Start()
	{
		if (PlayerPrefs.GetInt(MuteButton.VIBRATE_OFF_PREF_KEY) == 1)
		{
			isMuted = true;
		}
	}

	public static GameVibrations Instance()
	{
		return instance;
	}

	public virtual void Mute()
	{
		isMuted = true;
	}

	public virtual void UnMute()
	{
		if (PlayerPrefs.GetInt(MuteButton.VIBRATE_OFF_PREF_KEY) == 0)
		{
			isMuted = false;
		}
	}

	public virtual void PlayMadeDunk()
	{
		if (!isMuted)
		{
			Handheld.Vibrate();
		}
	}

	public virtual void PlayEndGame()
	{
		if (!isMuted)
		{
			Handheld.Vibrate();
		}
	}

	public virtual void PlayHitBackBoard()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
		}
	}

	public virtual void PlayTipped()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
		}
	}

	public virtual void PlayBigBlock()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
		}
	}

	public virtual void PlayHitHoop()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
		}
	}

	public virtual void PlayBounce(int bounceNum)
	{
		if (!isMuted)
		{
			switch (bounceNum)
			{
			case 1:
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
				break;
			case 2:
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
				break;
			}
		}
	}

	public virtual void PlayBlockCatch()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
		}
	}

	public virtual void PlayStolen()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
		}
	}

	public virtual void PlayPutInPlayer()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
		}
	}

	public virtual void PlayGotBall()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
		}
	}

	public virtual void PlayMadeThree()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
		}
	}

	public virtual void PlayMadeLayup()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
		}
	}

	public virtual void PlayMadeTwo()
	{
		if (!isMuted)
		{
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
		}
	}
}
*/
