using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System.Collections;

namespace QGMiniGame
{
    public class QGEditorWindow : EditorWindow
    {
        public static string buildSrc = "";  //用户选择构建的webgl路径

        public static string webglDir = "webgl";       //构建的webgl文件夹
        public static string wasmUrl = "";             //自定义loading网络url
        public static string streamingAssetsUrl = "";  //Addressable网络url

        public static string assetsUrl1 = "";//预加载地址1
        public static string assetsUrl2 = "";//预加载地址2
        public static string assetsUrl3 = "";//预加载地址3
        public static string assetsUrl4 = "";//预加载地址4
        public static string assetsUrl5 = "";//预加载地址5
        public ArrayList assetsList = new ArrayList();

        public static bool useSelfLoading;
        public static bool useSubPkgLoading;
        public static bool useAddressable;
        public static bool usePreAsset;
        public static bool useWebgl2;
        public static bool useCodeSize;

        [MenuItem("VIVO小游戏 / 构建", false, 1)]
        public static void OpenWindow()
        {
#if !(UNITY_2019_1_OR_NEWER )
            QGLog.LogError("目前仅支持 Unity2019以上的版本！");
#endif
            initData();
            var win = GetWindow(typeof(QGEditorWindow), false, "构建VIVO小游戏", true);
            win.minSize = new Vector2(650, 600);
            win.maxSize = new Vector2(1600, 600);
            win.Show();
            QGGameTools.CheckUpdate();
        }


        private static void initData()
        {
            QGGameConfig config = QGGameTools.GetEditorConfig();
            buildSrc = config.buildSrc;
            assetsUrl1 = config.assetsUrl1;
            assetsUrl2 = config.assetsUrl2;
            assetsUrl3 = config.assetsUrl3;
            assetsUrl4 = config.assetsUrl4;
            assetsUrl5 = config.assetsUrl5;
            useSelfLoading = config.useSelfLoading;
            useSubPkgLoading = config.useSubPkgLoading;
            useAddressable = config.useAddressable;
            usePreAsset = config.usePreAsset;
            useWebgl2 = config.useWebgl2;
            useCodeSize = config.useCodeSize;
            wasmUrl = config.envConfig.wasmUrl;
            streamingAssetsUrl = config.envConfig.streamingAssetsUrl;
        }


