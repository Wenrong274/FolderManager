using System.Collections;
using System.Collections.Generic;
using FolderManager;
using UnityEngine;

public class TestPath : MonoBehaviour
{
    [SerializeField] private FolderManager.Folders Folders;
    public List<FolderPath> Path = new List<FolderPath>();

    private void Start()
    {
        foreach (var item in Folders.Path)
        {
            Debug.Log(item.Label);
        }
    }
}
