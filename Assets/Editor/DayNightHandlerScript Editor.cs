using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DayNightHandler))]
public class DayNightHandlerScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Editor Actions", EditorStyles.boldLabel);

        DayNightHandler dayNightHandler = (DayNightHandler)target;

        if(GUILayout.Button("Set Morning")) dayNightHandler.SetMorning();
        if(GUILayout.Button("Set Afternoon")) dayNightHandler.SetAfternoon();
        if(GUILayout.Button("Set Night")) dayNightHandler.SetNight();
    }
}