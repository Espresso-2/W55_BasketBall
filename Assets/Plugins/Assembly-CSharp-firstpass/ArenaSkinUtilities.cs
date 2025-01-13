using System;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSkinUtilities : MonoBehaviour
{
	public static string ArenaSkinDirectory = "ArenaSkins/";

	public static string ArenaSkinDirectoryAssetPath = "Assets/Resources/" + ArenaSkinDirectory;

	public static string ArenaSkinBackgroundDirectory = "ArenaSkinBackgrounds/";

	public static string DefaultSkinName = "ArenaSkin";

	public static ArenaSkin CreateNewSkin()
	{
		return null;
	}

	public static ArenaSkin LoadSkinFromIndex(int index)
	{
		string[] array = new string[80]
		{
			"ArenaSkin_00_Default", "ArenaSkin_01_UrbanCourt", "ArenaSkin_02_Waterfront", "ArenaSkin_03_Beach", "ArenaSkin_04_Desert", "ArenaSkin_05_SpookyIce", "ArenaSkin_06_NightGreen", "ArenaSkin_07_CityBright", "ArenaSkin_08_LushGreen", "ArenaSkin_09_Hell",
			"ArenaSkin_10_Autumn", "ArenaSkin_11_Alien", "ArenaSkin_12_Blue-ish", "ArenaSkin_13_BrightGreen", "ArenaSkin_14_Mountains", "ArenaSkin_15_Hell2", "ArenaSkin_16_PurplePlatforms", "ArenaSkin_17_Alien2", "ArenaSkin_18_GenericIndoor", "ArenaSkin_19_ConcreteBeems",
			"ArenaSkin_20_GenericOutside", "ArenaSkin_21_GreyFloor", "ArenaSkin_22_Alien2", "ArenaSkin_23_UrbanCourt", "ArenaSkin_24_Autumn", "ArenaSkin_25_SpookyIce", "ArenaSkin_26_IndoorNatural", "ArenaSkin_27_CartoonLights", "ArenaSkin_28_fadedBakery", "ArenaSkin_29_CityPurple",
			"ArenaSkin_30_CityModern", "ArenaSkin_31_PanoramaCity", "ArenaSkin_32_PanormamaCityGreyScale", "ArenaSkin_33_PlayGround", "ArenaSkin_34_TropicalBeach", "ArenaSkin_35_CityWalk", "ArenaSkin_36_FlatMeso", "ArenaSkin_37_FlatMesoGrey", "ArenaSkin_38_FlatMesoGrey", "ArenaSkin_39_DistantCity",
			"ArenaSkin_40_DistantCity", "ArenaSkin_41_DistantCity", "ArenaSkin_42_VectorCity", "ArenaSkin_43_VectorCity", "ArenaSkin_44_VectorCity", "ArenaSkin_45_VectorCity", "ArenaSkin_46_NewYork", "ArenaSkin_47_FerrisWheel", "ArenaSkin_48_FerrisWheelWater", "ArenaSkin_49_nightBeforeChristmas",
			"ArenaSkin_50_SummerCity", "ArenaSkin_51_SummerCity", "ArenaSkin_52_Renewables", "ArenaSkin_53_RoadToCity", "ArenaSkin_54_SpringCity", "ArenaSkin_55_VectorCity", "ArenaSkin_56_Halloween", "ArenaSkin_57_Christmas", "ArenaSkin_58_ChristmasHouse", "ArenaSkin_59_ChristmasLogHouse",
			"ArenaSkin_60_ChristmasLogHouse", "ArenaSkin_61_ThanksGiving", "ArenaSkin_62_NightCitySky", "ArenaSkin_63_CityPurpleSky", "ArenaSkin_64_Philly", "ArenaSkin_65_Boston", "ArenaSkin_66_Pittsburgh", "ArenaSkin_67_BeachFullColor", "ArenaSkin_68_FarmLand", "ArenaSkin_69_GenericIndoor",
			"ArenaSkin_70_Patriotic", "ArenaSkin_71_SummerCityWithSign", "ArenaSkin_72_SummerCityWithSign", "ArenaSkin_73_SummerCityWithSign", "ArenaSkin_74_ParkWalkway", "ArenaSkin_75_AfricanCity", "ArenaSkin_76_CactusMountains", "ArenaSkin_77_MountainSunset", "AS_Bkg_78_Blank", "AS_Bkg_79_VeryBasic"
		};
		if (index >= array.Length)
		{
			index = array.Length - 1;
		}
		string text = array[index];
		return Resources.Load<ArenaSkin>(ArenaSkinDirectory + text);
	}

	public static string SkinPathFromIndex(int index)
	{
		string empty = string.Empty;
		string[] array = AssetPathsInDirectory(ArenaSkinDirectory);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (SkinFilenameIndex(text) == index)
			{
				return text;
			}
		}
		return empty;
	}

	public static void DebugSkinOrder()
	{
		ArenaSkin[] array = Resources.LoadAll<ArenaSkin>(ArenaSkinDirectory);
		string text = "****** SKIN ORDER : ";
		ArenaSkin[] array2 = array;
		foreach (ArenaSkin arenaSkin in array2)
		{
			text = text + arenaSkin.Description + ", ";
		}
		Debug.Log(text);
	}

	public static ArenaSkin DuplicateSkin(ArenaSkin skin)
	{
		return null;
	}

	public static bool DeleteSkin(ArenaSkin skin)
	{
		return false;
	}

	public static string SkinShortName(string assetPath)
	{
		string empty = string.Empty;
		return assetPath.Replace(ArenaSkinDirectoryAssetPath, string.Empty).Replace(".asset", string.Empty);
	}

	public static string[] AssetPathsInDirectory(string directorty)
	{
		List<string> list = new List<string>();
		return list.ToArray();
	}

	public static string UniqueIndexedFileName(string description = "")
	{
		string defaultSkinName = DefaultSkinName;
		int num = 0;
		string[] paths = AssetPathsInDirectory(ArenaSkinDirectory);
		num = CheckMissingIndex(paths);
		return defaultSkinName + "_" + num + "_" + description;
	}

	public static int CheckMissingIndex(string[] paths)
	{
		int result = paths.Length + 1;
		List<int> list = new List<int>();
		foreach (string fileName in paths)
		{
			list.Add(SkinFilenameIndex(fileName));
		}
		for (int j = 0; j < paths.Length; j++)
		{
			if (!list.Contains(j + 1))
			{
				result = j + 1;
				break;
			}
		}
		return result;
	}

	public static int SkinFilenameIndex(string fileName)
	{
		int result = -1;
		char[] separator = new char[1] { '_' };
		string[] array = fileName.Split(separator, StringSplitOptions.None);
		int result2;
		if (array.Length > 1 && int.TryParse(array[1], out result2))
		{
			result = result2;
		}
		return result;
	}

	public static void UpdateCurrentName(ArenaSkin skin)
	{
	}

	public static void RenameSkin(ArenaSkin skin, string skinName)
	{
	}
}
