using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Crosstales.Common.Util
{
	public static class SerializeDeSerialize
	{
		private static BinaryFormatter binaryFormatter
		{
			get
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				return binaryFormatter;
			}
		}

		public static void SerializeToFile<T>(T obj, string filename)
		{
			try
			{
				using (FileStream serializationStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					binaryFormatter.Serialize(serializationStream, obj);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not serialize the object to a file: " + ex);
			}
		}

		public static byte[] SerializeToByteArray<T>(T obj)
		{
			byte[] result = null;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					binaryFormatter.Serialize(memoryStream, obj);
					result = memoryStream.ToArray();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not serialize the object to a byte-array: " + ex);
			}
			return result;
		}

		public static T DeserializeFromFile<T>(string filename)
		{
			try
			{
				using (FileStream serializationStream = new FileStream(filename, FileMode.Open))
				{
					return (T)binaryFormatter.Deserialize(serializationStream);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not deserialize the object from a file: " + ex);
			}
			return default(T);
		}

		public static T DeserializeFromByteArray<T>(byte[] data)
		{
			try
			{
				using (MemoryStream serializationStream = new MemoryStream(data))
				{
					return (T)binaryFormatter.Deserialize(serializationStream);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not deserialize the object from a byte-array: " + ex);
			}
			return default(T);
		}
	}
}
