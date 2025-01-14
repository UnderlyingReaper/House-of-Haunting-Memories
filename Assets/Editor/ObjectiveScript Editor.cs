#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Objective))]
public class ObjectiveScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector fields
        DrawDefaultInspector();

        // Leave space and add a header
        GUILayout.Space(10);
        GUILayout.Label("Editor Actions", EditorStyles.boldLabel);

        // Get a reference to the target script
        Objective objective = (Objective)target;

        // Add a button to start the objective
        if (GUILayout.Button("Start Objective")) objective.StartObjective();
        if(GUILayout.Button("Finish Objective")) objective.FinishObjective();
    }
}

#endif