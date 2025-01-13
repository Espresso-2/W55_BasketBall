using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TimeoutButton : MonoBehaviour
{
	public Image image;

	public Color hasBallColor;

	public Color noBallColor;

	public Color dehydratedColor;

	private bool hasBall;

	private bool isDehydrated;

	public virtual void Start()
	{
		UpdateColor();
	}

	public virtual void SetHasBall(bool hb)
	{
		hasBall = hb;
		UpdateColor();
	}

	public virtual void SetIsDehydrated(bool d)
	{
		isDehydrated = d;
		UpdateColor();
	}

	private void UpdateColor()
	{
		if (!hasBall)
		{
			image.color = noBallColor;
		}
		else if (isDehydrated)
		{
			image.color = dehydratedColor;
		}
		else
		{
			image.color = hasBallColor;
		}
	}

	public virtual void TakeTimeout()
	{
	}

	public virtual void Update()
	{
	}
}
