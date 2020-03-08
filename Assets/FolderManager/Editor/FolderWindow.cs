using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {

        private int rootPathIndxed;
        private string folderPath;
        private string folderNode;
        private List<string> nodeList = new List<string>();

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.Show();
        }

        void OnGUI()
        {
            GUIStyle TitleStyle = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };
            GUIStyle BtnStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter };
            string[] options = Enum.GetNames(typeof(RootPathType));
            string[] Paths;

            rootPathIndxed = EditorGUILayout.Popup("Root Path", rootPathIndxed, options);
            folderPath = RootPath.GetFolderPath((RootPathType)rootPathIndxed);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Add Nodes", TitleStyle);
            folderNode = GUILayout.TextField(folderNode);
            if (GUILayout.Button("+", BtnStyle))
            {
                if (!string.IsNullOrEmpty(folderNode))
                {
                    nodeList.Add(folderNode);
                    folderNode = GUILayout.TextField(string.Empty);
                    Debug.Log("+");
                }
            }
            GUILayout.EndHorizontal();

            if (nodeList.Count != 0)
            {
                GUILayout.Label(folderPath, TitleStyle);
                string hyphen = string.Empty;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(hyphen + "└" + nodeList[i]);
                    if (GUILayout.Button("-", BtnStyle))
                    {
                        nodeList.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                    hyphen += " ";
                }

            }

            GUILayout.BeginHorizontal();
            Paths = new string[nodeList.Count + 1];
            Paths[0] = folderPath;
            for (int i = 1; i < Paths.Length; i++)
                Paths[i] = nodeList[i - 1];
            string path = Path.Combine(Paths).Replace('\\', '/');
            GUILayout.Label("Folder Path", TitleStyle);
            GUILayout.Label(path);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clean All Node"))
            {
                nodeList = new List<string>();
                Debug.Log("Clean");
            }
            if (GUILayout.Button("Saving"))
            {
                Folder asset = new Folder()
                {
                    RootPathType = (RootPathType)rootPathIndxed,
                    Node = nodeList.ToArray()
                };
                string savePath = "/FolderManager/" + typeof(Folder).ToString() + ".asset";
                CreateAsset(asset, savePath);
            }
            GUILayout.EndHorizontal();
        }

        private void CreateAsset<T>(T asset, string Savepath)where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string assetPathAndName;

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + Savepath);
          
            if (AssetDatabase.DeleteAsset(assetPathAndName))
            {
                AssetDatabase.CreateAsset(asset, assetPathAndName);
            }


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
