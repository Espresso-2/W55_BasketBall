using UnityEngine;

namespace Crosstales.UI
{
	public class StaticManager : MonoBehaviour
	{
		public void Quit()
		{
			Application.Quit();
		}

		public void OpenCrosstales()
		{
			Application.OpenURL("https://www.crosstales.com");
		}

		public void OpenAssetstore()
		{
			Application.OpenURL("https://goo.gl/qwtXyb");
		}
	}
}
