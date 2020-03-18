 using System.Collections.Generic;
 using System.Linq;
 using UnityEngine;

 namespace FolderManager
 {
     public class Folders : ScriptableObject
     {
         [SerializeField] private List<FolderPath> Paths = new List<FolderPath>();

         public void Add(FolderPath arg)
         {
             Paths.Add(arg);
         }

         public void Remove(FolderPath arg)
         {
             Paths.Remove(arg);
         }

         public FolderPath[] GetPaths()
         {
             return Paths.ToArray();
         }

         public FolderPath GetPath(string label)
         {
             return Paths.Where(o => o.Label == label).FirstOrDefault();
         }
     }
 }
