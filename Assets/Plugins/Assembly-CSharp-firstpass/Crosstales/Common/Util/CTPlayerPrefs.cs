using System;
using System.IO;
using UnityEngine;

namespace Crosstales.Common.Util
{
	public static class CTPlayerPrefs
	{
		private static SerializableDictionary<string, string> content;

		private static string fileName;

		static CTPlayerPrefs()
		{
			content = new SerializableDictionary<string, string>();
			fileName = Application.persistentDataPath + "/crosstales.cfg";
			if (File.Exists(fileName))
			{
				content = XmlHelper.DeserializeFromFile<SerializableDictionary<string, string>>(fileName);
			}
			else
			{
				content = new SerializableDictionary<string, string>();
			}
		}

		public static bool HasKey(string key)
		{
			return content.ContainsKey(key);
		}

		public static void DeleteAll()
		{
			content.Clear();
		}

		public static void DeleteKey(string key)
		{
			content.Remove(key);
		}

		public static void Save()
		{
			if (content != null && content.Count > 0)
			{
				XmlHelper.SerializeToFile(content, fileName);
			}
		}

		public static string GetString(string key)
		{
			return content[key];
		}

		public static float GetFloat(string key)
		{
			float result = 0f;
			float.TryParse(GetString(key), out result);
			return result;
		}

		public static int GetInt(string key)
		{
			int result = 0;
			int.TryParse(GetString(key), out result);
			return result;
		}

		public static bool GetBool(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			return "true".CTEquals(GetString(key)) ? true : false;
		}

		public static void SetString(string key, string value)
		{
			if (content.ContainsKey(key))
			{
				content[key] = value;
			}
			else
			{
				content.Add(key, value);
			}
		}

		public static void SetFloat(string key, float value)
		{
			SetString(key, value.ToString());
		}

		public static void SetInt(string key, int value)
		{
			SetString(key, value.ToString());
		}

		public static void SetBool(string key, bool value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			SetString(key, (!value) ? "false" : "true");
		}
	}
}
