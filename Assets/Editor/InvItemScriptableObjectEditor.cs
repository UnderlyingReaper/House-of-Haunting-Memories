#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryItem))]
public class ItemScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get a reference to the Scriptable Object
        InventoryItem item = (InventoryItem)target;

        // Display the sprite in the inspector
        if (item.imageSprite != null)
        {
            GUILayout.Label("Icon Preview:");
            GUILayout.Label(item.imageSprite.texture, GUILayout.Width(64), GUILayout.Height(64));
        }
    }
}

#endif