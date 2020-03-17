using System;
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
        Vector2 m_ScrollPosition = Vector2.zero;
        private static string SaveAssetPath = "Assets/FolderManager/StreamingAssets/FolderManager.asset";
        List<bool> showNodeOnList;
        List<string> nodeNameList;
        int count = 0;
        private void OnEnable()
        {
            m_Target = LoadAsset<Folders>(SaveAssetPath);

            if (m_Target == null)
                m_Target = new Folders();

            CellSetting();
        }

        private void CellSetting()
        {
            showNodeOnList = new List<bool>();
            nodeNameList = new List<string>();
            foreach (var item in m_Target.Path)
            {
                showNodeOnList.Add(false);
                nodeNameList.Add(string.Empty);
            }
        }

        public override void OnInspectorGUI()
        {
            using(GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(m_ScrollPosition))
            {
                m_ScrollPosition = scrollViewScope.scrollPosition;
                using(new GUILayout.VerticalScope(new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter }))
                {
                    foreach (var item in m_Target.Path.ToList())
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        FolderPathOnInspectorGUI(item);
                        Debug.Log(count);
                        GUILayout.EndVertical();
                        DeleteCell(item);
                        GUILayout.EndVertical();
                        DrawUILine(Color.grey);
                        count = count < m_Target.Path.Count - 1 ? count + 1 : 0;
                    }
                }
            }
        }

        #region FolderPath Editor
        private void FolderPathOnInspectorGUI(FolderPath asset)
        {
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter };
            GUIStyle textFieldGUI = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.LowerLeft };

            LabelGUI(asset, titleStyle, textFieldGUI);
            RootPathTypeGUI(asset, titleStyle);
            AddNodesGUI(asset, titleStyle, btnStyle);
            ShowPathGUI(asset, titleStyle, btnStyle, textFieldGUI);
        }

        private void LabelGUI(FolderPath asset, GUIStyle TitleStyle, GUIStyle TextFieldGUI)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Label", TitleStyle);
            asset.Label = GUILayout.TextField(asset.Label, TextFieldGUI);
            GUILayout.EndHorizontal();
        }

        private void RootPathTypeGUI(FolderPath asset, GUIStyle TitleStyle)
        {
            GUIStyle popupGUI = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleLeft };
            string[] options = Enum.GetNames(typeof(RootPathType));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path", TitleStyle);
            asset.RootPathType = (RootPathType)EditorGUILayout.Popup((int)asset.RootPathType, options, popupGUI);
            GUILayout.EndHorizontal();
        }

        private void AddNodesGUI(FolderPath asset, GUIStyle TitleStyle, GUIStyle BtnStyle)
        {
            GUIStyle textFieldGUI = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.LowerLeft };
            GUILayout.BeginHorizontal();
            GUILayout.Label("Add Nodes", TitleStyle);
            nodeNameList[count] = GUILayout.TextField(nodeNameList[count], textFieldGUI);
            if (GUILayout.Button("+", BtnStyle))
            {
                if (!string.IsNullOrEmpty(nodeNameList[count]))
                {
                    asset.Node.Add(nodeNameList[count]);
                    nodeNameList[count] = GUILayout.TextField(string.Empty);
                    Debug.Log("+");
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ShowPathGUI(FolderPath asset, GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextField)
        {
            string[] Paths = new string[asset.Node.Count + 1];
            string path;
            Paths[0] = asset.RootPath;
            for (int i = 1; i < Paths.Length; i++)
                Paths[i] = asset.Node[i - 1];
            path = System.IO.Path.Combine(Paths).Replace('\\', '/');

            GUILayout.BeginHorizontal();
            GUILayout.Label("Path Structure", TitleStyle);
            ShowNodesGUI(asset, path, TitleStyle, BtnStyle, TextField);
            GUILayout.EndHorizontal();
        }

        private void ShowNodesGUI(FolderPath asset, string path, GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextField)
        {
            TitleStyle.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            showNodeOnList[count] = EditorGUILayout.Foldout(showNodeOnList[count], path);
            if (showNodeOnList[count])
                ShowNode(asset, TitleStyle, BtnStyle, TextField);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void ShowNode(FolderPath asset, GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextFieldGUI)
        {
            for (int i = 0; i < asset.Node.Count; i++)
            {
                string hyphen = (i < asset.Node.Count - 1) ? "┝ " : "└ ";
                GUILayout.BeginHorizontal();
                GUILayout.Label(hyphen, TitleStyle);
                asset.Node[i] = GUILayout.TextField(asset.Node[i], TextFieldGUI);
                if (GUILayout.Button("-", BtnStyle))
                    asset.Node.RemoveAt(i);
                GUILayout.EndHorizontal();
            }
        }
        #endregion

        public void AddCell()
        {
            var asset = new FolderPath();
            m_Target.Path.Add(asset);
            asset.Label = "LabelPath_" + m_Target.Path.Count;
            CellSetting();
            CreateAsset<Folders>(m_Target, SaveAssetPath);
        }

        public void DeleteCell(FolderPath item)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Delete"))
            {
                m_Target.Path.Remove(item);
                CellSetting();
                CreateAsset<Folders>(m_Target, SaveAssetPath);
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
            T LoaderAsset = LoadAsset<T>(Savepath);
            string assetPathAndName = string.Empty;

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
