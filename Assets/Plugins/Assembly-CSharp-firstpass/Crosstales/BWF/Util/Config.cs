using Crosstales.Common.Util;

namespace Crosstales.BWF.Util
{
	public static class Config
	{
		public static bool DEBUG;

		public static bool DEBUG_BADWORDS;

		public static bool DEBUG_DOMAINS;

		public static bool ENSURE_NAME = true;

		public static bool isLoaded;

		public static void Reset()
		{
			if (!BaseConstants.DEV_DEBUG)
			{
				DEBUG = false;
			}
			DEBUG_BADWORDS = false;
			DEBUG_DOMAINS = false;
			ENSURE_NAME = true;
		}

		public static void Load()
		{
			if (!BaseConstants.DEV_DEBUG)
			{
				if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG"))
				{
					DEBUG = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG");
				}
			}
			else
			{
				DEBUG = BaseConstants.DEV_DEBUG;
			}
			if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG_BADWORDS"))
			{
				DEBUG_BADWORDS = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG_BADWORDS");
			}
			if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG_DOMAINS"))
			{
				DEBUG_DOMAINS = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG_DOMAINS");
			}
			if (CTPlayerPrefs.HasKey("BWF_CFG_ENSURE_NAME"))
			{
				ENSURE_NAME = CTPlayerPrefs.GetBool("BWF_CFG_ENSURE_NAME");
			}
			isLoaded = true;
		}

		public static void Save()
		{
			if (!BaseConstants.DEV_DEBUG)
			{
				CTPlayerPrefs.SetBool("BWF_CFG_DEBUG", DEBUG);
			}
			CTPlayerPrefs.SetBool("BWF_CFG_DEBUG_BADWORDS", DEBUG_BADWORDS);
			CTPlayerPrefs.SetBool("BWF_CFG_DEBUG_DOMAINS", DEBUG_DOMAINS);
			CTPlayerPrefs.SetBool("BWF_CFG_ENSURE_NAME", ENSURE_NAME);
			CTPlayerPrefs.Save();
		}
	}
}
