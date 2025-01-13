using UnityEngine;
using UnityEngine.UI;

public class ArenaSkinController : MonoBehaviour
{
	public static ArenaSkinController Instance;

	public ArenaSkin CurrentArenaSkin;

	public Transform BkgRoot;

	[Header("Court Elements")]
	public SpriteRenderer Floor;

	public SpriteRenderer FloorDetailUnder;

	public SpriteRenderer FloorDetailOver;

	public SpriteRenderer ThreePointArc_Player;

	public SpriteRenderer ThreePointArc_Enemy;

	public SpriteRenderer SideOfCourt;

	public SpriteRenderer ThreePointArcFill_Player;

	public SpriteRenderer ThreePointArcFill_Enemy;

	public SpriteRenderer CenterCircleFill_Right;

	public SpriteRenderer CenterCircleFill_Left;

	public SpriteRenderer FloorLogo;

	public SpriteRenderer FloorHalfCourtLine;

	public SpriteRenderer[] FloorBaselines;

	public SpriteRenderer[] FloorSidelines;

	[Header("Hoop Elements")]
	public SpriteRenderer[] BackboardStands;

	public SpriteRenderer[] Backboards;

	public SpriteRenderer[] BackboardOutlines;

	public SpriteRenderer[] Rectangles;

	public SpriteRenderer BackboardGraphic_Player;

	public SpriteRenderer BackboardGraphicOutline_Player;

	public SpriteRenderer BackboardGraphic_Enemy;

	public SpriteRenderer BackboardGraphicOutline_Enemy;

	[Header("Rim Elements")]
	public SpriteRenderer[] RimFronts;

	public SpriteRenderer[] RimBehinds;

	public SpriteRenderer[] RimOutlines;

	public SpriteRenderer[] RimConnectors;

	[Header("Net Elements")]
	public SpriteRenderer[] Nets;

	[Header("Scoreboard Elements")]
	public Image Scoreboard;

