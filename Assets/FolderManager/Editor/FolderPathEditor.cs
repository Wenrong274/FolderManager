using System;
using UnityEditor;
using UnityEngine;

namespace FolderManager
{
    [CustomEditor(typeof(FolderPath))]
    public class FolderPathEditor : Editor
    {
        FolderPath m_Target;
        string nodeName = string.Empty;
        bool on = false;

        private void OnEnable()
        {
            m_Target = (FolderPath)target;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label) { fixedWidth = 140, alignment = TextAnchor.MiddleLeft };
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 40, alignment = TextAnchor.MiddleCenter };
            GUIStyle textFieldGUI = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.LowerLeft };

            LabelGUI(titleStyle, textFieldGUI);
            RootPathTypeGUI(titleStyle);
            AddNodesGUI(titleStyle, btnStyle);
            ShowPathGUI(titleStyle, btnStyle, textFieldGUI);
        }

        private void LabelGUI(GUIStyle TitleStyle, GUIStyle TextFieldGUI)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Label", TitleStyle);
            m_Target.Label = GUILayout.TextField(m_Target.Label, TextFieldGUI);
            GUILayout.EndHorizontal();
        }

        private void RootPathTypeGUI(GUIStyle TitleStyle)
        {
            GUIStyle popupGUI = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleLeft };
            string[] options = Enum.GetNames(typeof(RootPathType));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path", TitleStyle);
            m_Target.RootPathType = (RootPathType)EditorGUILayout.Popup((int)m_Target.RootPathType, options, popupGUI);
            GUILayout.EndHorizontal();
        }

        private void AddNodesGUI(GUIStyle TitleStyle, GUIStyle BtnStyle)
        {
            GUIStyle textFieldGUI = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.LowerLeft };
            GUILayout.BeginHorizontal();
            GUILayout.Label("Add Nodes", TitleStyle);
            nodeName = GUILayout.TextField(nodeName, textFieldGUI);

            if (GUILayout.Button("+", BtnStyle))
            {
                if (!string.IsNullOrEmpty(nodeName))
                {
                    m_Target.Node.Add(nodeName);
                    nodeName = GUILayout.TextField(string.Empty);
                    Debug.Log("+");
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ShowPathGUI(GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextField)
        {
            string[] Paths = new string[m_Target.Node.Count + 1];
            string path;
            Paths[0] = m_Target.RootPath;
            for (int i = 1; i < Paths.Length; i++)
                Paths[i] = m_Target.Node[i - 1];
            path = System.IO.Path.Combine(Paths).Replace('\\', '/');

            GUILayout.BeginHorizontal();
            GUILayout.Label("Path Structure", TitleStyle);
            ShowNodesGUI(path, TitleStyle, BtnStyle, TextField);
            GUILayout.EndHorizontal();
        }

        private void ShowNodesGUI(string path, GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextField)
        {
            TitleStyle.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            on = EditorGUILayout.Foldout(on, path);
            if (on)
                ShowNode(TitleStyle, BtnStyle, TextField);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void ShowNode(GUIStyle TitleStyle, GUIStyle BtnStyle, GUIStyle TextFieldGUI)
        {
            for (int i = 0; i < m_Target.Node.Count; i++)
            {
                string hyphen = (i < m_Target.Node.Count - 1) ? "┝ " : "└ ";
                GUILayout.BeginHorizontal();
                GUILayout.Label(hyphen, TitleStyle);
                m_Target.Node[i] = GUILayout.TextField(m_Target.Node[i], TextFieldGUI);
                if (GUILayout.Button("-", BtnStyle))
                    m_Target.Node.RemoveAt(i);
                GUILayout.EndHorizontal();
            }
        }
    }
}
