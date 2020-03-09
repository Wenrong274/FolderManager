using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {
        private RootPathType rootPathType;
        private string folderPath;
        private string folderNode;
        private string AssetName = typeof(Folder).ToString() + ".asset";
        private List<string> nodeList = new List<string>();

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.Show();
            window.AseetInit();

        }

        public void AseetInit()
        {
            Folder asset = LoadAsset<Folder>(AssetName);
            if (asset != null)
            {
                rootPathType = asset.RootPathType;
                nodeList.AddRange(asset.Node);
            }
        }

        void OnGUI()
        {
            GUIStyle TitleStyle = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };
            GUIStyle BtnStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter };

            RootPathTypeGUI(TitleStyle);

            AddNodesGUI(TitleStyle, BtnStyle);

            ShowNodesGUI(TitleStyle, BtnStyle);

            ShowPathGUI(TitleStyle);

            CleanOrSaveGUI();
        }

        private void RootPathTypeGUI(GUIStyle TitleStyle)
        {
            GUIStyle popupGUI = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleLeft };
            string[] options = Enum.GetNames(typeof(RootPathType));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path", TitleStyle);
            rootPathType = (RootPathType)EditorGUILayout.Popup((int)rootPathType, options, popupGUI);
            folderPath = RootPath.GetFolderPath(rootPathType);
            GUILayout.EndHorizontal();
        }
        private void AddNodesGUI(GUIStyle TitleStyle, GUIStyle BtnStyle)
        {
            GUIStyle textFieldGUI = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.LowerLeft };
            GUILayout.BeginHorizontal();
            GUILayout.Label("Add Nodes", TitleStyle);
            folderNode = GUILayout.TextField(folderNode, textFieldGUI);
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
        }

        private void ShowNodesGUI(GUIStyle TitleStyle, GUIStyle BtnStyle)
        {
            if (nodeList.Count != 0)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Folder structure", TitleStyle);
                GUILayout.Label(folderPath, TitleStyle);

                GUILayout.EndHorizontal();

                string hyphen = string.Empty;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(hyphen + "└");
                    nodeList[i] = GUILayout.TextField(nodeList[i]);
                    if (GUILayout.Button("-", BtnStyle))
                    {

                        nodeList.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                    hyphen += " ";
                }
            }
        }

        private void ShowPathGUI(GUIStyle TitleStyle)
        {
            string[] Paths;
            GUILayout.BeginHorizontal();
            Paths = new string[nodeList.Count + 1];
            Paths[0] = folderPath;
            for (int i = 1; i < Paths.Length; i++)
                Paths[i] = nodeList[i - 1];
            string path = Path.Combine(Paths).Replace('\\', '/');
            GUILayout.Label("Folder Path", TitleStyle);
            GUILayout.Label(path);
            GUILayout.EndHorizontal();
        }

        private void CleanOrSaveGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clean All Node"))
            {
                nodeList = new List<string>();
                folderNode = string.Empty;
                Debug.Log("Clean");
            }

            if (GUILayout.Button("Saving"))
            {
                Folder asset = new Folder()
                {
                    RootPathType = (RootPathType)rootPathType,
                    Node = nodeList.ToArray()
                };
                CreateAsset(asset, AssetName);
            }
            GUILayout.EndHorizontal();
        }

        private void CreateAsset<T>(T asset, string Savepath)where T : ScriptableObject
        {
            string assetPathAndName = string.Empty;
            T LoaderAsset = LoadAsset<T>(Savepath);

            if (LoaderAsset != null)
            {
                assetPathAndName = GetAssetPathAndName(Savepath);
                AssetDatabase.CreateAsset(asset, assetPathAndName);
                LoaderAsset = asset;
                EditorUtility.SetDirty(LoaderAsset);
            }
            else
            {
                assetPathAndName = GenerateUniqueAssetPath(Savepath);
                AssetDatabase.CreateAsset(asset, assetPathAndName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        private T LoadAsset<T>(string Savepath)where T : ScriptableObject
        {
            string assetPathAndName = GetAssetPathAndName(Savepath);
            T result = AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T))as T;
            return result;
        }

        private string GetAssetPathAndName(string Savepath)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return Path.Combine(path, Savepath);
        }

        private string GenerateUniqueAssetPath(string Savepath)
        {
            string result = GetAssetPathAndName(Savepath);
            return AssetDatabase.GenerateUniqueAssetPath(result);
        }
    }
}
