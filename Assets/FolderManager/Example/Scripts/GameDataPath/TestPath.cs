using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : MonoBehaviour
{
    [SerializeField] private FolderManager.Folders Folders;

    private void Start()
    {
        foreach (var item in Folders.Path)
        {
            Debug.Log(item.Label);

        }
    }
}
