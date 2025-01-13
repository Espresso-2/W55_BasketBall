using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Crosstales.Common.Util
{
	public static class XmlHelper
	{
		public static void SerializeToFile<T>(T obj, string filename)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			try
			{
				File.WriteAllText(filename, SerializeToString(obj));
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not serialize the object to a file: " + ex);
			}
		}

		public static T DeserializeFromFile<T>(string filename, bool skipBOM = false)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			try
			{
				if (File.Exists(filename))
				{
					return DeserializeFromString<T>(File.ReadAllText(filename), skipBOM);
				}
				Debug.LogError("File doesn't exist: " + filename);
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not deserialize the object from a file: " + ex);
			}
			return default(T);
		}

		public static string SerializeToString<T>(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			try
			{
				MemoryStream stream = new MemoryStream();
				XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
				xmlSerializer.Serialize(xmlTextWriter, obj);
				stream = (MemoryStream)xmlTextWriter.BaseStream;
				return Encoding.UTF8.GetString(stream.ToArray());
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not serialize the object to a string: " + ex);
			}
			return string.Empty;
		}

		public static T DeserializeFromString<T>(string xmlAsString, bool skipBOM = true)
		{
			if (string.IsNullOrEmpty(xmlAsString))
			{
				throw new ArgumentNullException("xmlAsString");
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (StringReader stringReader = new StringReader(xmlAsString))
				{
					if (skipBOM)
					{
						stringReader.Read();
					}
					return (T)xmlSerializer.Deserialize(stringReader);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not deserialize the object from a string: " + ex);
			}
			return default(T);
		}

		public static T DeserializeFromResource<T>(string resourceName, bool skipBOM = true)
		{
			if (string.IsNullOrEmpty(resourceName))
			{
				throw new ArgumentNullException("resourceName");
			}
			TextAsset textAsset = Resources.Load(resourceName) as TextAsset;
			return DeserializeFromString<T>(textAsset.text, skipBOM);
		}
	}
}
