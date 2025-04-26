using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSceneManager))]
public class GameSceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Рендер стандартного интерфейса
        DrawDefaultInspector();

        // Получение ссылки на объект
        GameSceneManager manager = (GameSceneManager)target;

        // Кнопка Build
        if (GUILayout.Button("Build"))
        {
            manager.BuildHierarchy();
        }
    }
}
