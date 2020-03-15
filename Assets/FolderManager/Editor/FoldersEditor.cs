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
        List<Editor> editors = new List<Editor>();

        private void OnEnable()
        {
            m_Target = (Folders)target;
            foreach (var item in m_Target.Path)
                editors.Add(Editor.CreateEditor(item));
        }

        public override void OnInspectorGUI()
        {
            foreach (var item in editors.ToList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                item.OnInspectorGUI();
                GUILayout.EndVertical();
                DeleteButton(item);
                GUILayout.EndVertical();
                DrawUILine(Color.grey);
            }
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

        private void DeleteButton(Editor item)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Delete"))
            {
                var asset = (FolderPath)item.target;
                editors.Remove(item);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            GUILayout.EndVertical();
        }
    }
}
