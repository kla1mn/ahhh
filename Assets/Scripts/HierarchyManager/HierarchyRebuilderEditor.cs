using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HierarchyRebuilder))]
public class HierarchyRebuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HierarchyRebuilder rebuilder = (HierarchyRebuilder)target;

        if (GUILayout.Button("Rebuild"))
        {
            rebuilder.RebuildHierarchy();
        }
    }
}
