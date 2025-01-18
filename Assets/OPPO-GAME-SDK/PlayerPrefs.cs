using QGMiniGame;

//覆盖unity的PlayerPrefs
public static class PlayerPrefs
{
    public static void SetInt(string key, int value)
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.SetInt(key,value);
#else
        QG.StorageSetIntSync(key, value);
#endif
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
#if UNITY_EDITOR
        return UnityEngine.PlayerPrefs.GetInt(key,defaultValue);
#else
        return QG.StorageGetIntSync(key, defaultValue);
#endif
    }
    public static void SetString(string key, string value)
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.SetString(key,value);
#else
        QG.StorageSetStringSync(key, value);
#endif
    }
    public static string GetString(string key, string defaultValue = "")
    {
#if UNITY_EDITOR
        return UnityEngine.PlayerPrefs.GetString(key,defaultValue);
#else
        return QG.StorageGetStringSync(key, defaultValue);
#endif
    }
    public static void SetFloat(string key, float value)
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.SetFloat(key,value);
#else
        QG.StorageSetFloatSync(key, value);
#endif
    }
    public static float GetFloat(string key, float defaultValue = 0)
    {
#if UNITY_EDITOR
        return UnityEngine.PlayerPrefs.GetFloat(key,defaultValue);
#else
        return QG.StorageGetFloatSync(key, defaultValue);
#endif
    }
    public static void DeleteAll()
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.DeleteAll();
#else
        QG.StorageDeleteAllSync();
#endif
    }
    public static void DeleteKey(string key)
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.DeleteKey(key);
#else
        QG.StorageDeleteKeySync(key);
#endif
    }
    public static bool HasKey(string key)
    {
#if UNITY_EDITOR
        return UnityEngine.PlayerPrefs.HasKey(key);
#else
        return QG.StorageHasKeySync(key);
#endif
    }

    public static void Save()
    {
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.Save();
#endif
    }
}
