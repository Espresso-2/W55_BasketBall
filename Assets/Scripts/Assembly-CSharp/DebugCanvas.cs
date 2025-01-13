using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DebugCanvas : MonoBehaviour
{
	public Text debugText;

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void SetText(string text)
	{
		debugText.text = text;
	}
}
