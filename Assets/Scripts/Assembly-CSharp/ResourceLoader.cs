using System;
using UnityEngine;

[Serializable]
public class ResourceLoader : MonoBehaviour
{
	private Sprite portrait_0;

	private Sprite portrait_1;

	private Sprite portrait_2;

	private Sprite portrait_3;

	private Sprite portrait_4;

	private Sprite portrait_5;

	private Sprite portrait_6;

	private Sprite portrait_7;

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public virtual void Start()
	{
		if (GameObject.FindGameObjectsWithTag("ResourceLoader").Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public virtual Sprite GetPortrait(int num, bool isBackup)
	{
		Sprite sprite = null;
		if (sprite == null)
		{
			sprite = ((!isBackup) ? GetTextureResource("portrait_" + num) : GetTextureResource("portrait_backup_" + num));
		}
		return sprite;
	}

	private Sprite GetTextureResource(string resourceName)
	{
		return (Sprite)Resources.Load(resourceName, typeof(Sprite));
	}
}
