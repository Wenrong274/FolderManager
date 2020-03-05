using System.IO;
using UnityEngine;

namespace Manager
{
    public class FolderManager : MonoBehaviour
    {
        [SerializeField] private static string rootName = "downloads";
        [SerializeField] private static string configName = "config";

        public static string SettingPath
        {
            get { return Path.Combine(ConfigFolder, "setting.json").Replace('\\', '/'); }
        }

        public static string ConfigFolder
        {
            get { return Path.Combine(GetRootPath(), configName); }
        }

        private static string GetRootPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                    return Directory.GetParent(Application.dataPath).FullName.Replace('\\', '/') + "/" + rootName + "/";
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    return Path.Combine(Application.persistentDataPath, rootName) + "/";
                default:
                    return Application.dataPath + "/" + rootName + "/";
            }
        }
    }
}
