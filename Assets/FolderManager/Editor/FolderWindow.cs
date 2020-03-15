using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {
        Editor EditorFolder;
        Folders folder;
        List<Editor> EditorFolderPath = new List<Editor>();

        private string SaveAssetPath = "Assets/FolderManager/StreamingAssets/FolderManager.asset";
        Vector2 m_ScrollPosition = Vector2.zero;

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.InitAssetEdit();
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal(new GUIStyle() { });
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Path", new GUIStyle(GUI.skin.button) { fixedWidth = 280f, fixedHeight = 35, fontSize = 14 }))
            {
                var asset = ScriptableObject.CreateInstance<FolderPath>();
                folder.Path.Add(asset);
                EditorFolderPath.Add(Editor.CreateEditor(asset));
                asset.Label = "LabelPath_" + folder.Path.Count;
                CreateAsset<Folders>(folder, SaveAssetPath);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            using(GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(m_ScrollPosition))
            {
                m_ScrollPosition = scrollViewScope.scrollPosition;
                using(new GUILayout.VerticalScope(new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter }))
                {
                    EditorFolder.OnInspectorGUI();
                }
            }
        }

        private void InitAssetEdit()
        {
            folder = LoadAsset<Folders>(SaveAssetPath);
            if (folder == null)
                folder = new Folders();
            Debug.Log(("Asset/" + SaveAssetPath) + "  " + (folder == null));
            EditorFolder = Editor.CreateEditor(folder);
            foreach (var item in folder.Path)
            {
                var ed = Editor.CreateEditor(item);
                EditorFolderPath.Add(ed);
            }
        }

        private void CreateAsset<T>(T asset, string Savepath)where T : ScriptableObject
        {
            string assetPathAndName = string.Empty;
            T LoaderAsset = LoadAsset<T>(Savepath);
            if (LoaderAsset != null)
            {
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
        }

        private T LoadAsset<T>(string assetPathAndName)where T : ScriptableObject
        {
            return (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
        }

        private string GenerateUniqueAssetPath(string Savepath)
        {
            return AssetDatabase.GenerateUniqueAssetPath(Savepath);
        }
    }
}