	private void Awake()
	{
		ArenaSkinController[] array = Object.FindObjectsOfType<ArenaSkinController>();
		if (array.Length > 1)
		{
			Object.DestroyImmediate(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void UpdateArenaSkin(int SkinID)
	{
		ArenaSkin arenaSkin = ArenaSkinUtilities.LoadSkinFromIndex(SkinID);
		if (arenaSkin != null)
		{
			Debug.Log("Applying Skin: " + arenaSkin.Description);
			UpdateArenaSkin(arenaSkin);
		}
		else
		{
			Debug.Log("Can't find skin at index: " + SkinID);
		}
	}

	public void UpdateArenaSkin(ArenaSkin skin, bool updateBackground = true)
	{
		if (skin == null)
		{
			Debug.LogError("Can't Find Skin!");
			return;
		}
		if (updateBackground)
		{
			UpdateBackgrounds(skin);
		}
		UpdateCourt(skin);
		UpdateHoops(skin);
		UpdateRims(skin);
		UpdateNets(skin);
		UpdateScoreboard(skin);
	}

	private void UpdateBackgrounds(ArenaSkin skin)
	{
		foreach (Transform item in BkgRoot)
		{
		}
		for (int num = BkgRoot.childCount - 1; num >= 0; num--)
		{
			Object.DestroyImmediate(BkgRoot.GetChild(num).gameObject);
		}
		if (skin.BkgPrefab == null)
		{
			Debug.Log("No Background Set");
			return;
		}
		GameObject gameObject = null;
		if (gameObject == null)
		{
			gameObject = Object.Instantiate(skin.BkgPrefab);
		}
		gameObject.transform.parent = BkgRoot;
	}

	private void UpdateCourt(ArenaSkin skin)
	{
		if ((bool)Floor)
		{
			skin.Floor.Apply(Floor);
		}
		if ((bool)FloorDetailUnder)
		{
			skin.FloorDetailUnder.Apply(FloorDetailUnder);
		}
		if ((bool)FloorDetailOver)
		{
			skin.FloorDetailOver.Apply(FloorDetailOver);
		}
		if ((bool)ThreePointArc_Player)
		{
			skin.ThreePointArc_Enemy.Apply(ThreePointArc_Enemy);
			skin.ThreePointArc_Player.Apply(ThreePointArc_Player);
		}
		if ((bool)FloorLogo)
		{
			skin.FloorLogo.Apply(FloorLogo);
		}
		if ((bool)ThreePointArcFill_Enemy)
		{
			skin.ThreePointArcFill_Enemy.Apply(ThreePointArcFill_Enemy);
		}
		if ((bool)ThreePointArcFill_Player)
		{
			skin.ThreePointArcFill_Player.Apply(ThreePointArcFill_Player);
		}
		if ((bool)CenterCircleFill_Left)
		{
			skin.CenterCircleFill_Left.Apply(CenterCircleFill_Left);
		}
		if ((bool)CenterCircleFill_Right)
		{
			skin.CenterCircleFill_Right.Apply(CenterCircleFill_Right);
		}
		if ((bool)SideOfCourt)
		{
			skin.SideCourt.Apply(SideOfCourt);
		}
		Color spriteColor = skin.FloorBaseline.SpriteColor;
		ApplySkinSpriteToArray(FloorBaselines, skin.FloorBaseline);
		skin.FloorSideline.SpriteColor = spriteColor;
		ApplySkinSpriteToArray(FloorSidelines, skin.FloorSideline);
		if ((bool)FloorHalfCourtLine)
		{
			FloorHalfCourtLine.sprite = skin.FloorHalfCourtLine.SpriteImage;
			FloorHalfCourtLine.color = spriteColor;
		}
	}

	private void UpdateHoops(ArenaSkin skin)
	{
		ApplySkinSpriteToArray(Backboards, skin.Backboard);
		ApplySkinSpriteToArray(BackboardOutlines, skin.BackboardOutline);
		ApplySkinSpriteToArray(Rectangles, skin.Rectangle);
		ApplySkinSpriteToArray(BackboardStands, skin.BackboardStand);
		if ((bool)BackboardGraphic_Player)
		{
			skin.BackboardGraphic_Player.Apply(BackboardGraphic_Player);
		}
		if ((bool)BackboardGraphicOutline_Player)
		{
			skin.BackboardGraphicOutline_Player.Apply(BackboardGraphicOutline_Player);
		}
		if ((bool)BackboardGraphic_Enemy)
		{
			skin.BackboardGraphic_Enemy.Apply(BackboardGraphic_Enemy);
		}
		if ((bool)BackboardGraphicOutline_Enemy)
		{
			skin.BackboardGraphicOutline_Enemy.Apply(BackboardGraphicOutline_Enemy);
		}
	}

	private void UpdateRims(ArenaSkin skin)
	{
		ApplySkinSpriteToArray(RimFronts, skin.RimFront);
		ApplySkinSpriteToArray(RimBehinds, skin.RimBehind);
		ApplySkinSpriteToArray(RimOutlines, skin.RimOutline);
		ApplySkinSpriteToArray(RimConnectors, skin.RimConnector);
	}

	private void UpdateScoreboard(ArenaSkin skin)
	{
		if ((bool)Scoreboard)
		{
			Scoreboard.sprite = skin.Scoreboard.SpriteImage;
			Scoreboard.color = skin.Scoreboard.SpriteColor;
		}
	}

	private void UpdateNets(ArenaSkin skin)
	{
		ApplySkinSpriteToArray(Nets, skin.NetBase);
	}

	private void ApplySkinSpriteToArray(SpriteRenderer[] spriteArray, SkinSprite skinSprite)
	{
		if (spriteArray != null)
		{
			foreach (SpriteRenderer target in spriteArray)
			{
				skinSprite.Apply(target);
			}
		}
	}
}
