using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object=UnityEngine.Object;

/// <summary>
/// 用于更改纹理的尺寸
/// 操作方式：拖拽到指定位置，然后进行操作
/// </summary>
public class AssetsOutput : EditorWindow
{
    private const string TipName = "拖入需要导出的源文件";
    private const string Title = "导出资源";

    private readonly List<string> filePaths = new List<string>();

    [MenuItem(nameof(AdGeneric)+"/" + Title, priority = 101)]
    private static void TextureEx()
    {
        GetWindowWithRect<AssetsOutput>(new Rect(Screen.width / 2, Screen.height / 2, 500, 500)).titleContent =
            new GUIContent(Title);
    }

    public static readonly Color Khaki = new Color(0.9411765f, 0.9019608f, 0.5490196f, 1f);

    private void Awake() => filePaths.Clear();
    private bool keepStyle=true;
    private Vector2 pos;

    private void OnGUI()
    {
        //! 实现拖拽
        Rect drawRect = EditorGUILayout.BeginHorizontal();
        GUILayout.Box(TipName, GUILayout.MinHeight(64), GUILayout.MinWidth(512));
        UnityEngine.Event currentEvent = UnityEngine.Event.current;
        //拖拽范围内
        if (drawRect.Contains(currentEvent.mousePosition))
        {
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                    break;
                case EventType.DragPerform:
                    var paths = GetTotalFiles(DragAndDrop.paths);
                    foreach (var t in paths.Where(e => !filePaths.Contains(e))) filePaths.Add(t);

                    break;
            }
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        keepStyle = GUILayout.Toggle(keepStyle, "保持文件格式");
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUI.color = new Color(0, 1, 072f, 0.5f);
        var fileCountLabel = GUI.skin.label;
        fileCountLabel.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label($"选择了 {filePaths.Count} 个文件", fileCountLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUI.color = Color.white;
        pos = GUILayout.BeginScrollView(pos, GUILayout.MaxHeight(400), GUILayout.MinHeight(20));
        var itemLabel = GUI.skin.label;
        itemLabel.alignment = TextAnchor.MiddleLeft;
        foreach (var path in filePaths.ToArray())
        {
            GUILayout.BeginHorizontal();
            var itemBox = GUI.skin.box;
            itemBox.alignment = TextAnchor.MiddleLeft;
            GUI.color = Color.white;
            GUILayout.Label(AssetDatabase.GetCachedIcon(path), itemBox,
                GUILayout.Width(20), GUILayout.Height(20));
            GUI.color = Khaki;
            GUILayout.Label(Path.GetFileNameWithoutExtension(path), itemLabel);
            GUI.color = Color.white;
            if (GUILayout.Button("X", GUILayout.Width(20))) filePaths.Remove(path);

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUI.color = Color.white;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("清除", GUILayout.Width(256), GUILayout.Height(40))) filePaths.Clear();
        if (GUILayout.Button("导出", GUILayout.Width(256), GUILayout.Height(40))) OutputFile();
        GUILayout.EndHorizontal();
    }

    private void OutputFile()
    {
        if (filePaths.Count == 0)
        {
            ShowNotification(new GUIContent("未选择任何资源文件"));
            return;
        }

        var outputRootPath = EditorUtility.OpenFolderPanel("选择导出路径", "", "");
        if (!Directory.Exists(outputRootPath))
        {
            ShowNotification(new GUIContent("路径不存在"));
            return;
        }

        outputRootPath = $"{outputRootPath}/Output-{DateTime.Now:yyyyMMdd-HHmmss}";
        int fc = 0, sc = 0;
        foreach (var filePath in filePaths)
        {
            try
            {
                string path = filePath;
                if (!keepStyle) path = new FileInfo(path).Name;
                var info = new FileInfo($"{outputRootPath}/{path}");
                if (!info.Directory.Exists) Directory.CreateDirectory(info.Directory.FullName);
                File.Copy(filePath, info.FullName, true);
                sc++;
            }
            catch
            {
                fc++;
            }
        }

        ShowNotification(new GUIContent($"导出完成,共:{filePaths.Count},成功:{sc},失败:{fc}"));
        ShowInExplorer(outputRootPath);
    }

    private void ShowInExplorer(string path)
    {
        if (!Directory.Exists(path))
        {
            ShowNotification(new GUIContent($"路径:\n{path}\n不存在"));
            return;
        }

        var process = Process.Start("Explorer.exe", path.Replace("/", "\\"));
        process?.Dispose();
    }

    private static IEnumerable<string> GetTotalFiles(IEnumerable<string> paths)
    {
        var files = new List<string>();
        foreach (var path in paths)
        {
            if (File.Exists(path)) files.Add(path);
            else if (Directory.Exists(path)) files.AddRange(Directory.EnumerateFiles(path));
        }

        return files.Where(e => !".meta".Equals(new FileInfo(e).Extension, StringComparison.CurrentCultureIgnoreCase));
    }

    private void OnDestroy()
    {
        filePaths.Clear();
    }
}

