using UnityEngine;
using UnityEditor;

namespace Manager
{
    [CustomEditor(typeof(FolderManager))]
    public class FolderManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FolderManager fm = (FolderManager)target;

        }

    }
}
