using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Crosstales.Common.Model.Enum;
using UnityEngine;

namespace Crosstales.Common.Util
{
	public abstract class BaseHelper
	{
		public static readonly CultureInfo BaseCulture = new CultureInfo("en-US");

		protected static readonly Regex lineEndingsRegex = new Regex("\\r\\n|\\r|\\n");

		protected static readonly Regex cleanSpacesRegex = new Regex("\\s+");

		protected static readonly Regex cleanTagsRegex = new Regex("<.*?>");

		protected static readonly System.Random rnd = new System.Random();

		protected const string file_prefix = "file://";

		public static bool isInternetAvailable
		{
			get
			{
				return Application.internetReachability != NetworkReachability.NotReachable;
			}
		}

		public static bool isWindowsPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
			}
		}

		public static bool isMacOSPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor;
			}
		}

		public static bool isLinuxPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor;
			}
		}

		public static bool isStandalonePlatform
		{
			get
			{
				return isWindowsPlatform || isMacOSPlatform || isLinuxPlatform;
			}
		}

		public static bool isAndroidPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.Android;
			}
		}

		public static bool isIOSPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS;
			}
		}

		public static bool isWSAPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.MetroPlayerARM || Application.platform == RuntimePlatform.MetroPlayerX86 || Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.XboxOne;
			}
		}

		public static bool isWebGLPlatform
		{
			get
			{
				return Application.platform == RuntimePlatform.WebGLPlayer;
			}
		}

		public static bool isWebPlatform
		{
			get
			{
				return isWebGLPlatform;
			}
		}

		public static bool isWindowsBasedPlatform
		{
			get
			{
				return isWindowsPlatform || isWSAPlatform;
			}
		}

		public static bool isAppleBasedPlatform
		{
			get
			{
				return isMacOSPlatform || isIOSPlatform;
			}
		}

		public static bool isEditor
		{
			get
			{
				return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor;
			}
		}

		public static bool isEditorMode
		{
			get
			{
				return isEditor && !Application.isPlaying;
			}
		}

		public static Platform CurrentPlatform
		{
			get
			{
				if (isWindowsPlatform)
				{
					return Platform.Windows;
				}
				if (isMacOSPlatform)
				{
					return Platform.OSX;
				}
				if (isLinuxPlatform)
				{
					return Platform.Linux;
				}
				if (isAndroidPlatform)
				{
					return Platform.Android;
				}
				if (isIOSPlatform)
				{
					return Platform.IOS;
				}
				if (isWSAPlatform)
				{
					return Platform.WSA;
				}
				if (isWebPlatform)
				{
					return Platform.Web;
				}
				return Platform.Unsupported;
			}
		}

		public static string CreateString(string replaceChars, int stringLength)
		{
			if (replaceChars.Length > 1)
			{
				char[] array = new char[stringLength];
				for (int i = 0; i < stringLength; i++)
				{
					array[i] = replaceChars[rnd.Next(0, replaceChars.Length)];
				}
				return new string(array);
			}
			if (replaceChars.Length == 1)
			{
				return new string(replaceChars[0], stringLength);
			}
			return string.Empty;
		}

		public static bool hasActiveClip(AudioSource source)
		{
			return source != null && source.clip != null && ((!source.loop && source.timeSamples > 0 && source.timeSamples < source.clip.samples - 256) || source.loop || source.isPlaying);
		}

		public static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool result = true;
			if (sslPolicyErrors != 0)
			{
				for (int i = 0; i < chain.ChainStatus.Length; i++)
				{
					if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
					{
						chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
						chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
						result = chain.Build((X509Certificate2)certificate);
					}
				}
			}
			return result;
		}

		public static string ValidatePath(string path, bool addEndDelimiter = true)
		{
			if (!string.IsNullOrEmpty(path))
			{
				string text = path.Trim();
				string text2 = null;
				if (isWindowsBasedPlatform)
				{
					text2 = text.Replace('/', '\\');
					if (addEndDelimiter && !text2.EndsWith("\\"))
					{
						text2 += "\\";
					}
				}
				else
				{
					text2 = text.Replace('\\', '/');
					if (addEndDelimiter && !text2.EndsWith("/"))
					{
						text2 += "/";
					}
				}
				return string.Join("_", text2.Split(Path.GetInvalidPathChars()));
			}
			return path;
		}

		public static string ValidateFile(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				string text = ValidatePath(path);
				if (text.EndsWith("\\") || text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				string text2 = text.Substring((!isWindowsBasedPlatform) ? (text.LastIndexOf("/") + 1) : (text.LastIndexOf("\\") + 1));
				string text3 = string.Join(string.Empty, text2.Split(Path.GetInvalidFileNameChars()));
				return text.Substring(0, text.Length - text2.Length) + text3;
			}
			return path;
		}

		public static string ValidURLFromFilePath(string path)
		{
			if (path == null)
			{
				return path;
			}
			if (!string.IsNullOrEmpty(path))
			{
				if (!isValidURL(path))
				{
					return BaseConstants.PREFIX_FILE + ValidateFile(path).Replace(" ", "%20").Replace('\\', '/');
				}
				return ValidateFile(path).Replace(" ", "%20").Replace('\\', '/');
			}
			return Uri.EscapeDataString(path);
		}

		public static string CleanUrl(string url, bool removeProtocol = true, bool removeWWW = true, bool removeSlash = true)
		{
			string text = url.Trim();
			if (!string.IsNullOrEmpty(url))
			{
				if (removeProtocol)
				{
					text = text.Substring(text.IndexOf("//") + 2);
				}
				if (removeWWW)
				{
					text = text.CTReplace("www.", string.Empty);
				}
				if (removeSlash && text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			return text;
		}

		public static string ClearTags(string text)
		{
			return cleanTagsRegex.Replace(text, string.Empty).Trim();
		}

		public static string ClearSpaces(string text)
		{
			return cleanSpacesRegex.Replace(text, " ").Trim();
		}

		public static string ClearLineEndings(string text)
		{
			return lineEndingsRegex.Replace(text, string.Empty).Trim();
		}

		public static List<string> SplitStringToLines(string text, bool ignoreCommentedLines = true, int skipHeaderLines = 0, int skipFooterLines = 0)
		{
			List<string> list = new List<string>(100);
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'SplitStringToLines()' will return an empty string list.");
			}
			else
			{
				string[] array = lineEndingsRegex.Split(text);
				for (int i = 0; i < array.Length; i++)
				{
					if (i + 1 <= skipHeaderLines || i >= array.Length - skipFooterLines || string.IsNullOrEmpty(array[i]))
					{
						continue;
					}
					if (ignoreCommentedLines)
					{
						if (!array[i].StartsWith("#"))
						{
							list.Add(array[i]);
						}
					}
					else
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}

		public static string FormatBytesToHRF(long bytes)
		{
			string[] array = new string[5] { "B", "KB", "MB", "GB", "TB" };
			double num = bytes;
			int num2 = 0;
			while (num >= 1024.0 && num2 < array.Length - 1)
			{
				num2++;
				num /= 1024.0;
			}
			return string.Format("{0:0.##} {1}", num, array[num2]);
		}

		public static string FormatSecondsToHourMinSec(double seconds)
		{
			int num = (int)seconds;
			int num2 = num % 60;
			if (seconds >= 86400.0)
			{
				int num3 = num / 86400;
				int num4 = (num -= num3 * 86400) / 3600;
				int num5 = (num - num4 * 3600) / 60;
				return num3 + "d " + num4 + ":" + ((num5 >= 10) ? num5.ToString() : ("0" + num5)) + ":" + ((num2 >= 10) ? num2.ToString() : ("0" + num2));
			}
			if (seconds >= 3600.0)
			{
				int num6 = num / 3600;
				int num7 = (num - num6 * 3600) / 60;
				return num6 + ":" + ((num7 >= 10) ? num7.ToString() : ("0" + num7)) + ":" + ((num2 >= 10) ? num2.ToString() : ("0" + num2));
			}
			int num8 = num / 60;
			return num8 + ":" + ((num2 >= 10) ? num2.ToString() : ("0" + num2));
		}

		public static Color HSVToRGB(float h, float s, float v, float a = 1f)
		{
			if (s == 0f)
			{
				return new Color(v, v, v, a);
			}
			h /= 60f;
			int num = Mathf.FloorToInt(h);
			float num2 = h - (float)num;
			float num3 = v * (1f - s);
			float num4 = v * (1f - s * num2);
			float num5 = v * (1f - s * (1f - num2));
			switch (num)
			{
			case 0:
				return new Color(v, num5, num3, a);
			case 1:
				return new Color(num4, v, num3, a);
			case 2:
				return new Color(num3, v, num5, a);
			case 3:
				return new Color(num3, num4, v, a);
			case 4:
				return new Color(num5, num3, v, a);
			default:
				return new Color(v, num3, num4, a);
			}
		}

		public static bool isValidURL(string url)
		{
			return !string.IsNullOrEmpty(url) && (url.StartsWith("file://", StringComparison.OrdinalIgnoreCase) || url.StartsWith(BaseConstants.PREFIX_HTTP, StringComparison.OrdinalIgnoreCase) || url.StartsWith(BaseConstants.PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase));
		}

		public static void FileCopy(string inputFile, string outputFile, bool move = false)
		{
			if (string.IsNullOrEmpty(outputFile))
			{
				return;
			}
			try
			{
				if (!File.Exists(inputFile))
				{
					Debug.LogError("Input file does not exists: " + inputFile);
					return;
				}
				Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
				if (File.Exists(outputFile))
				{
					if (BaseConstants.DEV_DEBUG)
					{
						Debug.LogWarning("Overwrite output file: " + outputFile);
					}
					File.Delete(outputFile);
				}
				if (move)
				{
					File.Copy(inputFile, outputFile);
					File.Delete(inputFile);
				}
				else
				{
					File.Copy(inputFile, outputFile);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not copy file!" + Environment.NewLine + ex);
			}
		}
	}
}
