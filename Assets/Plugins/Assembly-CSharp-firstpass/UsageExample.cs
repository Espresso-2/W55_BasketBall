using UnityEngine;

public class UsageExample : MonoBehaviour
{
	private const string STORAGE_PERMISSION = "android.permission.WRITE_EXTERNAL_STORAGE";

	public void OnBrowseGalleryButtonPress()
	{
		if (!CheckPermissions())
		{
			Debug.LogWarning("Missing permission to browse device gallery, please grant the permission first");
		}
		else
		{
			Debug.Log("Browsing Gallery...");
		}
	}

	private bool CheckPermissions()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return true;
		}
		return AndroidPermissionsManager.IsPermissionGranted("android.permission.WRITE_EXTERNAL_STORAGE");
	}

	public void OnGrantButtonPress()
	{
		AndroidPermissionsManager.RequestPermission(new string[1] { "android.permission.WRITE_EXTERNAL_STORAGE" }, new AndroidPermissionCallback(delegate
		{
			OnBrowseGalleryButtonPress();
		}, delegate
		{
		}));
	}
}
