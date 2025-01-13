using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class NativeAdHolder : MonoBehaviour
{
	public bool isSmall;

	public bool autoLoadAd;

	public Image testImage;

	public Text coachHeadingText;

	public Text headlineText;

	public Text bodyText;

	public Text advertiserText;

	public Text callToActionText;

	public GameObject callToActionButton;

	public Image adChoicesLogo;

	public Image icon;

	public Text priceText;

	public Text storeText;

	public Image[] imageTextures;

	public GameObject[] imageTexturesGameObjects;

	public Image[] stars;

	public void Start()
	{
		if (autoLoadAd)
		{
			ShowAd();
		}
	}

	public void ShowAd()
	{
		Debug.Log("NativeAdHolder.ShowAd()");
		HideDefaultValues();
		if (callToActionButton != null)
		{
			callToActionButton.SetActive(false);
		}
		UnifiedNativeAd unifiedNativeAd = ((!isSmall) ? AdMediation.GetNativeAd() : AdMediation.GetNativeAdSmall());
		if (unifiedNativeAd != null)
		{
			Debug.Log("NativeAdHolder.ShowAd() nativeAd is not null, setup the Native Ad");
			if (coachHeadingText != null)
			{
				coachHeadingText.text = "OUR HALFTIME SPONSOR!";
			}
			Debug.Log("NativeAdHolder.ShowAd() Set string of the headline asset");
			if (headlineText != null)
			{
				headlineText.text = unifiedNativeAd.GetHeadlineText();
			}
			try
			{
				RegisterTheAdObjects(unifiedNativeAd);
			}
			catch (Exception ex)
			{
				Debug.Log("NativeAdHolder.ShowAd() Error happened when registering the native ad objects: " + ex.ToString());
			}
			Debug.Log("NativeAdHolder.ShowAd() Set the icon");
			if (icon != null)
			{
				Texture2D iconTexture = unifiedNativeAd.GetIconTexture();
				if (iconTexture != null)
				{
					icon.sprite = Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
				}
				else
				{
					Debug.Log("NativeAdHolder.ShowAd() iconTexture is null");
					icon.gameObject.SetActive(false);
				}
			}
			if (bodyText != null)
			{
				bodyText.text = string.Empty + unifiedNativeAd.GetBodyText();
			}
			if (advertiserText != null)
			{
				advertiserText.text = string.Empty + unifiedNativeAd.GetAdvertiserText();
			}
			if (callToActionText != null)
			{
				callToActionText.text = string.Empty + unifiedNativeAd.GetCallToActionText();
			}
			if (adChoicesLogo != null)
			{
				Texture2D adChoicesLogoTexture = unifiedNativeAd.GetAdChoicesLogoTexture();
				if (adChoicesLogoTexture != null)
				{
					adChoicesLogo.sprite = Sprite.Create(adChoicesLogoTexture, new Rect(0f, 0f, adChoicesLogoTexture.width, adChoicesLogoTexture.height), new Vector2(0.5f, 0.5f));
				}
				else
				{
					Debug.Log("NativeAdHolder.ShowAd() adChoicesLogoTexture is null");
					adChoicesLogo.gameObject.SetActive(false);
				}
			}
			string price = unifiedNativeAd.GetPrice();
			string text = string.Empty;
			string store = unifiedNativeAd.GetStore();
			if (price != null && price.Length > 0)
			{
				text = ((!(store.ToUpper() == "APP STORE")) ? " ON " : " ON THE ");
			}
			if (storeText != null)
			{
				storeText.text = price + text + store;
			}
			if (priceText != null)
			{
				priceText.text = string.Empty;
			}
			if (unifiedNativeAd.GetImageTextures() != null)
			{
				for (int i = 0; i < unifiedNativeAd.GetImageTextures().Count; i++)
				{
					Texture2D texture2D = unifiedNativeAd.GetImageTextures()[i];
					if (texture2D != null)
					{
						imageTextures[i].color = Color.white;
						imageTextures[i].sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
						Debug.Log("NativeAdHolder.ShowAd() Setting imageTextures[" + i + "].sprite (width: " + texture2D.width + " t2d.height: " + texture2D.height + ")");
					}
					else
					{
						Debug.Log("NativeAdHolder.ShowAd() nativeAd.GetImageTextures()[" + i + "] is null");
					}
				}
			}
			int num = 0;
			for (int j = 0; j < stars.Length; j++)
			{
				GameObject gameObject = stars[j].gameObject;
				gameObject.SetActive(j < num);
			}
			StartCoroutine(ShowCallToActionButton());
			Debug.Log("NativeAdHolder.ShowAd() Finished setting up the native ad");
			return;
		}
		Debug.Log("NativeAdHolder.ShowAd(): nativeAd is null, do not try and show");
		if (false)
		{
			if (callToActionText != null)
			{
				callToActionText.text = "EDITOR Call to action";
			}
			StartCoroutine(ShowCallToActionButton());
		}
		else if (autoLoadAd)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void HideDefaultValues()
	{
		Debug.Log("NativeAdHolder.HideDefaultValues()");
		if (headlineText != null)
		{
			headlineText.text = string.Empty;
		}
		if (bodyText != null)
		{
			bodyText.text = string.Empty;
		}
		if (advertiserText != null)
		{
			advertiserText.text = string.Empty;
		}
		if (callToActionText != null)
		{
			callToActionText.text = string.Empty;
		}
		if (priceText != null)
		{
			priceText.text = string.Empty;
		}
		if (storeText != null)
		{
			storeText.text = string.Empty;
		}
		for (int i = 0; i < stars.Length; i++)
		{
			GameObject gameObject = stars[i].gameObject;
			gameObject.SetActive(false);
		}
	}

	private void RegisterTheAdObjects(UnifiedNativeAd nativeAd)
	{
		Debug.Log("NativeAdHolder.ShowAd() Register all of the objects...");
		if (icon != null && !nativeAd.RegisterIconImageGameObject(icon.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register icon");
		}
		if (storeText != null && !nativeAd.RegisterStoreGameObject(storeText.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register storeText");
		}
		if (bodyText != null && !nativeAd.RegisterBodyTextGameObject(bodyText.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register bodyText");
		}
		if (headlineText != null && !nativeAd.RegisterHeadlineTextGameObject(headlineText.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register headlineText");
		}
		if (callToActionButton != null && !nativeAd.RegisterCallToActionGameObject(callToActionButton))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register callToActionButton");
		}
		if (adChoicesLogo != null && !nativeAd.RegisterAdChoicesLogoGameObject(adChoicesLogo.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register adChoicesLogo");
		}
		if (advertiserText != null && !nativeAd.RegisterAdvertiserTextGameObject(advertiserText.gameObject))
		{
			Debug.Log("NativeAdHolder.ShowAd() Failed to register advertiserText");
		}
		Debug.Log("NativeAdHolder.ShowAd() Register all of the images...");
		List<GameObject> gameObjects = new List<GameObject>(imageTexturesGameObjects);
		nativeAd.RegisterImageGameObjects(gameObjects);
	}

	private IEnumerator ShowCallToActionButton()
	{
		yield return new WaitForSeconds(2f);
		if (callToActionText.text != null && callToActionText.text != string.Empty)
		{
			callToActionButton.SetActive(true);
		}
	}
}
