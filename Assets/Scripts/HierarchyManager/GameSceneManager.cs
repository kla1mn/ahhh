using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GameSceneManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneObject
    {
        public string name; // Имя объекта
        public GameObject prefab; // Префаб объекта
        public Vector3 position; // Позиция объекта
        public string parentHierarchy; // Путь до родительского объекта (например, "Root/Environment/Trees")
    }

    [System.Serializable]
    public class Folder
    {
        public string folderName; // Имя объекта (папки)
        public string parentHierarchy; // Путь до родительского объекта (например, "Root/Environment")
    }

    [Header("Hierarchy Setup")]
    public Folder[] folders; // Список вложенных объектов (папок)

    [Header("Objects Setup")]
    public SceneObject[] sceneObjects; // Список объектов для спавна

    public void BuildHierarchy()
    {
        // Создаём вложенные GameObjects
        foreach (var folder in folders)
        {
            CreateFolder(folder.folderName, folder.parentHierarchy);
        }

        // Создаём объекты
        foreach (var obj in sceneObjects)
        {
            CreateObject(obj);
        }
    }

    private void CreateFolder(string folderName, string parentHierarchy)
    {
        Transform parentTransform = FindOrCreateHierarchy(parentHierarchy);
        Transform folderTransform = parentTransform.Find(folderName);

        if (folderTransform == null)
        {
            GameObject newFolder = new GameObject(folderName);
            newFolder.transform.SetParent(parentTransform);
            newFolder.transform.localPosition = Vector3.zero;
        }
    }

    private Transform FindOrCreateHierarchy(string hierarchyPath)
    {
        if (string.IsNullOrEmpty(hierarchyPath))
        {
            return transform;
        }

        string[] parts = hierarchyPath.Split('/');
        Transform currentParent = transform;

        foreach (string part in parts)
        {
            Transform child = currentParent.Find(part);
            if (child == null)
            {
                GameObject newFolder = new GameObject(part);
                newFolder.transform.SetParent(currentParent);
                newFolder.transform.localPosition = Vector3.zero;
                currentParent = newFolder.transform;
            }
            else
            {
                currentParent = child;
            }
        }

        return currentParent;
    }

    private void CreateObject(SceneObject obj)
    {
        Transform parentTransform = FindOrCreateHierarchy(obj.parentHierarchy);
        if (obj.prefab != null)
        {
            GameObject newObject = PrefabUtility.InstantiatePrefab(obj.prefab) as GameObject;
            if (newObject != null)
            {
                newObject.name = obj.name;
                newObject.transform.SetParent(parentTransform);
                newObject.transform.localPosition = obj.position;
            }
        }
        else
        {
            Debug.LogError($"Префаб для объекта '{obj.name}' не задан.");
        }
    }
}
