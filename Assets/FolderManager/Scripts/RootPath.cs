using System.IO;
using UnityEngine;

namespace FolderManager
{
    public enum RootPathType
    {
        ProjectPath,
        ProjectDataPath,
        PersistentDataPath,
        StreamingAssetsPath,
        TemporaryCachePath
    }

    public class RootPath
    {
        public static string GetFolderPath(RootPathType arg)
        {
            switch (arg)
            {
                case RootPathType.ProjectPath:
                    return Directory.GetParent(Application.dataPath).FullName.Replace('\\', '/');
                case RootPathType.ProjectDataPath:
                    return Application.dataPath.Replace('\\', '/');
                case RootPathType.PersistentDataPath:
                    return Application.persistentDataPath.Replace('\\', '/');
                case RootPathType.StreamingAssetsPath:
                    return Application.streamingAssetsPath.Replace('\\', '/');
                case RootPathType.TemporaryCachePath:
                    return Application.temporaryCachePath.Replace('\\', '/');
                default:
                    return string.Empty;
            }
        }
    }
}
