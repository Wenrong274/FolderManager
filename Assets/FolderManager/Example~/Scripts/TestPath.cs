using System.Collections;
using System.Collections.Generic;
using FolderManager;
using UnityEngine;

public class TestPath : MonoBehaviour
{
    [SerializeField] private FolderManager.Folders Folders;

    private void Start()
    {
        var f = Folders;
        foreach (var item in f.Path)
        {
            Debug.Log(item.Label);
        }
    }
}
