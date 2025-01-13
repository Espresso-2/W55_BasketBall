using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HeadVisual : MonoBehaviour
{
	public SpriteRenderer hairBehind;

	public SpriteRenderer head;

	public SpriteRenderer headOutline;

	public SpriteRenderer mouth;

	public SpriteRenderer face;

	public SpriteRenderer hair;

	public SpriteRenderer eyes;

	public SpriteRenderer nose;

	public SpriteRenderer brows;

	public SpriteRenderer hat;

	public Image hairBehindUiImage;

	public Image headUiImage;

	public Image headOutlineUiImage;

	public Image mouthUiImage;

	public Image faceUiImage;

	public Image hairUiImage;

	public Image eyesUiImage;

	public Image noseUiImage;

	public Image browsUiImage;

	public virtual void SetHairBehind(Sprite sprite, Color color)
	{
		if (hairBehind != null)
		{
			hairBehind.sprite = sprite;
			hairBehind.color = color;
			return;
		}
		hairBehindUiImage.sprite = sprite;
		if (sprite != null)
		{
			hairBehindUiImage.color = color;
		}
		else
		{
			hairBehindUiImage.color = Color.clear;
		}
	}

	public virtual void SetHead(Sprite sprite, Color color)
	{
		if (head != null)
		{
			head.sprite = sprite;
			head.color = color;
		}
		else
		{
			headUiImage.sprite = sprite;
			headUiImage.color = color;
		}
	}

	public virtual void SetHeadOutline(Sprite sprite, Color color)
	{
		if (headOutline != null)
		{
			headOutline.sprite = sprite;
			headOutline.color = color;
		}
		else
		{
			headOutlineUiImage.sprite = sprite;
			headOutlineUiImage.color = color;
		}
	}

	private void SetMouth(Sprite sprite, Color color)
	{
		if (mouth != null)
		{
			mouth.sprite = sprite;
			mouth.color = color;
			return;
		}
		mouthUiImage.sprite = sprite;
		if (sprite.texture.width == 128)
		{
			float x = 22.5f;
			Vector3 localScale = mouthUiImage.gameObject.transform.localScale;
			localScale.x = x;
			mouthUiImage.gameObject.transform.localScale = localScale;
			float y = 22.5f;
			Vector3 localScale2 = mouthUiImage.gameObject.transform.localScale;
			localScale2.y = y;
			mouthUiImage.gameObject.transform.localScale = localScale2;
		}
		else
		{
			float x2 = 45f;
			Vector3 localScale3 = mouthUiImage.gameObject.transform.localScale;
			localScale3.x = x2;
			mouthUiImage.gameObject.transform.localScale = localScale3;
			float y2 = 45f;
			Vector3 localScale4 = mouthUiImage.gameObject.transform.localScale;
			localScale4.y = y2;
			mouthUiImage.gameObject.transform.localScale = localScale4;
		}
		mouthUiImage.color = color;
	}

	public virtual void SetFace(Sprite sprite, Color color)
	{
		if (face != null)
		{
			face.sprite = sprite;
			face.color = color;
		}
		else
		{
			faceUiImage.sprite = sprite;
			faceUiImage.color = color;
		}
	}

	public virtual void SetHair(Sprite sprite, Color color)
	{
		if (hair != null)
		{
			hair.sprite = sprite;
			hair.color = color;
		}
		else
		{
			hairUiImage.sprite = sprite;
			hairUiImage.color = color;
		}
	}

	public virtual void SetEyes(Sprite sprite, Color color)
	{
		if (eyes != null)
		{
			eyes.sprite = sprite;
			eyes.color = color;
		}
		else
		{
			eyesUiImage.sprite = sprite;
			eyesUiImage.color = color;
		}
	}

	public virtual void SetNose(Sprite sprite, Color color)
	{
		if (nose != null)
		{
			nose.sprite = sprite;
			nose.color = color;
		}
		else
		{
			noseUiImage.sprite = sprite;
			noseUiImage.color = color;
		}
	}

	public virtual void SetBrows(Sprite sprite, Color color)
	{
		if (brows != null)
		{
			brows.sprite = sprite;
			brows.color = color;
		}
		else
		{
			browsUiImage.sprite = sprite;
			browsUiImage.color = color;
		}
	}

	public virtual void SetHat(Sprite sprite, Color color)
	{
		if (hat != null)
		{
			hat.sprite = sprite;
			hat.color = color;
		}
	}

	public virtual void SetVisual(Player p, CharacterSprites cs, int arenaNum)
	{
		Color color = default(Color);
		Sprite sprite = null;
		Sprite sprite2 = null;
		Color headColor = p.GetHeadColor(cs);
		Sprite sprite3 = null;
		Sprite sprite4 = null;
		Sprite sprite5 = null;
		Sprite sprite6 = null;
		Color hairColor = p.GetHairColor(cs);
		Sprite sprite7 = null;
		Color eyesColor = p.GetEyesColor(cs);
		Sprite sprite8 = null;
		Sprite sprite9 = null;
		Color browsColor = p.GetBrowsColor(cs);
		if (p.isP2)
		{
			if (p.isFemale)
			{
				sprite = cs.p2FemaleHairBehind[p.num];
				sprite2 = cs.p2FemaleHead[p.num];
				sprite3 = cs.p2FemaleHeadOutline[p.num];
				sprite4 = cs.p2FemaleMouth[p.num];
				color = cs.p2FemaleMouthColors[p.num];
				sprite5 = cs.p2FemaleFace[p.num];
				sprite6 = cs.p2FemaleHair[p.num];
				sprite7 = cs.p2FemaleEyes[p.num];
				sprite8 = cs.p2FemaleNose[p.num];
				sprite9 = cs.p2FemaleBrows[p.num];
			}
			else
			{
				sprite = cs.p2HairBehind[p.num];
				sprite2 = cs.p2Head[p.num];
				sprite3 = cs.p2HeadOutline[p.num];
				sprite4 = cs.p2Mouth[p.num];
				color = cs.p2MouthColors[p.num];
				sprite5 = cs.p2Face[p.num];
				sprite6 = cs.p2Hair[p.num];
				sprite7 = cs.p2Eyes[p.num];
				sprite8 = cs.p2Nose[p.num];
				sprite9 = cs.p2Brows[p.num];
			}
		}
		else if (p.isBackup)
		{
			if (p.isFemale)
			{
				sprite = cs.p1BackupFemaleHairBehind[p.num];
				sprite2 = cs.p1BackupFemaleHead[p.num];
				sprite3 = cs.p1BackupFemaleHeadOutline[p.num];
				sprite4 = cs.p1BackupFemaleMouth[p.num];
				color = cs.p1BackupFemaleMouthColors[p.num];
				sprite5 = cs.p1BackupFemaleFace[p.num];
				sprite6 = cs.p1BackupFemaleHair[p.num];
				sprite7 = cs.p1BackupFemaleEyes[p.num];
				sprite8 = cs.p1BackupFemaleNose[p.num];
				sprite9 = cs.p1BackupFemaleBrows[p.num];
			}
			else
			{
				sprite = cs.p1BackupHairBehind[p.num];
				sprite2 = cs.p1BackupHead[p.num];
				sprite3 = cs.p1BackupHeadOutline[p.num];
				sprite4 = cs.p1BackupMouth[p.num];
				color = cs.p1BackupMouthColors[p.num];
				sprite5 = cs.p1BackupFace[p.num];
				sprite6 = cs.p1BackupHair[p.num];
				sprite7 = cs.p1BackupEyes[p.num];
				sprite8 = cs.p1BackupNose[p.num];
				sprite9 = cs.p1BackupBrows[p.num];
			}
		}
		else if (p.isFemale)
		{
			sprite = cs.p1FemaleHairBehind[p.num];
			sprite2 = cs.p1FemaleHead[p.num];
			sprite3 = cs.p1FemaleHeadOutline[p.num];
			sprite4 = cs.p1FemaleMouth[p.num];
			color = cs.p1FemaleMouthColors[p.num];
			sprite5 = cs.p1FemaleFace[p.num];
			sprite6 = cs.p1FemaleHair[p.num];
			sprite7 = cs.p1FemaleEyes[p.num];
			sprite8 = cs.p1FemaleNose[p.num];
			sprite9 = cs.p1FemaleBrows[p.num];
		}
		else
		{
			sprite = cs.p1HairBehind[p.num];
			sprite2 = cs.p1Head[p.num];
			sprite3 = cs.p1HeadOutline[p.num];
			sprite4 = cs.p1Mouth[p.num];
			color = cs.p1MouthColors[p.num];
			sprite5 = cs.p1Face[p.num];
			sprite6 = cs.p1Hair[p.num];
			sprite7 = cs.p1Eyes[p.num];
			sprite8 = cs.p1Nose[p.num];
			sprite9 = cs.p1Brows[p.num];
		}
		SetHairBehind(sprite, hairColor);
		SetHead(sprite2, headColor);
		SetHeadOutline(sprite3, Color.white);
		SetMouth(sprite4, color);
		SetFace(sprite5, headColor);
		SetHair(sprite6, hairColor);
		SetEyes(sprite7, eyesColor);
		SetNose(sprite8, Color.white);
		SetBrows(sprite9, browsColor);
		int num = 0;
		if (HolidayItem.GetCurrentHoliday() == HolidayItem.HolidayType.Christmas && (arenaNum == 57 || arenaNum == 58 || arenaNum == 59 || arenaNum == 60))
		{
			num = 1;
		}
		Sprite sprite10 = cs.hats[num];
		SetHat(sprite10, Color.white);
		bool flag = p.num == 0 && !p.isBackup && !p.isFemale;
		if (sprite10 != null && !flag)
		{
			SetHair(null, Color.white);
		}
	}
}
