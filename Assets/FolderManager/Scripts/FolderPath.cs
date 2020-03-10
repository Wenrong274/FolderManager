using System.Collections.Generic;
using UnityEngine;

namespace FolderManager
{
    public class FolderPath : ScriptableObject
    {
        public string Label = "LabelPath";
        public RootPathType RootPathType;
        public string RootPath;
        public List<string> Node = new List<string>();

        private void Awake()
        {
            RootPath = FolderManager.RootPath.GetFolderPath(RootPathType);
        }
    }
}
