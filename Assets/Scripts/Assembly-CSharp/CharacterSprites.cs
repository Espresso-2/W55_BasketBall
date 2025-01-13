using System;
using UnityEngine;

[Serializable]
public class CharacterSprites : MonoBehaviour
{
	public Sprite[] ballFills;

	public Sprite[] p1HairBehind;

	public Sprite[] p1Head;

	public Sprite[] p1HeadOutline;

	public Sprite[] p1Mouth;

	public Sprite[] p1Face;

	public Sprite[] p1Hair;

	public Sprite[] p1Eyes;

	public Sprite[] p1Nose;

	public Sprite[] p1Brows;

	public Sprite[] p1FemaleHairBehind;

	public Sprite[] p1FemaleHead;

	public Sprite[] p1FemaleHeadOutline;

	public Sprite[] p1FemaleMouth;

	public Sprite[] p1FemaleFace;

	public Sprite[] p1FemaleHair;

	public Sprite[] p1FemaleEyes;

	public Sprite[] p1FemaleNose;

	public Sprite[] p1FemaleBrows;

	public Sprite[] p1BackupHairBehind;

	public Sprite[] p1BackupHead;

	public Sprite[] p1BackupHeadOutline;

	public Sprite[] p1BackupMouth;

	public Sprite[] p1BackupFace;

	public Sprite[] p1BackupHair;

	public Sprite[] p1BackupEyes;

	public Sprite[] p1BackupNose;

	public Sprite[] p1BackupBrows;

	public Sprite[] p1BackupFemaleHairBehind;

	public Sprite[] p1BackupFemaleHead;

	public Sprite[] p1BackupFemaleHeadOutline;

	public Sprite[] p1BackupFemaleMouth;

	public Sprite[] p1BackupFemaleFace;

	public Sprite[] p1BackupFemaleHair;

	public Sprite[] p1BackupFemaleEyes;

	public Sprite[] p1BackupFemaleNose;

	public Sprite[] p1BackupFemaleBrows;

	public Sprite[] p2HairBehind;

	public Sprite[] p2Head;

	public Sprite[] p2HeadOutline;

	public Sprite[] p2Mouth;

	public Sprite[] p2Face;

	public Sprite[] p2Hair;

	public Sprite[] p2Eyes;

	public Sprite[] p2Nose;

	public Sprite[] p2Brows;

	public Sprite[] p2FemaleHairBehind;

	public Sprite[] p2FemaleHead;

	public Sprite[] p2FemaleHeadOutline;

	public Sprite[] p2FemaleMouth;

	public Sprite[] p2FemaleFace;

	public Sprite[] p2FemaleHair;

	public Sprite[] p2FemaleEyes;

	public Sprite[] p2FemaleNose;

	public Sprite[] p2FemaleBrows;

	public Sprite[] hats;

	public Color[] p1HeadColors;

	public Color[] p1MouthColors;

	public Color[] p1HairColors;

	public Color[] p1EyesColors;

	public Color[] p1BrowsColors;

	public Color[] p1FemaleHeadColors;

	public Color[] p1FemaleMouthColors;

	public Color[] p1FemaleHairColors;

	public Color[] p1FemaleEyesColors;

	public Color[] p1FemaleBrowsColors;

	public Color[] p1BackupHeadColors;

	public Color[] p1BackupMouthColors;

	public Color[] p1BackupHairColors;

	public Color[] p1BackupEyesColors;

	public Color[] p1BackupBrowsColors;

	public Color[] p1BackupFemaleHeadColors;

	public Color[] p1BackupFemaleMouthColors;

	public Color[] p1BackupFemaleHairColors;

	public Color[] p1BackupFemaleEyesColors;

	public Color[] p1BackupFemaleBrowsColors;

	public Color[] p2HeadColors;

	public Color[] p2MouthColors;

	public Color[] p2HairColors;

	public Color[] p2EyesColors;

	public Color[] p2BrowsColors;

	public Color[] p2FemaleHeadColors;

	public Color[] p2FemaleMouthColors;

	public Color[] p2FemaleHairColors;

	public Color[] p2FemaleEyesColors;

	public Color[] p2FemaleBrowsColors;

	public Color[] p1SkinTones;

	public Color[] p1SkinTonesFemale;

	public Color[] p1BackupSkinTones;

	public Color[] p1BackupSkinTonesFemale;

	public Color[] p2SkinTones;

	public Color[] p2SkinTonesFemale;

	public Color[] customSkinTones;

	public Color[] customHairColors;

	public Color[] customEyesColors;

	public Sprite jersey;

	public Sprite jerseyFemale;

	public Sprite[] jerseyGraphics;

	public Sprite[] jerseyGraphicsFemale;

	public Color[] clothingColors;

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public virtual void Start()
	{
		if (GameObject.FindGameObjectsWithTag("CharacterSprites").Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public virtual void Update()
	{
	}
}
