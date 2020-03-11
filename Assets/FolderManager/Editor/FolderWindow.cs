using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {
        private List<Editor> editors;
        private string SaveAssetPath = "FolderManager/StreamingAssets/";
        Vector2 m_ScrollPosition = Vector2.zero;

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.editors = new List<Editor>();
            window.InitAssetEdit();
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
            using(GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(m_ScrollPosition))
            {
                m_ScrollPosition = scrollViewScope.scrollPosition;
                using(new GUILayout.VerticalScope(new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter }))
                {
                    foreach (var item in editors.ToList())
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        item.OnInspectorGUI();
                        GUILayout.EndVertical();
                        SaveAndDeleteButton(item);
                        GUILayout.EndVertical();
                        DrawUILine(Color.grey);
                    }
                }
            }
        }

        private void InitAssetEdit()
        {
            string assetPath = GetAssetPathAndName(SaveAssetPath);
            string[] fileName = Directory.GetFiles(Application.dataPath + "/" + SaveAssetPath, "*.asset", SearchOption.AllDirectories);

            foreach (var item in fileName)
            {
                string path = item.Replace(Application.dataPath, "Assets");
                var asset = LoadAsset<FolderPath>(path);
                if (asset != null)
                {
                    var editor = Editor.CreateEditor(asset);
                    editors.Add(editor);
                }
            }
        }

        private void SaveAndDeleteButton(Editor item)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Save"))
            {
                var asset = (FolderPath)item.target;
                CreateAsset<FolderPath>(asset, SaveAssetPath + asset.Label + ".asset");
            }
            if (GUILayout.Button("Delete"))
            {
                var asset = (FolderPath)item.target;
                DeleteAsset(asset);
                editors.Remove(item);
            }
            GUILayout.EndVertical();
        }

        private void DeleteAsset(FolderPath asset)
        {
            string label = asset.Label;
            string assetPathAndName = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.DeleteAsset(assetPathAndName);
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

        private void CreateAsset<T>(T asset, string Savepath)where T : ScriptableObject
        {
            string assetPathAndName = string.Empty;
            T LoaderAsset = LoadAsset<T>(GetAssetPathAndName(Savepath));
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
            T result = (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
            return result;
        }

        private string GetAssetPathAndName(string Savepath)
        {
            string result = Path.Combine("Assets/", Savepath);
            return result;
        }

        private string GenerateUniqueAssetPath(string Savepath)
        {
            string result = GetAssetPathAndName(Savepath);
            return AssetDatabase.GenerateUniqueAssetPath(result);
        }
    }
}
