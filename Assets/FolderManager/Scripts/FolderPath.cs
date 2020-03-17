using System.Collections.Generic;
using UnityEngine;

namespace FolderManager
{
    [System.Serializable]
    public class FolderPath
    {
        public string Label = "LabelPath";
        public RootPathType RootPathType;
        public List<string> Node = new List<string>();
        private string m_RootPath;
        public string RootPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_RootPath))
                    m_RootPath = FolderManager.RootPath.GetFolderPath(RootPathType);
                return m_RootPath;
            }
        }
    }
}
