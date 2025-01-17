using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ExportAssetReferences : EditorWindow
{
    private Object targetObject; // 要查找的目标资源
    private string exportFilePath = "Assets/W_Find/References.json"; // 导出路径

    [MenuItem("Tools/Export Asset References")]
    private static void Init()
    {
        GetWindow<ExportAssetReferences>("Export References");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Find and Export Asset References", EditorStyles.boldLabel);
        targetObject = EditorGUILayout.ObjectField("Target Asset", targetObject, typeof(Object), false);
        exportFilePath = EditorGUILayout.TextField("Export File Path", exportFilePath);

        if (GUILayout.Button("Export References"))
        {
            if (targetObject == null)
            {
                Debug.LogError("Please assign a target asset.");
                return;
            }

            string targetPath = AssetDatabase.GetAssetPath(targetObject);
            string targetGUID = AssetDatabase.AssetPathToGUID(targetPath);

            List<ReferenceInfo> references = new List<ReferenceInfo>();
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string path in allAssetPaths)
            {
                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".asset"))
                {
                    string content = File.ReadAllText(path);
                    if (content.Contains(targetGUID))
                    {
                        references.Add(new ReferenceInfo { FilePath = path, GUID = targetGUID });
                        Debug.Log($"Found reference in: {path}");
                    }
                }
            }

            // 导出 JSON
            string json = JsonUtility.ToJson(new ReferenceCollection { References = references }, true);
            File.WriteAllText(exportFilePath, json);
            AssetDatabase.Refresh();
            Debug.Log($"References exported to: {exportFilePath}");
        }
    }

    [System.Serializable]
    public class ReferenceInfo
    {
        public string FilePath; // 引用文件的路径
        public string GUID;     // 引用资源的GUID
    }

    [System.Serializable]
    public class ReferenceCollection
    {
        public List<ReferenceInfo> References;
    }
}
