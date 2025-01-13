using System;
using UnityEngine;

[Serializable]
public class ModalDialog : MonoBehaviour
{
	public GameObject[] objectsToHideWhenShown;

	public GameObject[] inModalObjectsToHideWhenShown;

	private static int numModalsShowing;

	public virtual void OnEnable()
	{
		numModalsShowing++;
		ToggleObjects(objectsToHideWhenShown, false);
		if (numModalsShowing > 1)
		{
			ToggleObjects(inModalObjectsToHideWhenShown, false);
		}
	}

	public virtual void OnDisable()
	{
		numModalsShowing--;
		if (numModalsShowing == 0)
		{
			ToggleObjects(objectsToHideWhenShown, true);
		}
		ToggleObjects(inModalObjectsToHideWhenShown, true);
	}

	public virtual void ToggleObjects(GameObject[] objects, bool show)
	{
		if (objects == null)
		{
			return;
		}
		foreach (GameObject gameObject in objects)
		{
			if (gameObject != null)
			{
				try
				{
					gameObject.SetActive(show);
				}
				catch (Exception ex)
				{
					Debug.Log("Could not hide object: " + ex.ToString());
				}
			}
		}
	}
}