        // 打包构建可编辑面板UI
        private void OnGUI()
        {
            var labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.fontSize = 14;
            labelStyle.margin.left = 20;
            labelStyle.margin.top = 10;
            labelStyle.margin.bottom = 10;
            GUILayout.Label("构建设置(非必填)", labelStyle);

            //--------------------------------------

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.margin.left = 20;
            toggleStyle.margin.right = 20;
            toggleStyle.fontSize = 12;

            var inputStyle = new GUIStyle(EditorStyles.textField);
            inputStyle.fontSize = 14;
            inputStyle.margin.left = 20;
            inputStyle.margin.bottom = 10;
            inputStyle.margin.right = 20;

            var assetStyle = new GUIStyle(EditorStyles.textField);
            assetStyle.fontSize = 14;
            assetStyle.margin.left = 20;
            assetStyle.margin.right = 20;

            useSelfLoading = GUILayout.Toggle(useSelfLoading, "使用上传加载自定义Loading", toggleStyle);
            if (useSelfLoading) {
                wasmUrl = EditorGUILayout.TextField("自定义Loading地址", wasmUrl, inputStyle);
                var tips1 = new GUIStyle();
                tips1.fontSize = 12;
                tips1.normal.textColor = Color.red;
                tips1.margin.left = 20;
                tips1.margin.bottom = 10;
                tips1.margin.right = 20;
                GUILayout.TextField("备注：该地址配置上线时请注意版本号的区分，不能多个导出版本混用同一个wasm线上文件", tips1);
            }

            useSubPkgLoading = GUILayout.Toggle(useSubPkgLoading, "使用分包加载自定义Loading", toggleStyle);

            useAddressable = GUILayout.Toggle(useAddressable, "使用Addressable", toggleStyle);
            if (useAddressable) {
                streamingAssetsUrl = EditorGUILayout.TextField("Addressable地址", streamingAssetsUrl, inputStyle);
            }

            usePreAsset = GUILayout.Toggle(usePreAsset, "使用预下载", toggleStyle);
            if (usePreAsset) {
                assetsUrl1 = EditorGUILayout.TextField("资源地址1", assetsUrl1, assetStyle);
                assetsUrl2 = EditorGUILayout.TextField("资源地址2", assetsUrl2, assetStyle);
                assetsUrl3 = EditorGUILayout.TextField("资源地址3", assetsUrl3, assetStyle);
                assetsUrl4 = EditorGUILayout.TextField("资源地址4", assetsUrl4, assetStyle);
                assetsUrl5 = EditorGUILayout.TextField("资源地址5", assetsUrl5, assetStyle);
            }
            assetsList.Clear();
            if (assetsUrl1 != String.Empty && !assetsList.Contains(assetsUrl1))
            {
                assetsList.Add(assetsUrl1);
            }

            if (assetsUrl2 != String.Empty && !assetsList.Contains(assetsUrl2))
            {
                assetsList.Add(assetsUrl2);
            }

            if (assetsUrl3 != String.Empty && !assetsList.Contains(assetsUrl3))
            {
                assetsList.Add(assetsUrl3);
            }

            if (assetsUrl4 != String.Empty && !assetsList.Contains(assetsUrl4))
            {
                assetsList.Add(assetsUrl4);
            }

            if (assetsUrl5 != String.Empty && !assetsList.Contains(assetsUrl5))
            {
                assetsList.Add(assetsUrl5);
            }

            useWebgl2 = GUILayout.Toggle(useWebgl2, "启用Webgl2.0", toggleStyle);

#if UNITY_2021
            useCodeSize = GUILayout.Toggle(useCodeSize, "启动代码裁剪(IL2CPP Code Generation)", toggleStyle);
#endif

            //--------------------------------------
            GUILayout.Label("路径选择(必填)", labelStyle);
            var chooseBuildPathClick = false;
            var openBuildPathClick = false;
            var resetBuildPathClick = false;

            if (buildSrc == String.Empty)
            {
                GUIStyle pathButtonStyle = new GUIStyle(GUI.skin.button);
                pathButtonStyle.fontSize = 12;
                pathButtonStyle.margin.left = 20;
                chooseBuildPathClick = GUILayout.Button("选择导出路径 *", pathButtonStyle, GUILayout.Height(30), GUILayout.Width(200));
            }
            else
            {
                int pathButtonHeight = 28;
                GUIStyle pathLabelStyle = new GUIStyle(GUI.skin.textField);
                pathLabelStyle.fontSize = 12;
                pathLabelStyle.alignment = TextAnchor.MiddleLeft;
                pathLabelStyle.margin.top = 6;
                pathLabelStyle.margin.bottom = 6;
                pathLabelStyle.margin.left = 20;

                GUILayout.BeginHorizontal();
                // 路径框
                GUILayout.Label(buildSrc, pathLabelStyle, GUILayout.Height(pathButtonHeight - 6), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 126));
                openBuildPathClick = GUILayout.Button("打开", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                resetBuildPathClick = GUILayout.Button("重选", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (chooseBuildPathClick)
            {
                var dstPath = EditorUtility.SaveFolderPanel("选择导出目录", "", "");

                if (dstPath != "")
                {
                    buildSrc = dstPath;
                }
            }

            if (openBuildPathClick)
            {
                QGGameTools.ShowInExplorer(buildSrc);
            }

            if (resetBuildPathClick)
            {
                buildSrc = "";
            }

#if UNITY_2021_1_OR_NEWER

#else
            var picTips = new GUIStyle();
            picTips.fontSize = 12;
            picTips.normal.textColor = Color.red;
            picTips.margin.left = 20;
            picTips.margin.top = 5;
            picTips.margin.right = 20;
            GUILayout.TextField("备注：当前Unity引擎版本低于2021，建议升级使用引擎版本自带的纹理ASTC压缩格式", picTips);
#endif
            // -------------------------------------
            GUIStyle exportButtonStyle = new GUIStyle(GUI.skin.button);
            exportButtonStyle.fontSize = 14;
            exportButtonStyle.margin.left = 20;
            exportButtonStyle.margin.top = 40;

            var isExportBtnPressed = GUILayout.Button("构建WEBGL并转换小游戏", exportButtonStyle, GUILayout.Height(40), GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));

            if (isExportBtnPressed)
            {
                DoBuild();
            }

            GUILayout.Label("其它功能", labelStyle);

            GUILayout.BeginHorizontal();
            GUIStyle toolButtonStyle = new GUIStyle(GUI.skin.button);
            toolButtonStyle.fontSize = 12;
            toolButtonStyle.margin.left = 20;
            toolButtonStyle.margin.top = 10;
            var isUpdateBtnPressed = GUILayout.Button("检查更新", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isUpdateBtnPressed)
            {
                QGGameTools.CheckUpdate();
            }

            toolButtonStyle.normal.textColor = Color.green;

            var isHelpBtnPressed = GUILayout.Button("使用文档与教程", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isHelpBtnPressed)
            {
                QGGameTools.OpenUnityGame();
            }

            toolButtonStyle.normal.textColor = Color.white;

            var isAssetBundlePressed = GUILayout.Button("AB打包工具", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isAssetBundlePressed)
            {
                QGGameTools.AssetBundleBuild();
            }

            var isPerformanceToolPressed = GUILayout.Button("性能优化工具", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isPerformanceToolPressed)
            {
                QGGameTools.PerformanceTool();
            }

            toolButtonStyle.normal.textColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.Label("意见问题反馈", labelStyle);

            GUILayout.BeginHorizontal();
            var isVivoGameBtnPressed = GUILayout.Button("开发平台", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isVivoGameBtnPressed)
            {
                QGGameTools.OpenVivoGame();
            }

            var isIssueBtnPressed = GUILayout.Button("意见反馈", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isIssueBtnPressed)
            {
                QGGameTools.OpenIssueGihub();
            }

            var isQuestionBtnPressed = GUILayout.Button("常见问题", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
            if (isQuestionBtnPressed)
            {
                QGGameTools.OpenQuestionGithub();
            }


            GUILayout.EndHorizontal();

            var tips = new GUIStyle();
            tips.fontSize =12;
            tips.normal.textColor = Color.red;
            tips.margin.left = 20;
            tips.margin.top = 5;
            tips.margin.right = 20;
            GUILayout.TextField("备注：Unity导出到vivo小游戏相关问题可以通过上述三个入口检索相应的信息", tips);
        }

        // 构建webgl
        public void DoBuild()
        {
            QGGameTools.setEditorConfig(buildSrc, wasmUrl, streamingAssetsUrl, assetsUrl1, assetsUrl2, assetsUrl3, assetsUrl4, assetsUrl5,useSelfLoading,useAddressable,usePreAsset, useWebgl2, useCodeSize);
            if (buildSrc == String.Empty)
            {
                ShowNotification(new GUIContent("请先选择游戏导出路径"));
            }
            else
            {
                // 判断是否是webgl平台
                if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
                {
                    if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
                    {
                        ShowNotification(new GUIContent("构建失败，请配置unity webgl构建环境"));
                        return;
                    }
                }

                if (useSelfLoading) {

                    if (wasmUrl == String.Empty) {
                        ShowNotification(new GUIContent("构建失败，请配置自定义Loading地址，或者取消该选项"));
                        return;
                    }
                }

                if (useSelfLoading && useSubPkgLoading) {
                    ShowNotification(new GUIContent("构建失败，只能选择一种自定义Loading加载方式"));
                    return;
                }

                var webGlPath = Path.Combine(buildSrc, webglDir);
                QGGameTools.SetPlayer(useWebgl2, useCodeSize);
                QGGameTools.DelectDir(webGlPath);
                QGGameTools.BuildWebGL(webGlPath);
                if (!Directory.Exists(webGlPath))
                {
                    ShowNotification(new GUIContent("构建失败，WebGl项目未成功生成"));
                    return;
                }
                QGGameTools.CreateEnvConfig(useSelfLoading ? wasmUrl : "", streamingAssetsUrl, webGlPath, assetsList, useSubPkgLoading);
                QGGameTools.ConvetWebGl(buildSrc, webGlPath,useSelfLoading, useSubPkgLoading);
            }
        }
    }
}
