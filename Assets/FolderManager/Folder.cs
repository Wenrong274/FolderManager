using UnityEngine;

namespace FolderManager
{
    public class Folder : ScriptableObject
    {
        public RootPathType RootPathType;
        public string rootPath;
        public string[] Node;

        private void Awake()
        {
            rootPath = RootPath.GetFolderPath(RootPathType);

        }
    }
}
