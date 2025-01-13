using UnityEngine;

namespace Crosstales.Common.Util
{
	public abstract class BaseConstants
	{
		public const string ASSET_AUTHOR = "crosstales LLC";

		public const string ASSET_AUTHOR_URL = "https://www.crosstales.com";

		public const string ASSET_CT_URL = "https://goo.gl/qwtXyb";

		public const string ASSET_SOCIAL_DISCORD = "https://discord.gg/ZbZ2sh4";

		public const string ASSET_SOCIAL_FACEBOOK = "https://www.facebook.com/crosstales/";

		public const string ASSET_SOCIAL_TWITTER = "https://twitter.com/crosstales";

		public const string ASSET_SOCIAL_YOUTUBE = "https://www.youtube.com/c/Crosstales";

		public const string ASSET_SOCIAL_LINKEDIN = "https://www.linkedin.com/company/crosstales";

		public const string ASSET_SOCIAL_XING = "https://www.xing.com/companies/crosstales";

		public const string ASSET_3P_PLAYMAKER = "https://www.assetstore.unity3d.com/#!/content/368?aid=1011lNGT";

		public const int FACTOR_KB = 1024;

		public const int FACTOR_MB = 1048576;

		public const int FACTOR_GB = 1073741824;

		public const float FLOAT_32768 = 32768f;

		public const string FORMAT_TWO_DECIMAL_PLACES = "0.00";

		public const string FORMAT_NO_DECIMAL_PLACES = "0";

		public const string FORMAT_PERCENT = "0%";

		public const bool DEFAULT_DEBUG = false;

		public const string PATH_DELIMITER_WINDOWS = "\\";

		public const string PATH_DELIMITER_UNIX = "/";

		public static readonly string APPLICATION_PATH = BaseHelper.ValidatePath(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1));

		public static bool DEV_DEBUG = false;

		public static string TEXT_TOSTRING_END = "}";

		public static string TEXT_TOSTRING_DELIMITER = "', ";

		public static string TEXT_TOSTRING_DELIMITER_END = "'";

		public static string TEXT_TOSTRING_START = " {";

		public static string PREFIX_HTTP = "http://";

		public static string PREFIX_HTTPS = "https://";

		public static int PROCESS_KILL_TIME = 5000;

		public static string PREFIX_FILE
		{
			get
			{
				if (BaseHelper.isWindowsPlatform)
				{
					return "file:///";
				}
				return "file://";
			}
		}
	}
}
