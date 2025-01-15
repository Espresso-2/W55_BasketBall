/*
using System;
using UnityEngine;

public class iOSHapticFeedback : MonoBehaviour
{
	[Serializable]
	public class iOSFeedbackTypeSettings
	{
		public bool SelectionChange = true;

		public bool ImpactLight = true;

		public bool ImpactMedium = true;

		public bool ImpactHeavy = true;

		public bool NotificationSuccess = true;

		public bool NotificationWarning = true;

		public bool NotificationFailure = true;

		public bool Notifications
		{
			get
			{
				return NotificationSuccess || NotificationWarning || NotificationFailure;
			}
		}
	}

	public enum iOSFeedbackType
	{
		SelectionChange = 0,
		ImpactLight = 1,
		ImpactMedium = 2,
		ImpactHeavy = 3,
		Success = 4,
		Warning = 5,
		Failure = 6,
		None = 7
	}

	private static iOSHapticFeedback _instance;

	public iOSFeedbackTypeSettings usedFeedbackTypes = new iOSFeedbackTypeSettings();

	private bool feedbackGeneratorsSetUp;

	public bool debug = true;

	public static iOSHapticFeedback Instance
	{
		get
		{
			if (!_instance)
			{
				Debug.LogWarning("No iOS Haptic Feedback instance available. Creating one.");
				GameObject gameObject = new GameObject("iOS Haptic Feedback");
				_instance = gameObject.AddComponent<iOSHapticFeedback>();
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if ((bool)_instance)
		{
			Debug.LogWarning("There is already an instance of iOSHapticFeedback.");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_instance = this;
		for (int i = 0; i < 5; i++)
		{
			if (FeedbackIdSet(i))
			{
				InstantiateFeedbackGenerator(i);
			}
		}
		feedbackGeneratorsSetUp = true;
	}

	protected void OnDestroy()
	{
		if (!feedbackGeneratorsSetUp)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (FeedbackIdSet(i))
			{
				ReleaseFeedbackGenerator(i);
			}
		}
	}

	protected bool FeedbackIdSet(int id)
	{
		return (id == 0 && usedFeedbackTypes.SelectionChange) || (id == 1 && usedFeedbackTypes.ImpactLight) || (id == 2 && usedFeedbackTypes.ImpactMedium) || (id == 3 && usedFeedbackTypes.ImpactHeavy) || ((id == 4 || id == 5 || id == 6) && usedFeedbackTypes.Notifications);
	}

	private void _instantiateFeedbackGenerator(int id)
	{
	}

	private void _prepareFeedbackGenerator(int id)
	{
	}

	private void _triggerFeedbackGenerator(int id, bool advanced)
	{
	}

	private void _releaseFeedbackGenerator(int id)
	{
	}

	protected void InstantiateFeedbackGenerator(int id)
	{
		if (debug)
		{
			Debug.Log("Instantiate iOS feedback generator " + (iOSFeedbackType)id);
		}
		_instantiateFeedbackGenerator(id);
	}

	protected void PrepareFeedbackGenerator(int id)
	{
		if (debug)
		{
			Debug.Log("Prepare iOS feedback generator " + (iOSFeedbackType)id);
		}
		_prepareFeedbackGenerator(id);
	}

	protected void TriggerFeedbackGenerator(int id, bool advanced)
	{
		if (debug)
		{
			Debug.Log(string.Concat("Trigger iOS feedback generator ", (iOSFeedbackType)id, ", advanced mode: ", advanced));
		}
		_triggerFeedbackGenerator(id, advanced);
	}

	protected void ReleaseFeedbackGenerator(int id)
	{
		if (debug)
		{
			Debug.Log("Release iOS feedback generator " + (iOSFeedbackType)id);
		}
		_releaseFeedbackGenerator(id);
	}

	public virtual void Trigger(iOSFeedbackType feedbackType)
	{
		if (FeedbackIdSet((int)feedbackType))
		{
			TriggerFeedbackGenerator((int)feedbackType, false);
		}
		else
		{
			Debug.LogError("You cannot trigger a feedback generator without instantiating it first");
		}
	}

	public bool IsSupported()
	{
		return false;
	}
}
*/
