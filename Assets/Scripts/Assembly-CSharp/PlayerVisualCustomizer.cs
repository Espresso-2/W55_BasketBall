using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerVisualCustomizer : MonoBehaviour
{
	public PlayerVisual playerVisual;

	public PlayerDetails playerDetails;

	private int skinToneNum;

	private int hairColorNum;

	private int browsColorNum;

	private int eyesColorNum;

	private CharacterSprites characterSprites;

	private GameSounds gameSounds;

	public virtual void Awake()
	{
		characterSprites = (CharacterSprites)GameObject.Find("CharacterSprites").GetComponent(typeof(CharacterSprites));
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		skinToneNum = playerDetails.GetPlayer().GetSkinToneColorNum();
		hairColorNum = playerDetails.GetPlayer().GetHairColorNum();
		browsColorNum = playerDetails.GetPlayer().GetBrowsColorNum();
		eyesColorNum = playerDetails.GetPlayer().GetEyesColorNum();
	}

	public virtual void NextSkinTone(bool goBack)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		skinToneNum = NextColor(skinToneNum, characterSprites.customSkinTones, goBack);
		if (skinToneNum == 0)
		{
			color = playerDetails.GetPlayer().GetDefaultSkinToneColor(characterSprites);
			color2 = playerDetails.GetPlayer().GetDefaultHeadColor(characterSprites);
		}
		else
		{
			color = characterSprites.customSkinTones[skinToneNum];
			color2 = characterSprites.customSkinTones[skinToneNum];
		}
		playerVisual.SetSkinTone(color);
		playerVisual.headVisual.SetHead(playerVisual.headVisual.head.sprite, color2);
		playerVisual.headVisual.SetFace(playerVisual.headVisual.face.sprite, color2);
	}

	public virtual void NextHairColor(bool goBack)
	{
		Color color = default(Color);
		hairColorNum = NextColor(hairColorNum, characterSprites.customHairColors, goBack);
		color = ((hairColorNum != 0) ? characterSprites.customHairColors[hairColorNum] : playerDetails.GetPlayer().GetDefaultHairColor(characterSprites));
		playerVisual.headVisual.SetHairBehind(playerVisual.headVisual.hairBehind.sprite, color);
		playerVisual.headVisual.SetHair(playerVisual.headVisual.hair.sprite, color);
	}

	public virtual void NextBrowsColor(bool goBack)
	{
		Color color = default(Color);
		browsColorNum = NextColor(browsColorNum, characterSprites.customHairColors, goBack);
		color = ((browsColorNum != 0) ? characterSprites.customHairColors[browsColorNum] : playerDetails.GetPlayer().GetDefaultBrowsColor(characterSprites));
		playerVisual.headVisual.SetBrows(playerVisual.headVisual.brows.sprite, color);
	}

	public virtual void NextEyesColor(bool goBack)
	{
		Color color = default(Color);
		eyesColorNum = NextColor(eyesColorNum, characterSprites.customEyesColors, goBack);
		color = ((eyesColorNum != 0) ? characterSprites.customEyesColors[eyesColorNum] : playerDetails.GetPlayer().GetDefaultEyesColor(characterSprites));
		playerVisual.headVisual.SetEyes(playerVisual.headVisual.eyes.sprite, color);
	}

	private int NextColor(int colorNum, Color[] colorArray, bool goBack)
	{
		gameSounds.Play_select();
		colorNum = ((!goBack) ? (colorNum + 1) : (colorNum - 1));
		if (colorNum < 0)
		{
			colorNum = colorArray.Length - 1;
		}
		else if (colorNum >= colorArray.Length)
		{
			colorNum = 0;
		}
		return colorNum;
	}

	public virtual void Randomize()
	{
		StartCoroutine(RandomizeRoutine());
	}

	private IEnumerator RandomizeRoutine()
	{
		int tot = 0;
		gameSounds.Play_tap_2();
		int ran4 = UnityEngine.Random.Range(0, 10);
		for (int l = 0; l < ran4; l++)
		{
			yield return new WaitForSeconds(0.02f);
			NextSkinTone(false);
			tot++;
		}
		ran4 = UnityEngine.Random.Range(0, 10);
		for (int l = 0; l < ran4; l++)
		{
			yield return new WaitForSeconds(0.02f);
			NextHairColor(false);
			tot++;
		}
		ran4 = UnityEngine.Random.Range(0, 10);
		for (int l = 0; l < ran4; l++)
		{
			yield return new WaitForSeconds(0.02f);
			NextBrowsColor(false);
			tot++;
		}
		ran4 = 25 - tot;
		for (int l = 0; l < ran4; l++)
		{
			yield return new WaitForSeconds(0.02f);
			NextEyesColor(false);
			tot++;
		}
	}

	public virtual void Reset()
	{
		skinToneNum = 0;
		hairColorNum = 0;
		browsColorNum = 0;
		eyesColorNum = 0;
		NextSkinTone(false);
		NextSkinTone(true);
		NextHairColor(false);
		NextHairColor(true);
		NextBrowsColor(false);
		NextBrowsColor(true);
		NextEyesColor(false);
		NextEyesColor(true);
	}

	public virtual void SaveChanges()
	{
		Player player = playerDetails.GetPlayer();
		player.SaveCustomColors(skinToneNum, hairColorNum, browsColorNum, eyesColorNum);
	}
}
