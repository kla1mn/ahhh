using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HierarchyRebuilder : MonoBehaviour
{
    [System.Serializable]
    public class TagHierarchyMapping
    {
        public string tag;
        public string targetFolderPath; // Путь к папке, как строка
    }

    public List<TagHierarchyMapping> tagHierarchyMappings = new();

    // Перемещение объектов с тегами в заданные папки
    public void RebuildHierarchy()
    {
        foreach (GameObject rootObj in GetRootObjects())
        {
            foreach (var mapping in tagHierarchyMappings)
            {
                if (rootObj.CompareTag(mapping.tag))
                {
                    Transform targetParent = GetOrCreateHierarchy(mapping.targetFolderPath);
                    if (targetParent != null)
                    {
                        Vector3 originalPosition = rootObj.transform.position;
                        rootObj.transform.SetParent(targetParent);
                        rootObj.transform.position = originalPosition;
                    }
                }
            }
        }
    }

    private Transform GetOrCreateHierarchy(string folderPath)
    {
        return GameObject.Find(folderPath).transform;
    }


    private List<GameObject> GetRootObjects()
    {
        var rootObjects = new List<GameObject>();
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.transform.parent == null)
            {
                rootObjects.Add(obj);
            }
        }
        return rootObjects;
    }
}
