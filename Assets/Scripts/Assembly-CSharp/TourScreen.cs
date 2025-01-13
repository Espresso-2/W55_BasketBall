using System;
using UnityEngine;

[Serializable]
public class TourScreen : MonoBehaviour
{
	public GameObject tournamentPanel;

	public GameObject exitBox;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !tournamentPanel.activeSelf)
		{
			exitBox.SetActive(true);
		}
	}
}
