using System.Runtime.InteropServices;
using UnityEngine;
public static class AdAdapter
{

#if UNITY_EDITOR
    public static void Init(string posIdStr)
    {
        Debug.Log("初始化激励");
    }
    public static void ShowBannerAd(string posIdStr)
    {
        Debug.Log("调用banner");
    }

    public static void ShowCustomAd(string posIdStr , string leftPosIdStr , string showDate , int orientation)
    {
        Debug.Log("调用原生");
    }
    public static void ShowNative(string posIdStr , int orientation )
    {
        Debug.Log("调用自渲染Native");
    }
    public static void ShowNativeIcon(string posIdStr , int orientation )
    {
        Debug.Log("调用自渲染Icon");
    }

    /// <summary>
    /// 激励广告接口,在编辑器中立刻发放奖励
    /// </summary>
    /// <param name="callBackObjectName">挂载回调函数的物体在场景中的名称</param>
    /// <param name="callBackMethodName">需要调用的回调函数方法名</param>
    /// <param name="callBackParam">回调函数参数,仅限字符串</param>
    /// <param name="posIdStr">激励广告ID</param>
    public static void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam, string posIdStr)
    {
        Debug.Log("调用激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
    }

    public static void CreateShortcutButton()
    {
        Debug.Log("添加到桌面");
    }
    public static void ExitApplication()
    {
        Application.Quit();
    }

#else
    [DllImport("__Internal")]
    public static extern void Init(string posIdStr);

    [DllImport("__Internal")]
    public static extern void ShowBannerAd(string posIdStr);

    [DllImport("__Internal")]
    public static extern void HideBannerAd();

    [DllImport("__Internal")]
    public static extern void ShowCustomAd(string posIdStr , string leftPosIdStr , string showDate , int orientation );

    [DllImport("__Internal")]
    public static extern void HideCustomAd();

    [DllImport("__Internal")]
    public static extern void ShowNative(string posIdStr , int orientation );

    [DllImport("__Internal")]
    public static extern void HideNative();
 
    [DllImport("__Internal")]
    public static extern void ShowNativeIcon(string posIdStr , int orientation );

    [DllImport("__Internal")]
    public static extern void HideNativeIcon();

    [DllImport("__Internal")]
    public static extern void ShowRewardAd(string callBackObjectName, string callBackMethodName,string callBackParam, string posIdStr );

    [DllImport("__Internal")]
    public static extern void CreateShortcutButton();

    [DllImport("__Internal")]
    public static extern void ExitApplication();
#endif
}

