using System;
using System.Collections;
using Moments;
using UnityEngine;
using UnityEngine.UI;

public class ReplayCamera : MonoBehaviour
{
	public Camera cam;

	private Recorder recorder;

	private BasketballReplayRecorder bballReplayRecorder;

	private RenderTexture[] recordedRenderTextures;

	private RenderTexture replayTexture;

	private float replaySpeed = 2f;

	private float delayBetweenFrames;

	private float frameTimer;

	private int currentFrame;

	public Color replayBorderColor;

	public float replayBorderSize;

	private Rect replayRect;

	private Rect replayBorderRect;

	private bool savingGif;

	private float savingGifProgFreq = 0.1f;

	private float savingGifProgTimer = 1f;

	private float savingGifProgBonusNum = 1f;

	public GameObject saveGifButton;

	public RectTransform savingGifProgressBar;

	public RectTransform savingGifMsgBox;

	public Text savingGifResult;

	public GameObject openPhotosButton;

	public GameObject cancelButton;

	public static string gifFilePath;

	private GameObject gameSounds;

	private bool calledSaveGifCompleted;

	private void Start()
	{
		gameSounds = GameObject.Find("GameSounds");
		GameObject gameObject = GameObject.Find("RecordCamera");
		if (gameObject != null)
		{
			recorder = gameObject.GetComponent<Recorder>();
			bballReplayRecorder = gameObject.GetComponent<BasketballReplayRecorder>();
		}
		if (recorder != null && recorder.RenderTextureFrames != null)
		{
			recordedRenderTextures = recorder.RenderTextureFrames.ToArray();
			delayBetweenFrames = 0.06f;
			float num = 0.65f;
			float num2 = 0.175f;
			float num3 = 0.08f;
			if (DtUtils.IsSuperWideScreenDevice())
			{
				num = 0.55f;
				num2 = 0.225f;
				num3 = 0.105f;
			}
			replayRect = new Rect((float)Screen.width * num2, (float)Screen.height * num3, (float)Screen.width * num, (float)Screen.height * num);
			replayBorderRect = new Rect(replayRect.x - replayBorderSize, replayRect.y - replayBorderSize, replayRect.width + replayBorderSize * 2f, replayRect.height + replayBorderSize * 2f);
			openPhotosButton.SetActive(false);
			cancelButton.SetActive(false);
			savingGifResult.text = string.Empty;
		}
	}

	private void Update()
	{
		if (savingGif)
		{
			if (bballReplayRecorder.savingCompleted)
			{
				if (!calledSaveGifCompleted)
				{
					SaveGifCompleted();
				}
			}
			else if (savingGifProgTimer >= savingGifProgFreq)
			{
				if (savingGifProgBonusNum < 5f)
				{
					savingGifProgBonusNum += 0.5f;
				}
				float num = Mathf.Round(bballReplayRecorder.m_Progress + savingGifProgBonusNum);
				if (num > 100f)
				{
					num = 100f;
				}
				savingGifProgTimer = 0f;
				Vector2 sizeDelta = new Vector2(savingGifMsgBox.rect.width * (0f - (1f - num / 100f)), savingGifProgressBar.sizeDelta.y);
				savingGifProgressBar.sizeDelta = sizeDelta;
			}
			else
			{
				savingGifProgTimer += Time.deltaTime;
			}
		}
		frameTimer += Time.deltaTime;
	}

	public void SaveGif()
	{
		//FlurryAnalytics.Instance().LogEvent("GIF_SAVE_INITIATED");
		gameSounds.SendMessage("Play_select");
		if (calledSaveGifCompleted)
		{
			ShowNativeShare();
			return;
		}
		saveGifButton.SetActive(false);
		savingGifMsgBox.gameObject.SetActive(true);
		savingGif = true;
		cancelButton.SetActive(true);
		try
		{
			bballReplayRecorder.SaveGif();
		}
		catch (Exception ex)
		{
			Debug.Log("error calling ScreenshotManager.SaveScreenshot() error: " + ex.ToString());
			savingGifResult.text = "ERROR: CONTACT SUPPORT FOR HELP";
			savingGifMsgBox.gameObject.SetActive(false);
			cancelButton.SetActive(false);
			//FlurryAnalytics.Instance().LogEvent("GIF_SAVE_ERROR");
		}
	}

	private void RequestAndroidWritePermission()
	{
		if (Application.platform == RuntimePlatform.Android && !AndroidPermissionsManager.IsPermissionGranted("android.permission.WRITE_EXTERNAL_STORAGE"))
		{
			AndroidPermissionsManager.RequestPermission(new string[1] { "android.permission.WRITE_EXTERNAL_STORAGE" }, new AndroidPermissionCallback(delegate
			{
			}, delegate
			{
			}));
		}
	}

