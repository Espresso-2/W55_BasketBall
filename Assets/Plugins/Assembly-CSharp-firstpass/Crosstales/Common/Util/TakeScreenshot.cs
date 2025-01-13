using System;
using UnityEngine;

namespace Crosstales.Common.Util
{
	[DisallowMultipleComponent]
	public class TakeScreenshot : MonoBehaviour
	{
		[Tooltip("Prefix for the generate file names.")]
		public string Prefix = "CT_Screenshot";

		[Tooltip("Factor by which to increase resolution (default: 1).")]
		public int Scale = 1;

		[Tooltip("Key-press to capture the screen (default: F8).")]
		public KeyCode KeyCode = KeyCode.F8;

		private Texture2D texture;

		public void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.transform.root.gameObject);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode))
			{
				Capture();
			}
		}

		public void Capture()
		{
			string text = Prefix + DateTime.Now.ToString("_d-MM-yyyy-HH-mm-ss-f") + ".png";
			ScreenCapture.CaptureScreenshot(text, Scale);
			Debug.Log("Screenshot saved: " + text);
		}
	}
}
