using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSceneManager))]
public class GameSceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ������ ������������ ����������
        DrawDefaultInspector();

        // ��������� ������ �� ������
        GameSceneManager manager = (GameSceneManager)target;

        // ������ Build
        if (GUILayout.Button("Build"))
        {
            manager.BuildHierarchy();
        }
    }
}