	private void SaveGifCompleted()
	{
		calledSaveGifCompleted = true;
		Vector2 sizeDelta = new Vector2(savingGifMsgBox.rect.width * -0f, savingGifProgressBar.sizeDelta.y);
		savingGifProgressBar.sizeDelta = sizeDelta;
		//FlurryAnalytics.Instance().LogEvent("GIF_ADD_TO_PHOTOS");
		gifFilePath = bballReplayRecorder.m_LastFile;
		Debug.Log("GIF LOCATION: " + gifFilePath);
		try
		{
			gameSounds.SendMessage("Play_coin_glow");
			savingGifResult.text = string.Empty;
			savingGif = false;
			cancelButton.SetActive(false);
			savingGifMsgBox.gameObject.SetActive(false);
			ShowNativeShare();
		}
		catch (Exception ex)
		{
			Debug.Log("error preparing gif  error: " + ex.ToString());
			savingGifResult.text = "ERROR! Contact support for help.";
			savingGifMsgBox.gameObject.SetActive(false);
			cancelButton.SetActive(false);
			//FlurryAnalytics.Instance().LogEvent("GIF_ADD_TO_PHOTOS_ERROR");
		}
	}

	public void ShowNativeShare()
	{
		Debug.Log("Try to share Basketball Battle animated gif -->");
		NativeShare nativeShare = new NativeShare();
		nativeShare.SetSubject("Game winner!");
		nativeShare.SetText("#BasketballBattle http://bit.ly/BballBattle");
		nativeShare.AddFile(gifFilePath);
		nativeShare.SetTitle("Share GIF!");
		nativeShare.Share();
		saveGifButton.SetActive(true);
		Debug.Log("<--Code for animated gif finished being called");
	}

	private void ScreenshotSaved(string path)
	{
		Debug.Log("Screenshot finished saving to " + path);
		//FlurryAnalytics.Instance().LogEvent("GIF_ADD_TO_PHOTOS_COMPLETED");
		StartCoroutine(ShowScreenshotSavedCompleted());
	}

	private IEnumerator ShowScreenshotSavedCompleted()
	{
		yield return new WaitForSeconds(2f);
		savingGifMsgBox.gameObject.SetActive(false);
		gameSounds.SendMessage("Play_coin_glow");
		savingGifResult.text = "ANIMATED GIF SAVED TO YOUR PHOTOS!";
		openPhotosButton.SetActive(true);
		savingGif = false;
		cancelButton.SetActive(false);
		savingGifMsgBox.gameObject.SetActive(false);
	}

	public void CancelSave()
	{
		gameSounds.SendMessage("Play_select");
		savingGifResult.text = "CANCELLED Preparing GIF";
		cancelButton.SetActive(false);
		savingGifMsgBox.gameObject.SetActive(false);
		savingGif = false;
	}

	public void OpenUserPhotos()
	{
		gameSounds.SendMessage("Play_select");
		try
		{
			Debug.Log("OpendAndroidGallery?");
			OpenAndroidGallery();
		}
		catch (Exception ex)
		{
			Debug.Log("error calling OpenAndroidGallery() error: " + ex.ToString());
			savingGifResult.text = "UNABLE TO OPEN PHOTOS\nOPEN PHOTOS APP TO SEE YOUR GIF!";
		}
	}

	private void OnGUI()
	{
		if (recordedRenderTextures == null || recordedRenderTextures.Length <= 0)
		{
			return;
		}
		if (frameTimer > delayBetweenFrames)
		{
			frameTimer = 0f;
			replayTexture = recordedRenderTextures[currentFrame];
			currentFrame++;
			if (currentFrame >= recordedRenderTextures.Length)
			{
				currentFrame = 0;
			}
		}
		if (replayTexture != null)
		{
			DrawQuad(replayBorderRect, replayBorderColor);
			GUI.DrawTexture(replayRect, replayTexture, ScaleMode.StretchToFill, false, 1f);
		}
	}

	private void DrawQuad(Rect position, Color color)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, color);
		texture2D.Apply();
		GUI.skin.box.normal.background = texture2D;
		GUI.Box(position, GUIContent.none);
	}

	private void OpenAndroidGallery()
	{
		Debug.Log(" OpenAndroidGallery()");
		Debug.Log(" Instantiate the class()");
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
		androidJavaObject.Call<AndroidJavaObject>("setAction", new object[1] { androidJavaClass.GetStatic<string>("ACTION_VIEW") });
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
		AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1] { "content://media/internal/images/media" });
		androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
		{
			androidJavaClass.GetStatic<string>("EXTRA_STREAM"),
			androidJavaObject2
		});
		androidJavaObject.Call<AndroidJavaObject>("setType", new object[1] { "image/jpeg" });
		AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject @static = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log(" TRY TO START THE ACTIVITY");
		@static.Call("startActivity", androidJavaObject);
		Debug.Log(" WAS THE ACTIVITY STARTED?");
	}

	public float GetiOSVersion()
	{
		return 0f;
	}
}
