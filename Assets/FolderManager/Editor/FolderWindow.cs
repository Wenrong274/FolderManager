using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderWindow : EditorWindow
{

    public enum RootPath
    {
        ProjectPath,
        ProjectDataPath,
        PersistentDataPath,
        StreamingAssetsPath,
        TemporaryCachePath
    }

    private int rootPathIndxed;
    private string folderPath;
    private string folderNode;
    private List<string> nodeList = new List<string>();

    [MenuItem("Folder Manage/AA")]
    static void Init()
    {
        FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUIStyle TitleStyle = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };

        var options = Enum.GetNames(typeof(RootPath));
        rootPathIndxed = EditorGUILayout.Popup("Root Path", rootPathIndxed, options);
        folderPath = GetFolderPath((RootPath)rootPathIndxed);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Add Nodes", TitleStyle);
        folderNode = GUILayout.TextField(folderNode);
        if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter }))
        {
            if (!string.IsNullOrEmpty(folderNode))
            {
                nodeList.Add(folderNode);
                Debug.Log("+");
            }
        }
        GUILayout.EndHorizontal();

        if (nodeList.Count != 0)
        {
            GUILayout.Label(folderPath, TitleStyle);
            string hyphen = string.Empty;
            for (int i = 1; i < nodeList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(hyphen + "└" + nodeList[i]);
                if (GUILayout.Button("-", new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter }))
                {
                    nodeList.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
                hyphen += " ";
            }

        }
        GUILayout.BeginHorizontal();

        GUILayout.Label("Folder Path", TitleStyle);
        GUILayout.Label(folderPath, TitleStyle);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Clean All Node"))
        {
            nodeList = new List<string>();
            Debug.Log("Clean");
        }
    }

    private string GetFolderPath(RootPath arg)
    {
        switch (arg)
        {
            case RootPath.ProjectPath:
                return Directory.GetParent(Application.dataPath).FullName.Replace('\\', '/');
            case RootPath.ProjectDataPath:
                return Application.dataPath.Replace('\\', '/');
            case RootPath.PersistentDataPath:
                return Application.persistentDataPath.Replace('\\', '/');
            case RootPath.StreamingAssetsPath:
                return Application.streamingAssetsPath.Replace('\\', '/');
            case RootPath.TemporaryCachePath:
                return Application.temporaryCachePath.Replace('\\', '/');
            default:
                return string.Empty;
        }
    }
}
