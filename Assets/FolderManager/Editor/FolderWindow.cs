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

    [MenuItem("Folder Manager/Editor Path")]
    static void Init()
    {

        EditorWindow window = GetWindow(typeof(FolderWindow));
        window.Show();
    }

    void OnGUI()
    {
        var Style = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };
        var options = Enum.GetNames(typeof(RootPath));
        rootPathIndxed = EditorGUILayout.Popup("Root Path", rootPathIndxed, options);
        folderPath = GetFolderPath((RootPath)rootPathIndxed);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Folder Path", Style);
        GUILayout.Label(folderPath, Style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Add Node Folder", Style);
        folderNode = GUILayout.TextField(folderNode);
        nodeList.Add(folderNode);
        if (GUILayout.Button("-", new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter }))
        {
            Debug.Log("-");
        }

        if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter }))
        {
            Debug.Log("+");
        }
        GUILayout.EndHorizontal();

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
