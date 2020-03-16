using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FolderManager
{
    [CustomEditor(typeof(Folders))]
    public class FoldersEditor : Editor
    {
        Folders m_Target;
        List<FolderPathEditor> editors = new List<FolderPathEditor>();
        Vector2 m_ScrollPosition = Vector2.zero;
        private static string SaveAssetPath = "Assets/FolderManager/StreamingAssets/FolderManager.asset";

        private void OnEnable()
        {
            m_Target = LoadAsset<Folders>(SaveAssetPath);

            if (m_Target == null)
                m_Target = new Folders();

            foreach (var item in m_Target.Path)
                editors.Add((FolderPathEditor)Editor.CreateEditor(item));
        }

        public override void OnInspectorGUI()
        {
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
                        DeleteCell(item);
                        GUILayout.EndVertical();
                        DrawUILine(Color.grey);
                    }
                }
            }
        }

        public void AddCell()
        {
            var asset = ScriptableObject.CreateInstance<FolderPath>();
            m_Target.Path.Add(asset);
            editors.Add((FolderPathEditor)Editor.CreateEditor(asset));
            asset.Label = "LabelPath_" + m_Target.Path.Count;
            CreateAsset<Folders>(m_Target, SaveAssetPath);
        }

        public void DeleteCell(FolderPathEditor item)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Delete"))
            {
                var asset = (FolderPath)item.target;
                editors.Remove(item);
                m_Target.Path.Remove(asset);
                CreateAsset<Folders>(m_Target, SaveAssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            GUILayout.EndVertical();
        }

        private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
        private static void CreateAsset<T>(T asset, string Savepath)where T : ScriptableObject
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

        private static T LoadAsset<T>(string assetPathAndName)where T : ScriptableObject
        {
            return (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
        }

        private static string GenerateUniqueAssetPath(string Savepath)
        {
            return AssetDatabase.GenerateUniqueAssetPath(Savepath);
        }
    }
}
