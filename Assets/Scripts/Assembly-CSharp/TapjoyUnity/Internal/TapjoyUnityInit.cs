using UnityEngine;
using UnityEngine.SceneManagement;

namespace TapjoyUnity.Internal
{
	public sealed class TapjoyUnityInit : MonoBehaviour
	{
		private static bool initialized;

		private void Awake()
		{
			if (!initialized)
			{
				initialized = true;
				ApiBindingAndroid.Install();
				SceneManager.sceneLoaded += delegate(Scene scene, LoadSceneMode mode)
				{
					UnityDependency.OnSceneLoaded(scene, scene.buildIndex, scene.name, scene.path, (int)mode);
				};
				SceneManager.sceneUnloaded += delegate(Scene scene)
				{
					UnityDependency.OnSceneUnloaded(scene, scene.buildIndex, scene.name, scene.path);
				};
				UnityDependency.sceneCount = () => SceneManager.sceneCount;
				UnityDependency.GetSceneAt = (int index) => SceneManager.GetSceneAt(index);
				UnityDependency.ToJson = JsonUtility.ToJson;
			}
		}
	}
}
