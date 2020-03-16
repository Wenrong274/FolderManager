using System.Collections;
using System.Collections.Generic;
using FolderManager;
using UnityEngine;
using UnityEngine.Networking;

public class GamePathManger : MonoBehaviour
{
    private static GamePathManger instance;

    public static GamePathManger Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GamePathManger");

                instance = go.AddComponent<GamePathManger>();
                instance.initial();
            }
            return instance;
        }
    }
    public Folders Folders;

    private void initial()
    {
        //Application.streamingAssetsPath + "FolderManager.asset";

    }

    IEnumerator Start()

    {
        UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "FolderManager.asset");
        yield return webRequest.SendWebRequest();
        if (!webRequest.isHttpError || !webRequest.isNetworkError)
        {
            AssetBundle asset = DownloadHandlerAssetBundle.GetContent(webRequest);
            var loadAsset = asset.LoadAssetAsync<Folders>("FolderManager");
            yield return loadAsset;
            Debug.Log((Folders)loadAsset.asset != null);
            asset.Unload(false);
        }
    }
}
