using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AbilityReqBox : MonoBehaviour
{
	public Text currentText;

	public Text reqText;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void SetText(int c, int r)
	{
		currentText.text = c.ToString();
		reqText.text = r.ToString();
	}
}
