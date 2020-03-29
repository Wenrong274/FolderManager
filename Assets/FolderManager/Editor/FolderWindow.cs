using UnityEditor;
using UnityEngine;

namespace FolderManager
{
    public class FolderWindow : EditorWindow
    {
        FoldersEditor FoldersEditor;

        [MenuItem("Folder Manage/Edit Path")]
        static void Init()
        {
            FolderWindow window = (FolderWindow)EditorWindow.GetWindow(typeof(FolderWindow));
            window.Show();
        }

        private void InitAssetEdit()
        {
            if (FoldersEditor == null)
                FoldersEditor = new FoldersEditor();
        }

        void OnGUI()
        {
            InitAssetEdit();
            CreatePath();
            DrawUILine(Color.grey);
            FoldersEditor.OnInspectorGUI();
        }

        private void CreatePath()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button) { fixedWidth = 280f, fixedHeight = 35, fontSize = 14 };
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Path", style))
                FoldersEditor.AddCell();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
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
    }
}
