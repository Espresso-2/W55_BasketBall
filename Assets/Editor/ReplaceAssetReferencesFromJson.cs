using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ReplaceAssetReferencesFromJson : EditorWindow
{
    private Object newAsset; // 新的目标资源
    private string importFilePath = "Assets/References.json"; // JSON 文件路径

    [MenuItem("Tools/Replace References From JSON")]
    private static void Init()
    {
        GetWindow<ReplaceAssetReferencesFromJson>("Replace References");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Replace Asset References From JSON", EditorStyles.boldLabel);
        newAsset = EditorGUILayout.ObjectField("New Asset", newAsset, typeof(Object), false);
        importFilePath = EditorGUILayout.TextField("Import File Path", importFilePath);

        if (GUILayout.Button("Replace References"))
        {
            if (newAsset == null)
            {
                Debug.LogError("Please assign a new asset.");
                return;
            }

            if (!File.Exists(importFilePath))
            {
                Debug.LogError("JSON file not found at the specified path.");
                return;
            }

            string json = File.ReadAllText(importFilePath);
            ReferenceCollection references = JsonUtility.FromJson<ReferenceCollection>(json);

            string newAssetPath = AssetDatabase.GetAssetPath(newAsset);
            string newGUID = AssetDatabase.AssetPathToGUID(newAssetPath);

            foreach (var reference in references.References)
            {
                if (File.Exists(reference.FilePath))
                {
                    string content = File.ReadAllText(reference.FilePath);
                    content = content.Replace(reference.GUID, newGUID);
                    File.WriteAllText(reference.FilePath, content);
                    Debug.Log($"Replaced reference in: {reference.FilePath}");
                }
            }

            // 刷新资源
            AssetDatabase.Refresh();
            Debug.Log("All references have been replaced!");
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
