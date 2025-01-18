using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AdGeneric.Ext;
using UnityEngine;
public static class AdAdapter
{

#if UNITY_EDITOR
    /// <summary>
    /// 初始化激励广告,主要用来提前load一个视频备用,避免首次展示激励时提示无可用广告
    /// 该函数不需要手动调用,已在构造函数中自动调用
    /// </summary>
    /// <param name="adUnitIdStr">激励广告id</param>
    public static void Init(string adUnitIdStr) => Debug.Log("初始化激励");

    public static void ShowBannerAd(string bannerAdUnitIdStr, string showDateStr) => Debug.Log("调用 Banner");

    /// <summary>
    /// 展示模板原生,并在原生关闭时自动展示banner
    /// </summary>
    /// <param name="adUnitIdStr">模板原生广告id</param>
    /// <param name="showDate">屏蔽时间</param>
    /// <param name="orientation">横竖屏</param>
    public static void ShowCustomAd(string adUnitIdStr , string showDate , int orientation ) =>
        Debug.Log("调用原生");


    public static void ShowNative(string posIdStr , int orientation ) => Debug.Log("调用自渲染Native");

    public static void ShowNativeIcon(string posIdStr , int orientation ) => Debug.Log("调用自渲染Icon");

    /// <summary>
    /// 展示激励广告
    /// </summary>
    /// <param name="callBackObjectName">回调函数脚本所在的gameobject名</param>
    /// <param name="callBackMethodName">回调函数名</param>
    /// <param name="callBackParam">回调函数参数</param>
    public static void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null)
    {
        Debug.Log("调用激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
    }

    public static void CreateShortcutButton() => Debug.Log("添加到桌面");

    public static void ExitApplication() => Application.Quit();

#else
    [DllImport("__Internal")]
    public static extern void Init(string adUnitIdStr );

    [DllImport("__Internal")]
    public static extern void ShowBannerAd(string bannerAdUnitIdStr , string showDateStr );

    [DllImport("__Internal")]
    public static extern void ShowCustomAd(string adUnitIdStr , string showDateStr, int orientation );

    
    [DllImport("__Internal")]
    public static extern void ShowNative(string posIdStr , int orientation );

    [DllImport("__Internal")]
    public static extern void HideNative();
 
    [DllImport("__Internal")]
    public static extern void ShowNativeIcon(string posIdStr , int orientation );

    [DllImport("__Internal")]
    public static extern void HideNativeIcon();

    [DllImport("__Internal")]
    public static extern void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam =
        null);

    [DllImport("__Internal")]
    public static extern void CreateShortcutButton();

    [DllImport("__Internal")]
    public static extern void ExitApplication();
#endif
}

