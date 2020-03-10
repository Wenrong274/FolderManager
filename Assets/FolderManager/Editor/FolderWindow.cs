using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {
        private List<Editor> editors;
        private RootPathType rootPathType;
        private string AssetName = typeof(FolderPath).ToString() + ".asset";

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.editors = new List<Editor>();
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Folder Path"))
            {
                var editor = Editor.CreateEditor(ScriptableObject.CreateInstance<FolderPath>());
                editors.Add(editor);
            }
            GUILayout.EndHorizontal();
            foreach (var e in editors)
            {
                GUILayout.BeginVertical();
                e.OnInspectorGUI();
                GUILayout.EndVertical();
                DrawUILine(Color.grey);

            }

        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        private void CleanOrSaveGUI()
        {

            if (GUILayout.Button("Saving"))
            {
                FolderPath asset = new FolderPath()
                {
                    RootPathType = (RootPathType)rootPathType,
                    //Node = nodeList
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
