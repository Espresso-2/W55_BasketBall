using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomItems : MonoBehaviour
{
	public static int BALL;

	public static int JERSEY;

	public static int ARM_BAND;

	public static int PANTS;

	public static int SHOES;

	private List<CustomItem> customBalls;

	private List<CustomItem> customJerseys;

	private List<CustomItem> customArmBands;

	private List<CustomItem> customPants;

	private List<CustomItem> customShoes;

	private List<CustomItem> allCustomItems;

	static CustomItems()
	{
		JERSEY = 1;
		ARM_BAND = 2;
		PANTS = 3;
		SHOES = 4;
	}

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		InstantiateCustomItems();
	}

	public virtual void Start()
	{
		if (GameObject.FindGameObjectsWithTag("CustomItems").Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public virtual void Update()
	{
	}

	public virtual CustomItem GetActiveItem(int type)
	{
		int @int = PlayerPrefs.GetInt("CUSTOM_ITEM_" + type + "_ACTIVE");
		CustomItem result = null;
		if (type == BALL)
		{
			result = customBalls[@int];
		}
		else if (type == JERSEY)
		{
			result = customJerseys[@int];
		}
		else if (type == ARM_BAND)
		{
			result = customArmBands[@int];
		}
		else if (type == PANTS)
		{
			result = customPants[@int];
		}
		else if (type == SHOES)
		{
			result = customShoes[@int];
		}
		return result;
	}

	public virtual List<CustomItem> GetItems(int type)
	{
		List<CustomItem> result = new List<CustomItem>();
		if (type == BALL)
		{
			result = customBalls;
		}
		else if (type == JERSEY)
		{
			result = customJerseys;
		}
		else if (type == ARM_BAND)
		{
			result = customArmBands;
		}
		else if (type == PANTS)
		{
			result = customPants;
		}
		else if (type == SHOES)
		{
			result = customShoes;
		}
		return result;
	}

	public List<CustomItem> GetOwnedItems(int type)
	{
		List<CustomItem> list = new List<CustomItem>();
		for (int i = 0; i < allCustomItems.Count; i++)
		{
			CustomItem customItem = allCustomItems[i];
			if (customItem.type == type && customItem.owned)
			{
				list.Add(customItem);
			}
		}
		return list;
	}

	public virtual void InstantiateCustomItems()
	{
		customBalls = new List<CustomItem>();
		customJerseys = new List<CustomItem>();
		customArmBands = new List<CustomItem>();
		customPants = new List<CustomItem>();
		customShoes = new List<CustomItem>();
		allCustomItems = new List<CustomItem>();
		AddItem(BALL, 0, 13, 0, 13);
		AddItem(BALL, 0, 16, 0, 15);
		AddItem(BALL, 0, 13, 0, 14);
		AddItem(BALL, 5, 9, 0, 9);
		AddItem(BALL, 10, 16, 0, 4);
		AddItem(BALL, 10, 4, 0, 1);
		AddItem(BALL, 15, 0, 0, 0);
		AddItem(BALL, 15, 12, 0, 12);
		AddItem(BALL, 10, 28, 0, 26);
		AddItem(BALL, 10, 29, 0, 26);
		AddItem(BALL, 10, 30, 0, 31);
		AddItem(BALL, 5, 31, 0, 31);
		AddItem(BALL, 10, 32, 0, 30);
		AddItem(BALL, 5, 35, 0, 35);
		AddItem(BALL, 15, 35, 0, 36);
		AddItem(BALL, 10, 36, 0, 38);
		AddItem(BALL, 5, 36, 0, 36);
		AddItem(BALL, 10, 37, 0, 35);
		AddItem(BALL, 10, 37, 0, 36);
		AddItem(BALL, 5, 42, 0, 42);
		AddItem(BALL, 10, 42, 0, 43);
		AddItem(BALL, 10, 45, 0, 46);
		AddItem(BALL, 10, 45, 0, 47);
		AddItem(BALL, 10, 49, 0, 45);
		AddItem(BALL, 10, 49, 0, 47);
		AddItem(BALL, 15, 50, 0, 51);
		AddItem(BALL, 10, 53, 0, 52);
		AddItem(BALL, 5, 58, 0, 58);
		AddItem(BALL, 10, 60, 0, 61);
		AddItem(BALL, 15, 60, 0, 63);
		AddItem(BALL, 15, 63, 0, 62);
		AddItem(BALL, 5, 65, 0, 65);
		AddItem(BALL, 15, 70, 0, 71);
		AddItem(BALL, 10, 72, 0, 73);
		AddItem(BALL, 10, 86, 0, 85);
		AddItem(BALL, 10, 88, 0, 86);
		AddItem(BALL, 10, 90, 0, 91);
		AddItem(BALL, 10, 93, 0, 91);
		AddItem(BALL, 15, 95, 0, 97);
		AddItem(BALL, 15, 100, 0, 101);
		AddItem(BALL, 10, 103, 0, 100);
		AddItem(BALL, 15, 103, 0, 101);
		AddItem(BALL, 5, 106, 0, 106);
		AddItem(BALL, 5, 110, 0, 110);
		AddItem(BALL, 10, 113, 0, 110);
		AddItem(BALL, 10, 115, 0, 116);
		AddItem(BALL, 10, 117, 0, 116);
		AddItem(BALL, 10, 120, 0, 123);
		AddItem(BALL, 15, 122, 0, 124);
		AddItem(BALL, 10, 125, 0, 127);
		AddItem(BALL, 15, 126, 0, 125);
		AddItem(BALL, 15, 128, 0, 127);
		AddItem(BALL, 10, 128, 0, 125);
		AddItem(BALL, 5, 133, 0, 133);
		AddItem(BALL, 15, 136, 0, 137);
		AddItem(BALL, 10, 144, 0, 141);
		AddItem(BALL, 10, 145, 0, 146);
		AddItem(BALL, 10, 148, 0, 146);
		AddItem(BALL, 10, 150, 0, 4);
		AddItem(BALL, 10, 152, 0, 150);
		AddItem(BALL, 10, 153, 0, 150);
		AddItem(BALL, 15, 155, 0, 157);
		AddItem(BALL, 10, 160, 0, 164);
		AddItem(BALL, 10, 161, 0, 164);
		AddItem(BALL, 15, 165, 0, 166);
		AddItem(BALL, 10, 166, 0, 167);
		AddItem(BALL, 10, 166, 0, 168);
		AddItem(BALL, 15, 169, 0, 165);
		AddItem(BALL, 10, 35, 0, 51);
		AddItem(BALL, 10, 35, 0, 18);
		AddItem(BALL, 10, 18, 0, 51);
		AddItem(BALL, 10, 0, 0, 12);
		AddItem(BALL, 10, 11, 0, 13);
		AddItem(BALL, 10, 10, 0, 13);
		AddItem(BALL, 10, 35, 0, 13);
		AddItem(BALL, 10, 9, 0, 13);
		AddItem(BALL, 10, 4, 0, 13);
		AddItem(JERSEY, 0, 0, 0, 11);
		AddItem(JERSEY, 0, 12, 0, 11);
		AddItem(JERSEY, 0, 18, 0, 16);
		AddItem(JERSEY, 5, 20, 0, 22);
		AddItem(JERSEY, 5, 20, 1, 4);
		AddItem(JERSEY, 5, 126, 0, 1);
		AddItem(JERSEY, 5, 5, 0, 4);
		AddItem(JERSEY, 5, 21, 2, 22);
		AddItem(JERSEY, 5, 21, 2, 23);
		AddItem(JERSEY, 10, 21, 3, 24);
		AddItem(JERSEY, 10, 22, 4, 21);
		AddItem(JERSEY, 10, 22, 6, 23);
		AddItem(JERSEY, 10, 22, 7, 24);
		AddItem(JERSEY, 15, 23, 8, 21);
		AddItem(JERSEY, 10, 23, 9, 22);
		AddItem(JERSEY, 10, 23, 11, 24);
		AddItem(JERSEY, 10, 24, 12, 21);
		AddItem(JERSEY, 10, 24, 13, 22);
		AddItem(JERSEY, 10, 24, 14, 23);
		AddItem(JERSEY, 10, 25, 17, 26);
		AddItem(JERSEY, 10, 25, 20, 29);
		AddItem(JERSEY, 15, 26, 21, 25);
		AddItem(JERSEY, 5, 26, 1, 4);
		AddItem(JERSEY, 5, 26, 2, 29);
		AddItem(JERSEY, 10, 27, 3, 25);
		AddItem(JERSEY, 10, 27, 4, 26);
		AddItem(JERSEY, 10, 27, 5, 28);
		AddItem(JERSEY, 10, 27, 6, 29);
		AddItem(JERSEY, 10, 28, 7, 25);
		AddItem(JERSEY, 15, 28, 8, 26);
		AddItem(JERSEY, 10, 28, 9, 27);
		AddItem(JERSEY, 10, 28, 10, 29);
		AddItem(JERSEY, 10, 29, 12, 26);
		AddItem(JERSEY, 10, 29, 13, 28);
		AddItem(JERSEY, 10, 30, 14, 31);
		AddItem(JERSEY, 10, 30, 15, 32);
		AddItem(JERSEY, 15, 30, 16, 33);
		AddItem(JERSEY, 10, 30, 17, 34);
		AddItem(JERSEY, 10, 31, 18, 30);
		AddItem(JERSEY, 10, 31, 19, 32);
		AddItem(JERSEY, 10, 31, 20, 33);
		AddItem(JERSEY, 10, 31, 21, 34);
		AddItem(JERSEY, 15, 32, 22, 30);
		AddItem(JERSEY, 15, 32, 23, 31);
		AddItem(JERSEY, 5, 32, 0, 31);
		AddItem(JERSEY, 5, 32, 1, 4);
		AddItem(JERSEY, 5, 33, 2, 30);
		AddItem(JERSEY, 10, 33, 3, 31);
		AddItem(JERSEY, 10, 33, 4, 32);
		AddItem(JERSEY, 10, 33, 5, 34);
		AddItem(JERSEY, 10, 34, 6, 30);
		AddItem(JERSEY, 10, 34, 7, 31);
		AddItem(JERSEY, 10, 34, 8, 32);
		AddItem(JERSEY, 10, 34, 9, 33);
		AddItem(JERSEY, 10, 35, 10, 36);
		AddItem(JERSEY, 10, 35, 11, 37);
		AddItem(JERSEY, 10, 35, 12, 38);
		AddItem(JERSEY, 15, 35, 13, 39);
		AddItem(JERSEY, 15, 36, 14, 35);
		AddItem(JERSEY, 10, 36, 15, 37);
		AddItem(JERSEY, 15, 36, 16, 38);
		AddItem(JERSEY, 10, 36, 17, 39);
		AddItem(JERSEY, 10, 37, 18, 35);
		AddItem(JERSEY, 10, 37, 19, 36);
		AddItem(JERSEY, 10, 37, 20, 38);
		AddItem(JERSEY, 15, 37, 21, 39);
		AddItem(JERSEY, 15, 38, 22, 35);
		AddItem(JERSEY, 15, 38, 23, 36);
		AddItem(JERSEY, 15, 38, 8, 37);
		AddItem(JERSEY, 5, 38, 1, 4);
		AddItem(JERSEY, 5, 39, 2, 35);
		AddItem(JERSEY, 10, 39, 3, 36);
		AddItem(JERSEY, 10, 39, 4, 37);
		AddItem(JERSEY, 10, 39, 5, 38);
		AddItem(JERSEY, 10, 38, 6, 36);
		AddItem(JERSEY, 10, 38, 7, 39);
		AddItem(JERSEY, 10, 41, 8, 42);
		AddItem(JERSEY, 10, 41, 10, 44);
		AddItem(JERSEY, 10, 42, 11, 41);
		AddItem(JERSEY, 10, 42, 12, 43);
		AddItem(JERSEY, 10, 42, 13, 44);
		AddItem(JERSEY, 10, 43, 14, 41);
		AddItem(JERSEY, 15, 43, 15, 42);
		AddItem(JERSEY, 10, 43, 16, 44);
		AddItem(JERSEY, 10, 44, 17, 42);
		AddItem(JERSEY, 10, 44, 18, 43);
		AddItem(JERSEY, 10, 45, 19, 46);
		AddItem(JERSEY, 10, 45, 20, 47);
		AddItem(JERSEY, 15, 45, 21, 48);
		AddItem(JERSEY, 10, 45, 22, 49);
		AddItem(JERSEY, 15, 46, 23, 45);
		AddItem(JERSEY, 5, 31, 0, 47);
		AddItem(JERSEY, 5, 46, 1, 4);
		AddItem(JERSEY, 5, 46, 2, 49);
		AddItem(JERSEY, 10, 47, 3, 45);
		AddItem(JERSEY, 10, 47, 4, 46);
		AddItem(JERSEY, 10, 47, 5, 48);
		AddItem(JERSEY, 10, 47, 6, 49);
		AddItem(JERSEY, 10, 48, 7, 45);
		AddItem(JERSEY, 10, 48, 8, 46);
		AddItem(JERSEY, 10, 48, 9, 49);
		AddItem(JERSEY, 10, 49, 10, 45);
		AddItem(JERSEY, 10, 49, 12, 47);
		AddItem(JERSEY, 10, 49, 13, 48);
		AddItem(JERSEY, 15, 50, 14, 51);
		AddItem(JERSEY, 10, 50, 15, 52);
		AddItem(JERSEY, 15, 50, 16, 53);
		AddItem(JERSEY, 10, 51, 17, 50);
		AddItem(JERSEY, 10, 51, 18, 52);
		AddItem(JERSEY, 10, 51, 19, 53);
		AddItem(JERSEY, 10, 52, 20, 51);
		AddItem(JERSEY, 10, 52, 21, 53);
		AddItem(JERSEY, 10, 53, 22, 50);
		AddItem(JERSEY, 10, 53, 23, 51);
		AddItem(JERSEY, 5, 53, 0, 52);
		AddItem(JERSEY, 5, 55, 1, 4);
		AddItem(JERSEY, 5, 55, 2, 57);
		AddItem(JERSEY, 10, 55, 3, 58);
		AddItem(JERSEY, 10, 56, 4, 55);
		AddItem(JERSEY, 10, 56, 6, 58);
		AddItem(JERSEY, 10, 57, 7, 55);
		AddItem(JERSEY, 10, 57, 8, 58);
		AddItem(JERSEY, 10, 58, 9, 55);
		AddItem(JERSEY, 10, 58, 10, 56);
		AddItem(JERSEY, 10, 58, 11, 57);
		AddItem(JERSEY, 10, 60, 12, 61);
		AddItem(JERSEY, 10, 60, 13, 62);
		AddItem(JERSEY, 10, 60, 14, 63);
		AddItem(JERSEY, 10, 61, 15, 60);
		AddItem(JERSEY, 15, 61, 16, 62);
		AddItem(JERSEY, 10, 61, 17, 63);
		AddItem(JERSEY, 10, 62, 18, 60);
		AddItem(JERSEY, 10, 62, 19, 61);
		AddItem(JERSEY, 10, 62, 20, 63);
		AddItem(JERSEY, 15, 63, 21, 60);
		AddItem(JERSEY, 10, 63, 22, 61);
		AddItem(JERSEY, 10, 63, 23, 62);
		AddItem(JERSEY, 5, 65, 0, 66);
		AddItem(JERSEY, 5, 65, 2, 4);
		AddItem(JERSEY, 5, 65, 2, 68);
		AddItem(JERSEY, 10, 66, 3, 65);
		AddItem(JERSEY, 10, 66, 4, 68);
		AddItem(JERSEY, 10, 67, 5, 65);
		AddItem(JERSEY, 10, 67, 6, 66);
		AddItem(JERSEY, 10, 67, 7, 68);
		AddItem(JERSEY, 10, 68, 8, 65);
		AddItem(JERSEY, 10, 68, 9, 66);
		AddItem(JERSEY, 10, 70, 10, 71);
		AddItem(JERSEY, 10, 70, 11, 72);
		AddItem(JERSEY, 10, 70, 12, 73);
		AddItem(JERSEY, 10, 71, 13, 70);
		AddItem(JERSEY, 10, 71, 14, 72);
		AddItem(JERSEY, 15, 71, 15, 73);
		AddItem(JERSEY, 15, 72, 16, 70);
		AddItem(JERSEY, 10, 72, 17, 71);
		AddItem(JERSEY, 10, 72, 18, 73);
		AddItem(JERSEY, 10, 73, 19, 70);
		AddItem(JERSEY, 10, 73, 20, 71);
		AddItem(JERSEY, 10, 73, 21, 72);
		AddItem(JERSEY, 10, 76, 22, 77);
		AddItem(JERSEY, 15, 76, 23, 78);
		AddItem(JERSEY, 5, 77, 0, 76);
		AddItem(JERSEY, 5, 77, 1, 4);
		AddItem(JERSEY, 5, 78, 2, 76);
		AddItem(JERSEY, 10, 78, 3, 77);
		AddItem(JERSEY, 10, 76, 4, 77);
		AddItem(JERSEY, 10, 76, 5, 78);
		AddItem(JERSEY, 10, 77, 6, 76);
		AddItem(JERSEY, 10, 77, 7, 78);
		AddItem(JERSEY, 10, 78, 8, 76);
		AddItem(JERSEY, 10, 78, 9, 77);
		AddItem(JERSEY, 10, 80, 4, 81);
		AddItem(JERSEY, 10, 80, 5, 82);
		AddItem(JERSEY, 10, 80, 6, 83);
		AddItem(JERSEY, 10, 81, 7, 80);
		AddItem(JERSEY, 10, 81, 8, 82);
		AddItem(JERSEY, 10, 81, 9, 83);
		AddItem(JERSEY, 10, 82, 10, 80);
		AddItem(JERSEY, 10, 82, 11, 81);
		AddItem(JERSEY, 10, 82, 12, 83);
		AddItem(JERSEY, 10, 83, 13, 80);
		AddItem(JERSEY, 10, 83, 14, 81);
		AddItem(JERSEY, 10, 83, 15, 82);
		AddItem(JERSEY, 10, 85, 16, 86);
		AddItem(JERSEY, 10, 85, 17, 87);
		AddItem(JERSEY, 10, 85, 18, 88);
		AddItem(JERSEY, 10, 86, 19, 85);
		AddItem(JERSEY, 15, 86, 20, 87);
		AddItem(JERSEY, 10, 86, 21, 88);
		AddItem(JERSEY, 10, 87, 22, 85);
		AddItem(JERSEY, 10, 87, 23, 86);
		AddItem(JERSEY, 10, 87, 2, 88);
		AddItem(JERSEY, 5, 88, 1, 4);
		AddItem(JERSEY, 5, 88, 2, 86);
		AddItem(JERSEY, 10, 88, 3, 87);
		AddItem(JERSEY, 10, 90, 4, 91);
		AddItem(JERSEY, 10, 90, 5, 93);
		AddItem(JERSEY, 10, 91, 6, 90);
		AddItem(JERSEY, 10, 91, 7, 93);
		AddItem(JERSEY, 15, 93, 8, 90);
		AddItem(JERSEY, 10, 93, 9, 91);
		AddItem(JERSEY, 10, 91, 10, 90);
		AddItem(JERSEY, 10, 91, 11, 93);
		AddItem(JERSEY, 10, 93, 12, 90);
		AddItem(JERSEY, 10, 93, 13, 91);
		AddItem(JERSEY, 10, 95, 10, 96);
		AddItem(JERSEY, 10, 95, 11, 97);
		AddItem(JERSEY, 15, 95, 12, 98);
		AddItem(JERSEY, 10, 96, 13, 95);
		AddItem(JERSEY, 10, 96, 14, 97);
		AddItem(JERSEY, 10, 96, 15, 98);
		AddItem(JERSEY, 10, 97, 16, 95);
		AddItem(JERSEY, 10, 97, 17, 96);
		AddItem(JERSEY, 10, 97, 18, 98);
		AddItem(JERSEY, 10, 98, 19, 95);
		AddItem(JERSEY, 10, 98, 20, 96);
		AddItem(JERSEY, 10, 98, 21, 97);
		AddItem(JERSEY, 10, 100, 22, 101);
		AddItem(JERSEY, 10, 100, 23, 102);
		AddItem(JERSEY, 5, 100, 0, 103);
		AddItem(JERSEY, 5, 101, 1, 4);
		AddItem(JERSEY, 5, 101, 2, 102);
		AddItem(JERSEY, 10, 101, 3, 103);
		AddItem(JERSEY, 10, 102, 4, 100);
		AddItem(JERSEY, 10, 102, 5, 101);
		AddItem(JERSEY, 10, 102, 6, 103);
		AddItem(JERSEY, 10, 103, 7, 100);
		AddItem(JERSEY, 10, 103, 8, 101);
		AddItem(JERSEY, 10, 103, 9, 102);
		AddItem(JERSEY, 10, 105, 10, 106);
		AddItem(JERSEY, 10, 105, 11, 107);
		AddItem(JERSEY, 10, 106, 12, 105);
		AddItem(JERSEY, 15, 106, 13, 107);
		AddItem(JERSEY, 10, 107, 14, 105);
		AddItem(JERSEY, 10, 107, 15, 106);
		AddItem(JERSEY, 10, 105, 16, 106);
		AddItem(JERSEY, 15, 105, 17, 107);
		AddItem(JERSEY, 10, 106, 18, 105);
		AddItem(JERSEY, 10, 106, 19, 107);
		AddItem(JERSEY, 10, 107, 20, 105);
		AddItem(JERSEY, 10, 107, 21, 106);
		AddItem(JERSEY, 15, 110, 16, 111);
		AddItem(JERSEY, 10, 110, 17, 113);
		AddItem(JERSEY, 15, 111, 18, 110);
		AddItem(JERSEY, 10, 111, 19, 112);
		AddItem(JERSEY, 10, 111, 20, 113);
		AddItem(JERSEY, 10, 125, 2, 126);
		AddItem(JERSEY, 15, 112, 22, 111);
		AddItem(JERSEY, 10, 112, 23, 113);
		AddItem(JERSEY, 5, 113, 0, 110);
		AddItem(JERSEY, 5, 113, 1, 4);
		AddItem(JERSEY, 5, 113, 2, 112);
		AddItem(JERSEY, 10, 115, 3, 116);
		AddItem(JERSEY, 10, 115, 4, 117);
		AddItem(JERSEY, 10, 115, 5, 118);
		AddItem(JERSEY, 10, 116, 6, 115);
		AddItem(JERSEY, 10, 116, 7, 117);
		AddItem(JERSEY, 10, 116, 8, 118);
		AddItem(JERSEY, 15, 117, 9, 115);
		AddItem(JERSEY, 10, 117, 10, 116);
		AddItem(JERSEY, 10, 117, 11, 118);
		AddItem(JERSEY, 10, 118, 12, 115);
		AddItem(JERSEY, 10, 118, 13, 116);
		AddItem(JERSEY, 10, 118, 14, 117);
		AddItem(JERSEY, 10, 118, 15, 115);
		AddItem(JERSEY, 10, 118, 16, 116);
		AddItem(JERSEY, 10, 118, 17, 117);
		AddItem(JERSEY, 10, 120, 15, 121);
		AddItem(JERSEY, 15, 120, 16, 122);
		AddItem(JERSEY, 10, 120, 17, 123);
		AddItem(JERSEY, 10, 120, 18, 124);
		AddItem(JERSEY, 10, 121, 19, 120);
		AddItem(JERSEY, 10, 121, 20, 122);
		AddItem(JERSEY, 10, 121, 21, 123);
		AddItem(JERSEY, 10, 121, 22, 124);
		AddItem(JERSEY, 10, 122, 23, 120);
		AddItem(JERSEY, 5, 122, 0, 121);
		AddItem(JERSEY, 5, 122, 1, 4);
		AddItem(JERSEY, 5, 122, 2, 124);
		AddItem(JERSEY, 10, 123, 3, 120);
		AddItem(JERSEY, 10, 123, 4, 121);
		AddItem(JERSEY, 10, 123, 5, 122);
		AddItem(JERSEY, 10, 123, 6, 124);
		AddItem(JERSEY, 10, 124, 7, 120);
		AddItem(JERSEY, 10, 124, 8, 0);
		AddItem(JERSEY, 10, 124, 9, 122);
		AddItem(JERSEY, 10, 125, 11, 126);
		AddItem(JERSEY, 10, 125, 12, 127);
		AddItem(JERSEY, 10, 125, 13, 128);
		AddItem(JERSEY, 15, 126, 14, 125);
		AddItem(JERSEY, 5, 126, 1, 4);
		AddItem(JERSEY, 5, 126, 2, 128);
		AddItem(JERSEY, 10, 127, 3, 125);
		AddItem(JERSEY, 15, 127, 5, 128);
		AddItem(JERSEY, 10, 128, 6, 125);
		AddItem(JERSEY, 10, 128, 7, 126);
		AddItem(JERSEY, 10, 128, 8, 127);
		AddItem(JERSEY, 10, 127, 9, 125);
		AddItem(JERSEY, 10, 127, 11, 128);
		AddItem(JERSEY, 10, 128, 12, 125);
		AddItem(JERSEY, 10, 128, 13, 126);
		AddItem(JERSEY, 10, 128, 14, 127);
		AddItem(JERSEY, 10, 130, 9, 131);
		AddItem(JERSEY, 15, 130, 11, 132);
		AddItem(JERSEY, 10, 130, 10, 133);
		AddItem(JERSEY, 10, 130, 11, 134);
		AddItem(JERSEY, 10, 131, 12, 130);
		AddItem(JERSEY, 15, 131, 13, 132);
		AddItem(JERSEY, 10, 131, 14, 133);
		AddItem(JERSEY, 10, 131, 15, 134);
		AddItem(JERSEY, 10, 132, 14, 130);
		AddItem(JERSEY, 5, 132, 2, 131);
		AddItem(JERSEY, 10, 132, 17, 133);
		AddItem(JERSEY, 10, 132, 18, 134);
		AddItem(JERSEY, 10, 133, 19, 130);
		AddItem(JERSEY, 10, 133, 20, 131);
		AddItem(JERSEY, 10, 133, 21, 132);
		AddItem(JERSEY, 10, 133, 22, 134);
		AddItem(JERSEY, 10, 134, 23, 130);
		AddItem(JERSEY, 5, 134, 2, 133);
		AddItem(JERSEY, 10, 135, 3, 136);
		AddItem(JERSEY, 10, 135, 4, 137);
		AddItem(JERSEY, 10, 136, 5, 135);
		AddItem(JERSEY, 10, 136, 6, 137);
		AddItem(JERSEY, 10, 137, 7, 135);
		AddItem(JERSEY, 10, 137, 8, 136);
		AddItem(JERSEY, 10, 135, 9, 136);
		AddItem(JERSEY, 10, 135, 10, 137);
		AddItem(JERSEY, 10, 136, 11, 135);
		AddItem(JERSEY, 10, 136, 12, 137);
		AddItem(JERSEY, 10, 137, 13, 135);
		AddItem(JERSEY, 10, 137, 14, 136);
		AddItem(JERSEY, 15, 140, 9, 141);
		AddItem(JERSEY, 10, 140, 10, 142);
		AddItem(JERSEY, 10, 140, 11, 143);
		AddItem(JERSEY, 10, 140, 12, 144);
		AddItem(JERSEY, 15, 141, 13, 140);
		AddItem(JERSEY, 10, 141, 14, 142);
		AddItem(JERSEY, 10, 141, 15, 143);
		AddItem(JERSEY, 10, 141, 16, 144);
		AddItem(JERSEY, 10, 142, 17, 140);
		AddItem(JERSEY, 10, 142, 18, 141);
		AddItem(JERSEY, 10, 142, 19, 143);
		AddItem(JERSEY, 10, 142, 20, 144);
		AddItem(JERSEY, 15, 143, 21, 140);
		AddItem(JERSEY, 10, 143, 22, 141);
		AddItem(JERSEY, 5, 143, 1, 142);
		AddItem(JERSEY, 5, 143, 0, 144);
		AddItem(JERSEY, 5, 144, 1, 4);
		AddItem(JERSEY, 5, 144, 2, 141);
		AddItem(JERSEY, 10, 144, 4, 143);
		AddItem(JERSEY, 10, 145, 5, 146);
		AddItem(JERSEY, 10, 145, 6, 147);
		AddItem(JERSEY, 10, 145, 7, 148);
		AddItem(JERSEY, 10, 145, 8, 149);
		AddItem(JERSEY, 10, 146, 9, 145);
		AddItem(JERSEY, 10, 146, 10, 147);
		AddItem(JERSEY, 10, 146, 11, 148);
		AddItem(JERSEY, 10, 146, 12, 149);
		AddItem(JERSEY, 10, 147, 13, 145);
		AddItem(JERSEY, 10, 147, 14, 146);
		AddItem(JERSEY, 10, 147, 15, 148);
		AddItem(JERSEY, 10, 147, 16, 149);
		AddItem(JERSEY, 10, 148, 17, 145);
		AddItem(JERSEY, 10, 148, 18, 146);
		AddItem(JERSEY, 10, 148, 19, 147);
		AddItem(JERSEY, 10, 148, 20, 149);
		AddItem(JERSEY, 15, 149, 21, 145);
		AddItem(JERSEY, 10, 149, 22, 146);
		AddItem(JERSEY, 10, 149, 23, 147);
		AddItem(JERSEY, 5, 150, 1, 4);
		AddItem(JERSEY, 5, 150, 2, 152);
		AddItem(JERSEY, 10, 150, 3, 153);
		AddItem(JERSEY, 10, 151, 4, 150);
		AddItem(JERSEY, 10, 151, 5, 152);
		AddItem(JERSEY, 15, 151, 6, 153);
		AddItem(JERSEY, 10, 152, 7, 150);
		AddItem(JERSEY, 10, 152, 8, 151);
		AddItem(JERSEY, 10, 152, 9, 153);
		AddItem(JERSEY, 10, 153, 10, 150);
		AddItem(JERSEY, 15, 153, 11, 151);
		AddItem(JERSEY, 10, 153, 12, 152);
		AddItem(JERSEY, 10, 152, 14, 150);
		AddItem(JERSEY, 10, 152, 15, 151);
		AddItem(JERSEY, 10, 152, 16, 153);
		AddItem(JERSEY, 10, 153, 17, 150);
		AddItem(JERSEY, 10, 153, 18, 151);
		AddItem(JERSEY, 10, 153, 19, 152);
		AddItem(JERSEY, 15, 155, 13, 156);
		AddItem(JERSEY, 10, 155, 14, 157);
		AddItem(JERSEY, 15, 155, 15, 158);
		AddItem(JERSEY, 15, 156, 16, 155);
		AddItem(JERSEY, 10, 156, 17, 157);
		AddItem(JERSEY, 10, 156, 18, 158);
		AddItem(JERSEY, 10, 157, 19, 155);
		AddItem(JERSEY, 10, 157, 20, 156);
		AddItem(JERSEY, 10, 157, 21, 158);
		AddItem(JERSEY, 15, 158, 22, 155);
		AddItem(JERSEY, 10, 158, 23, 156);
		AddItem(JERSEY, 5, 158, 0, 157);
		AddItem(JERSEY, 5, 157, 1, 155);
		AddItem(JERSEY, 5, 157, 2, 156);
		AddItem(JERSEY, 10, 157, 3, 158);
		AddItem(JERSEY, 10, 158, 4, 155);
		AddItem(JERSEY, 10, 158, 5, 156);
		AddItem(JERSEY, 10, 158, 6, 157);
		AddItem(JERSEY, 5, 160, 1, 161);
		AddItem(JERSEY, 5, 160, 2, 162);
		AddItem(JERSEY, 10, 160, 3, 163);
		AddItem(JERSEY, 10, 160, 4, 164);
		AddItem(JERSEY, 15, 161, 5, 160);
		AddItem(JERSEY, 10, 161, 6, 162);
		AddItem(JERSEY, 10, 161, 7, 163);
		AddItem(JERSEY, 10, 161, 8, 164);
		AddItem(JERSEY, 10, 162, 9, 160);
		AddItem(JERSEY, 10, 162, 10, 161);
		AddItem(JERSEY, 10, 162, 11, 163);
		AddItem(JERSEY, 10, 162, 12, 164);
		AddItem(JERSEY, 15, 163, 13, 160);
		AddItem(JERSEY, 10, 163, 14, 161);
		AddItem(JERSEY, 10, 163, 15, 162);
		AddItem(JERSEY, 15, 163, 16, 164);
		AddItem(JERSEY, 15, 164, 17, 160);
		AddItem(JERSEY, 10, 164, 18, 161);
		AddItem(JERSEY, 15, 164, 19, 162);
		AddItem(JERSEY, 10, 164, 20, 163);
		AddItem(JERSEY, 15, 165, 21, 166);
		AddItem(JERSEY, 10, 165, 22, 167);
		AddItem(JERSEY, 15, 165, 23, 168);
		AddItem(JERSEY, 5, 165, 0, 169);
		AddItem(JERSEY, 5, 166, 1, 165);
		AddItem(JERSEY, 5, 166, 2, 167);
		AddItem(JERSEY, 10, 166, 3, 168);
		AddItem(JERSEY, 10, 167, 5, 165);
		AddItem(JERSEY, 10, 167, 6, 166);
		AddItem(JERSEY, 10, 167, 7, 168);
		AddItem(JERSEY, 10, 167, 8, 169);
		AddItem(JERSEY, 10, 168, 10, 166);
		AddItem(JERSEY, 10, 168, 11, 167);
		AddItem(JERSEY, 10, 168, 12, 169);
		AddItem(JERSEY, 10, 169, 13, 165);
		AddItem(JERSEY, 10, 169, 14, 166);
		AddItem(JERSEY, 10, 169, 15, 167);
		AddItem(JERSEY, 10, 169, 16, 168);
		AddItem(ARM_BAND, 0, 0, 0, 0);
		AddItem(ARM_BAND, 0, 12, 0, 0);
		AddItem(ARM_BAND, 5, 18, 0, 0);
		AddItem(ARM_BAND, 10, 4, 0, 0);
		AddItem(ARM_BAND, 5, 7, 0, 0);
		AddItem(ARM_BAND, 10, 5, 0, 0);
		AddItem(ARM_BAND, 10, 10, 0, 0);
		AddItem(ARM_BAND, 5, 22, 0, 0);
		AddItem(ARM_BAND, 5, 25, 0, 0);
		AddItem(ARM_BAND, 5, 26, 0, 0);
		AddItem(ARM_BAND, 5, 28, 0, 0);
		AddItem(ARM_BAND, 5, 32, 0, 0);
		AddItem(ARM_BAND, 5, 33, 0, 0);
		AddItem(ARM_BAND, 15, 36, 0, 0);
		AddItem(ARM_BAND, 15, 37, 0, 0);
		AddItem(ARM_BAND, 5, 46, 0, 0);
		AddItem(ARM_BAND, 5, 60, 0, 0);
		AddItem(ARM_BAND, 10, 63, 0, 0);
		AddItem(ARM_BAND, 10, 77, 0, 0);
		AddItem(ARM_BAND, 15, 50, 0, 0);
		AddItem(ARM_BAND, 15, 51, 0, 0);
		AddItem(ARM_BAND, 5, 80, 0, 0);
		AddItem(ARM_BAND, 5, 81, 0, 0);
		AddItem(ARM_BAND, 5, 86, 0, 0);
		AddItem(ARM_BAND, 5, 90, 0, 0);
		AddItem(ARM_BAND, 5, 101, 0, 0);
		AddItem(ARM_BAND, 5, 106, 0, 0);
		AddItem(ARM_BAND, 5, 112, 0, 0);
		AddItem(ARM_BAND, 5, 120, 0, 0);
		AddItem(ARM_BAND, 15, 126, 0, 0);
		AddItem(ARM_BAND, 5, 130, 0, 0);
		AddItem(ARM_BAND, 5, 131, 0, 0);
		AddItem(ARM_BAND, 10, 151, 0, 0);
		AddItem(ARM_BAND, 10, 158, 0, 0);
		AddItem(ARM_BAND, 10, 167, 0, 0);
		AddItem(ARM_BAND, 5, 148, 0, 0);
		AddItem(ARM_BAND, 10, 152, 0, 0);
		AddItem(ARM_BAND, 5, 169, 0, 0);
		AddItem(PANTS, 0, 1, 0, 0);
		AddItem(PANTS, 0, 4, 0, 0);
		AddItem(PANTS, 15, 0, 0, 0);
		AddItem(PANTS, 15, 12, 0, 0);
		AddItem(PANTS, 5, 18, 0, 0);
		AddItem(PANTS, 5, 7, 0, 0);
		AddItem(PANTS, 10, 5, 0, 0);
		AddItem(PANTS, 10, 10, 0, 0);
		AddItem(PANTS, 5, 22, 0, 0);
		AddItem(PANTS, 5, 25, 0, 0);
		AddItem(PANTS, 5, 26, 0, 0);
		AddItem(PANTS, 5, 28, 0, 0);
		AddItem(PANTS, 10, 32, 0, 0);
		AddItem(PANTS, 5, 33, 0, 0);
		AddItem(PANTS, 10, 36, 0, 0);
		AddItem(PANTS, 5, 37, 0, 0);
		AddItem(PANTS, 5, 46, 0, 0);
		AddItem(PANTS, 10, 50, 0, 0);
		AddItem(PANTS, 15, 51, 0, 0);
		AddItem(PANTS, 5, 60, 0, 0);
		AddItem(PANTS, 10, 63, 0, 0);
		AddItem(PANTS, 5, 73, 0, 0);
		AddItem(PANTS, 5, 77, 0, 0);
		AddItem(PANTS, 5, 78, 0, 0);
		AddItem(PANTS, 5, 80, 0, 0);
		AddItem(PANTS, 5, 81, 0, 0);
		AddItem(PANTS, 5, 86, 0, 0);
		AddItem(PANTS, 5, 90, 0, 0);
		AddItem(PANTS, 5, 101, 0, 0);
		AddItem(PANTS, 5, 106, 0, 0);
		AddItem(PANTS, 5, 112, 0, 0);
		AddItem(PANTS, 5, 120, 0, 0);
		AddItem(PANTS, 15, 126, 0, 0);
		AddItem(PANTS, 5, 130, 0, 0);
		AddItem(PANTS, 5, 131, 0, 0);
		AddItem(PANTS, 10, 151, 0, 0);
		AddItem(PANTS, 15, 158, 0, 0);
		AddItem(PANTS, 5, 167, 0, 0);
		AddItem(PANTS, 5, 148, 0, 0);
		AddItem(PANTS, 5, 152, 0, 0);
		AddItem(PANTS, 5, 169, 0, 0);
		AddItem(SHOES, 0, 16, 0, 4);
		AddItem(SHOES, 0, 25, 17, 26);
		AddItem(SHOES, 5, 4, 0, 1);
		AddItem(SHOES, 15, 0, 0, 12);
		AddItem(SHOES, 10, 4, 0, 0);
		AddItem(SHOES, 5, 18, 0, 4);
		AddItem(SHOES, 5, 25, 20, 29);
		AddItem(SHOES, 5, 26, 21, 25);
		AddItem(SHOES, 5, 26, 1, 4);
		AddItem(SHOES, 5, 28, 7, 25);
		AddItem(SHOES, 5, 28, 8, 26);
		AddItem(SHOES, 10, 28, 10, 29);
		AddItem(SHOES, 5, 29, 12, 26);
		AddItem(SHOES, 5, 29, 13, 28);
		AddItem(SHOES, 5, 30, 14, 31);
		AddItem(SHOES, 5, 30, 15, 32);
		AddItem(SHOES, 10, 30, 16, 33);
		AddItem(SHOES, 5, 30, 17, 34);
		AddItem(SHOES, 5, 31, 18, 30);
		AddItem(SHOES, 5, 31, 19, 32);
		AddItem(SHOES, 5, 31, 20, 33);
		AddItem(SHOES, 5, 31, 21, 34);
		AddItem(SHOES, 5, 32, 22, 30);
		AddItem(SHOES, 5, 32, 0, 31);
		AddItem(SHOES, 5, 33, 2, 30);
		AddItem(SHOES, 5, 33, 4, 32);
		AddItem(SHOES, 15, 33, 5, 34);
		AddItem(SHOES, 5, 34, 8, 32);
		AddItem(SHOES, 5, 34, 9, 33);
		AddItem(SHOES, 10, 35, 10, 36);
		AddItem(SHOES, 5, 35, 11, 37);
		AddItem(SHOES, 15, 35, 12, 38);
		AddItem(SHOES, 5, 35, 13, 39);
		AddItem(SHOES, 5, 36, 14, 35);
		AddItem(SHOES, 5, 36, 15, 37);
		AddItem(SHOES, 5, 36, 16, 38);
		AddItem(SHOES, 5, 36, 17, 39);
		AddItem(SHOES, 5, 37, 18, 35);
		AddItem(SHOES, 5, 37, 19, 36);
		AddItem(SHOES, 5, 37, 20, 38);
		AddItem(SHOES, 5, 37, 21, 39);
		AddItem(SHOES, 5, 38, 22, 35);
		AddItem(SHOES, 5, 38, 23, 36);
		AddItem(SHOES, 5, 38, 8, 37);
		AddItem(SHOES, 5, 38, 1, 4);
		AddItem(SHOES, 5, 39, 2, 35);
		AddItem(SHOES, 5, 39, 3, 36);
		AddItem(SHOES, 5, 39, 4, 37);
		AddItem(SHOES, 5, 39, 5, 38);
		AddItem(SHOES, 5, 38, 6, 36);
		AddItem(SHOES, 5, 38, 7, 39);
		AddItem(SHOES, 5, 42, 11, 41);
		AddItem(SHOES, 5, 42, 12, 43);
		AddItem(SHOES, 5, 42, 13, 44);
		AddItem(SHOES, 5, 43, 15, 42);
		AddItem(SHOES, 5, 44, 17, 42);
		AddItem(SHOES, 5, 44, 18, 43);
		AddItem(SHOES, 5, 45, 19, 46);
		AddItem(SHOES, 5, 45, 20, 47);
		AddItem(SHOES, 5, 45, 21, 48);
		AddItem(SHOES, 5, 45, 22, 49);
		AddItem(SHOES, 5, 46, 23, 45);
		AddItem(SHOES, 5, 31, 0, 47);
		AddItem(SHOES, 5, 46, 1, 4);
		AddItem(SHOES, 5, 46, 2, 49);
		AddItem(SHOES, 5, 47, 3, 45);
		AddItem(SHOES, 5, 47, 4, 46);
		AddItem(SHOES, 5, 47, 5, 48);
		AddItem(SHOES, 5, 47, 6, 49);
		AddItem(SHOES, 5, 48, 7, 45);
		AddItem(SHOES, 5, 48, 8, 46);
		AddItem(SHOES, 5, 48, 9, 49);
		AddItem(SHOES, 5, 49, 10, 45);
		AddItem(SHOES, 5, 49, 12, 47);
		AddItem(SHOES, 5, 49, 13, 48);
		AddItem(SHOES, 10, 50, 14, 51);
		AddItem(SHOES, 5, 51, 17, 50);
		AddItem(SHOES, 5, 51, 18, 52);
		AddItem(SHOES, 5, 51, 19, 53);
		AddItem(SHOES, 10, 52, 20, 51);
		AddItem(SHOES, 5, 52, 21, 53);
		AddItem(SHOES, 5, 53, 22, 50);
		AddItem(SHOES, 5, 53, 23, 51);
		AddItem(SHOES, 5, 53, 0, 52);
		AddItem(SHOES, 10, 55, 1, 4);
		AddItem(SHOES, 5, 55, 2, 57);
		AddItem(SHOES, 5, 55, 3, 58);
		AddItem(SHOES, 5, 57, 7, 55);
		AddItem(SHOES, 5, 57, 8, 58);
		AddItem(SHOES, 5, 58, 9, 55);
		AddItem(SHOES, 5, 58, 11, 57);
		AddItem(SHOES, 10, 60, 12, 61);
		AddItem(SHOES, 5, 60, 13, 62);
		AddItem(SHOES, 15, 60, 14, 63);
		AddItem(SHOES, 5, 61, 15, 60);
		AddItem(SHOES, 10, 61, 16, 62);
		AddItem(SHOES, 10, 61, 17, 63);
		AddItem(SHOES, 5, 62, 18, 60);
		AddItem(SHOES, 10, 62, 19, 61);
		AddItem(SHOES, 5, 62, 20, 63);
		AddItem(SHOES, 10, 63, 21, 60);
		AddItem(SHOES, 10, 63, 22, 61);
		AddItem(SHOES, 5, 63, 23, 62);
		AddItem(SHOES, 5, 65, 0, 66);
		AddItem(SHOES, 15, 65, 2, 4);
		AddItem(SHOES, 5, 65, 2, 68);
		AddItem(SHOES, 5, 66, 3, 65);
		AddItem(SHOES, 10, 70, 10, 71);
		AddItem(SHOES, 5, 70, 11, 72);
		AddItem(SHOES, 5, 70, 12, 73);
		AddItem(SHOES, 15, 71, 13, 70);
		AddItem(SHOES, 5, 71, 14, 72);
		AddItem(SHOES, 10, 71, 15, 73);
		AddItem(SHOES, 10, 72, 16, 70);
		AddItem(SHOES, 5, 72, 17, 71);
		AddItem(SHOES, 5, 72, 18, 73);
		AddItem(SHOES, 5, 73, 20, 71);
		AddItem(SHOES, 5, 73, 21, 72);
		AddItem(SHOES, 5, 85, 0, 86);
		AddItem(SHOES, 5, 85, 0, 87);
		AddItem(SHOES, 5, 85, 0, 88);
		AddItem(SHOES, 5, 86, 0, 85);
		AddItem(SHOES, 5, 86, 0, 87);
		AddItem(SHOES, 5, 86, 0, 88);
		AddItem(SHOES, 5, 87, 0, 85);
		AddItem(SHOES, 5, 87, 0, 86);
		AddItem(SHOES, 5, 87, 0, 88);
		AddItem(SHOES, 5, 88, 0, 4);
		AddItem(SHOES, 5, 88, 0, 86);
		AddItem(SHOES, 5, 88, 0, 87);
		AddItem(SHOES, 10, 90, 0, 91);
		AddItem(SHOES, 10, 91, 0, 93);
		AddItem(SHOES, 5, 93, 0, 90);
		AddItem(SHOES, 5, 93, 0, 91);
		AddItem(SHOES, 5, 91, 0, 90);
		AddItem(SHOES, 5, 93, 0, 91);
		AddItem(SHOES, 15, 95, 11, 97);
		AddItem(SHOES, 5, 96, 13, 95);
		AddItem(SHOES, 10, 96, 14, 97);
		AddItem(SHOES, 5, 97, 17, 96);
		AddItem(SHOES, 5, 100, 22, 101);
		AddItem(SHOES, 5, 100, 23, 102);
		AddItem(SHOES, 5, 100, 0, 103);
		AddItem(SHOES, 5, 102, 5, 101);
		AddItem(SHOES, 5, 102, 6, 103);
		AddItem(SHOES, 5, 103, 7, 100);
		AddItem(SHOES, 5, 103, 8, 101);
		AddItem(SHOES, 5, 105, 11, 107);
		AddItem(SHOES, 10, 106, 13, 107);
		AddItem(SHOES, 10, 107, 15, 106);
		AddItem(SHOES, 5, 105, 16, 106);
		AddItem(SHOES, 5, 105, 17, 107);
		AddItem(SHOES, 5, 107, 20, 105);
		AddItem(SHOES, 15, 110, 16, 111);
		AddItem(SHOES, 5, 110, 17, 113);
		AddItem(SHOES, 10, 111, 18, 110);
		AddItem(SHOES, 5, 113, 0, 110);
		AddItem(SHOES, 5, 115, 3, 116);
		AddItem(SHOES, 10, 115, 4, 117);
		AddItem(SHOES, 5, 115, 5, 118);
		AddItem(SHOES, 5, 116, 7, 117);
		AddItem(SHOES, 5, 117, 9, 115);
		AddItem(SHOES, 5, 117, 10, 116);
		AddItem(SHOES, 10, 118, 12, 115);
		AddItem(SHOES, 5, 118, 17, 117);
		AddItem(SHOES, 15, 120, 17, 123);
		AddItem(SHOES, 5, 120, 18, 124);
		AddItem(SHOES, 5, 121, 19, 120);
		AddItem(SHOES, 10, 121, 20, 122);
		AddItem(SHOES, 5, 122, 1, 4);
		AddItem(SHOES, 10, 122, 2, 124);
		AddItem(SHOES, 10, 123, 3, 120);
		AddItem(SHOES, 5, 124, 7, 120);
		AddItem(SHOES, 5, 124, 8, 0);
		AddItem(SHOES, 5, 124, 9, 122);
		AddItem(SHOES, 5, 125, 12, 127);
		AddItem(SHOES, 10, 125, 13, 128);
		AddItem(SHOES, 10, 126, 14, 125);
		AddItem(SHOES, 5, 126, 2, 128);
		AddItem(SHOES, 5, 127, 4, 126);
		AddItem(SHOES, 5, 127, 5, 128);
		AddItem(SHOES, 5, 128, 7, 126);
		AddItem(SHOES, 5, 128, 8, 127);
		AddItem(SHOES, 5, 127, 9, 125);
		AddItem(SHOES, 10, 127, 10, 126);
		AddItem(SHOES, 10, 128, 12, 125);
		AddItem(SHOES, 10, 128, 14, 127);
		AddItem(SHOES, 15, 132, 14, 130);
		AddItem(SHOES, 10, 132, 2, 131);
		AddItem(SHOES, 5, 133, 22, 134);
		AddItem(SHOES, 5, 134, 23, 130);
		AddItem(SHOES, 5, 134, 2, 133);
		AddItem(SHOES, 15, 135, 3, 136);
		AddItem(SHOES, 5, 135, 4, 137);
		AddItem(SHOES, 5, 136, 6, 137);
		AddItem(SHOES, 5, 137, 8, 136);
		AddItem(SHOES, 5, 136, 11, 135);
		AddItem(SHOES, 5, 136, 12, 137);
		AddItem(SHOES, 5, 137, 13, 135);
		AddItem(SHOES, 5, 137, 14, 136);
		AddItem(SHOES, 10, 140, 9, 141);
		AddItem(SHOES, 5, 140, 12, 144);
		AddItem(SHOES, 15, 141, 13, 140);
		AddItem(SHOES, 5, 144, 1, 4);
		AddItem(SHOES, 5, 144, 2, 141);
		AddItem(SHOES, 5, 145, 5, 146);
		AddItem(SHOES, 10, 145, 6, 147);
		AddItem(SHOES, 5, 145, 8, 149);
		AddItem(SHOES, 10, 146, 11, 148);
		AddItem(SHOES, 5, 146, 12, 149);
		AddItem(SHOES, 10, 147, 13, 145);
		AddItem(SHOES, 5, 147, 16, 149);
		AddItem(SHOES, 5, 148, 18, 146);
		AddItem(SHOES, 5, 148, 20, 149);
		AddItem(SHOES, 15, 149, 21, 145);
		AddItem(SHOES, 5, 149, 22, 146);
		AddItem(SHOES, 5, 149, 23, 147);
		AddItem(SHOES, 5, 150, 1, 4);
		AddItem(SHOES, 5, 150, 3, 153);
		AddItem(SHOES, 5, 151, 4, 150);
		AddItem(SHOES, 5, 151, 6, 153);
		AddItem(SHOES, 5, 152, 7, 150);
		AddItem(SHOES, 5, 152, 8, 151);
		AddItem(SHOES, 5, 153, 11, 151);
		AddItem(SHOES, 5, 153, 12, 152);
		AddItem(SHOES, 5, 152, 14, 150);
		AddItem(SHOES, 5, 152, 16, 153);
		AddItem(SHOES, 15, 153, 17, 150);
		AddItem(SHOES, 10, 153, 18, 151);
		AddItem(SHOES, 5, 155, 14, 157);
		AddItem(SHOES, 5, 155, 15, 158);
		AddItem(SHOES, 5, 156, 16, 155);
		AddItem(SHOES, 10, 156, 18, 158);
		AddItem(SHOES, 10, 157, 19, 155);
		AddItem(SHOES, 5, 157, 21, 158);
		AddItem(SHOES, 10, 158, 22, 155);
		AddItem(SHOES, 5, 158, 0, 157);
		AddItem(SHOES, 5, 157, 1, 155);
		AddItem(SHOES, 5, 157, 3, 158);
		AddItem(SHOES, 5, 158, 4, 155);
		AddItem(SHOES, 5, 158, 5, 156);
		AddItem(SHOES, 15, 160, 1, 161);
		AddItem(SHOES, 5, 160, 3, 163);
		AddItem(SHOES, 5, 160, 4, 164);
		AddItem(SHOES, 10, 161, 5, 160);
		AddItem(SHOES, 10, 161, 7, 163);
		AddItem(SHOES, 5, 161, 8, 164);
		AddItem(SHOES, 10, 162, 9, 160);
		AddItem(SHOES, 5, 162, 10, 161);
		AddItem(SHOES, 5, 163, 13, 160);
		AddItem(SHOES, 5, 163, 14, 161);
		AddItem(SHOES, 5, 164, 18, 161);
		AddItem(SHOES, 5, 164, 20, 163);
		AddItem(SHOES, 15, 165, 21, 166);
		AddItem(SHOES, 10, 165, 22, 167);
		AddItem(SHOES, 15, 165, 23, 168);
		AddItem(SHOES, 10, 166, 1, 165);
		AddItem(SHOES, 10, 166, 2, 167);
		AddItem(SHOES, 10, 166, 3, 168);
		AddItem(SHOES, 10, 167, 5, 165);
		AddItem(SHOES, 10, 167, 7, 168);
		AddItem(SHOES, 5, 167, 8, 169);
		AddItem(SHOES, 15, 169, 13, 165);
		AddItem(SHOES, 5, 169, 14, 166);
	}

	private void AddItem(int type, int goldValue, int color, int overlaySprite, object overlayColor)
	{
		CustomItem customItem = new CustomItem();
		customItem.type = type;
		customItem.goldValue = goldValue;
		customItem.color = color;
		customItem.overlaySprite = overlaySprite;
		customItem.overlayColor = (int)overlayColor;
		if (customItem.type == BALL)
		{
			customItem.num = customBalls.Count;
			customBalls.Add(customItem);
		}
		else if (customItem.type == JERSEY)
		{
			customItem.num = customJerseys.Count;
			customJerseys.Add(customItem);
		}
		else if (customItem.type == ARM_BAND)
		{
			customItem.num = customArmBands.Count;
			customArmBands.Add(customItem);
		}
		else if (customItem.type == PANTS)
		{
			customItem.num = customPants.Count;
			customPants.Add(customItem);
		}
		else if (customItem.type == SHOES)
		{
			customItem.num = customShoes.Count;
			customShoes.Add(customItem);
		}
		if (customItem.goldValue == 0 || PlayerPrefs.GetInt(CustomItem.OWNED_KEY + customItem.type + "_" + customItem.num) == 1)
		{
			customItem.owned = true;
		}
		else
		{
			customItem.owned = false;
		}
		allCustomItems.Add(customItem);
	}
}
