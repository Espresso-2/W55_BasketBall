using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedWidthCamera : MonoBehaviour
{
	public float screenWidth = 600f;

	public float screenWidthForiPhoneX = 600f;

	public bool onlyFixNonWideScreenDevices;

	public bool fixiPhoneXWidth;

	public GameControls gameControls;

	private float controlsScale = 1f;

	private Camera camera;

	private float size;

	private float screenHeight;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		if ((fixiPhoneXWidth && DtUtils.IsIosDeviceWithiPhoneXStyleScreen()) || DtUtils.IsSuperWideScreenDevice())
		{
			if (PlayerPrefs.GetInt("ZOOM_WIDE_DEVICES_OFF") == 0)
			{
				screenHeight = screenWidthForiPhoneX * ((float)Screen.height / (float)Screen.width);
				size = screenHeight / 200f;
				controlsScale = size / camera.orthographicSize;
				camera.orthographicSize = size;
			}
		}
		else if (!DtUtils.IsWideScreenDevice() || !onlyFixNonWideScreenDevices)
		{
			screenHeight = screenWidth * ((float)Screen.height / (float)Screen.width);
			size = screenHeight / 200f;
			controlsScale = size / camera.orthographicSize;
			camera.orthographicSize = size;
		}
	}

	public void AdjustGameControls()
	{
		if (gameControls != null)
		{
			gameControls.SetScales(controlsScale);
		}
	}
}
