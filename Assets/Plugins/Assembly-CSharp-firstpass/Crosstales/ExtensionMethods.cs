using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Crosstales
{
	public static class ExtensionMethods
	{
		public static string CTToTitleCase(this string str)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
		}

		public static string CTReverse(this string str)
		{
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		public static string CTReplace(this string str, string oldString, string newString, StringComparison comp = StringComparison.OrdinalIgnoreCase)
		{
			int num = str.IndexOf(oldString, comp);
			if (num >= 0)
			{
				str = str.Remove(num, oldString.Length);
				str = str.Insert(num, newString);
			}
			return str;
		}

		public static bool CTEquals(this string str, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return str.Equals(toCheck, comp);
		}

		public static bool CTContains(this string str, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return str.IndexOf(toCheck, comp) >= 0;
		}

		public static bool CTContainsAny(this string str, string searchTerms, char splitChar = ' ')
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (string.IsNullOrEmpty(searchTerms))
			{
				return true;
			}
			char[] separator = new char[1] { splitChar };
			return searchTerms.Split(separator, StringSplitOptions.RemoveEmptyEntries).Any((string searchTerm) => str.CTContains(searchTerm));
		}

		public static bool CTContainsAll(this string str, string searchTerms, char splitChar = ' ')
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (string.IsNullOrEmpty(searchTerms))
			{
				return true;
			}
			char[] separator = new char[1] { splitChar };
			return searchTerms.Split(separator, StringSplitOptions.RemoveEmptyEntries).All((string searchTerm) => str.CTContains(searchTerm));
		}

		public static void CTShuffle<T>(this T[] array, int seed = 0)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			System.Random random = ((seed != 0) ? new System.Random(seed) : new System.Random());
			int num = array.Length;
			while (num > 1)
			{
				int num2 = random.Next(num--);
				T val = array[num];
				array[num] = array[num2];
				array[num2] = val;
			}
		}

		public static string CTDump<T>(this T[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				T val = array[i];
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(val.ToString());
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this Vector2[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				Vector2 vector = array[i];
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(vector.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.y);
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this Vector3[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = array[i];
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(vector.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.y);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.z);
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this Vector4[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				Vector4 vector = array[i];
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(vector.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.y);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.z);
				stringBuilder.Append(", ");
				stringBuilder.Append(vector.w);
			}
			return stringBuilder.ToString();
		}

		public static string[] CTToString<T>(this T[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].ToString();
			}
			return array2;
		}

		public static void CTShuffle<T>(this IList<T> list, int seed = 0)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			System.Random random = ((seed != 0) ? new System.Random(seed) : new System.Random());
			int count = list.Count;
			while (count > 1)
			{
				int index = random.Next(count--);
				T value = list[count];
				list[count] = list[index];
				list[index] = value;
			}
		}

		public static string CTDump<T>(this IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (T item in list)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(item.ToString());
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this IList<Vector2> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Vector2 item in list)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(item.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.y);
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this IList<Vector3> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Vector3 item in list)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(item.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.y);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.z);
			}
			return stringBuilder.ToString();
		}

		public static string CTDump(this IList<Vector4> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Vector4 item in list)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(item.x);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.y);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.z);
				stringBuilder.Append(", ");
				stringBuilder.Append(item.w);
			}
			return stringBuilder.ToString();
		}

		public static List<string> CTToString<T>(this IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<string> list2 = new List<string>(list.Count);
			foreach (T item in list)
			{
				list2.Add(item.ToString());
			}
			return list2;
		}

		public static string CTDump<K, V>(this IDictionary<K, V> dict)
		{
			if (dict == null)
			{
				throw new ArgumentNullException("dict");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> item in dict)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append("Key = ");
				stringBuilder.Append(item.Key);
				stringBuilder.Append(", Value = ");
				stringBuilder.Append(item.Value);
			}
			return stringBuilder.ToString();
		}

		public static void CTAddRange<K, V>(this IDictionary<K, V> source, IDictionary<K, V> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (KeyValuePair<K, V> item in collection)
			{
				if (!source.ContainsKey(item.Key))
				{
					source.Add(item.Key, item.Value);
				}
				else
				{
					Debug.LogWarning("Duplicate key found: " + item.Key);
				}
			}
		}

		public static bool CTIsVisibleFrom(this Renderer renderer, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}
}
