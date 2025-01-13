using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
	public class FPSDisplay : MonoBehaviour
	{
		[Tooltip("Text component to display the FPS.")]
		public Text FPS;

		private float deltaTime;

		private float elapsedTime;

		private float msec;

		private float fps;

		private const string wait = "<i>...calculating <b>FPS</b>...</i>";

		private const string red = "<color=#E57373><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>";

		private const string orange = "<color=#FFB74D><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>";

		private const string green = "<color=#81C784><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>";

		public void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1f)
			{
				if (Time.frameCount % 3 == 0 && FPS != null)
				{
					msec = deltaTime * 1000f;
					fps = 1f / deltaTime;
					if (fps < 15f)
					{
						FPS.text = string.Format("<color=#E57373><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>", fps, msec);
					}
					else if (fps < 29f)
					{
						FPS.text = string.Format("<color=#FFB74D><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>", fps, msec);
					}
					else
					{
						FPS.text = string.Format("<color=#81C784><b>FPS: {0:0.}</b> ({1:0.0} ms)</color>", fps, msec);
					}
				}
			}
			else
			{
				FPS.text = "<i>...calculating <b>FPS</b>...</i>";
			}
		}
	}
}
