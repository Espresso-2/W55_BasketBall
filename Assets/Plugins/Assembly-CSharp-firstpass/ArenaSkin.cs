using UnityEngine;

[CreateAssetMenu(menuName = "Basketball Battle/ArenaSkin")]
public class ArenaSkin : ScriptableObject
{
	private static Color DefaultColor = Color.white;

	[Header("Skin Info")]
	public int ID;

	public string Filename = "Skin Name";

	public string Description = string.Empty;

	[Header("Court Elements")]
	public SkinSprite Floor;

	public SkinSprite FloorDetailUnder;

	public SkinSprite FloorDetailOver;

	public SkinSprite ThreePointArc_Player;

	public SkinSprite ThreePointArc_Enemy;

	public SkinSprite SideCourt;

	public SkinSprite ThreePointArcFill_Player;

	public SkinSprite ThreePointArcFill_Enemy;

	public SkinSprite CenterCircleFill_Left;

	public SkinSprite CenterCircleFill_Right;

	public SkinSprite FloorLogo;

	public SkinSprite FloorBaseline;

	public SkinSprite FloorHalfCourtLine;

	public SkinSprite FloorSideline;

	[Header("Hoop Elements")]
	public SkinSprite BackboardStand;

	public SkinSprite Backboard;

	public SkinSprite BackboardOutline;

	public SkinSprite Rectangle;

	public SkinSprite BackboardGraphic_Player;

	public SkinSprite BackboardGraphicOutline_Player;

	public SkinSprite BackboardGraphic_Enemy;

	public SkinSprite BackboardGraphicOutline_Enemy;

	[Header("Rim Elements")]
	public SkinSprite RimFront;

	public SkinSprite RimBehind;

	public SkinSprite RimOutline;

	public SkinSprite RimConnector;

	[Header("Net Elements")]
	public SkinSprite NetBase;

	[Header("Scoreboard")]
	public SkinSprite Scoreboard;

	[Header("Background")]
	public GameObject BkgPrefab;
}
