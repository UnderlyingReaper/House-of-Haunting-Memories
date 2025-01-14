#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSpeechData))]
public class PlayerSpeechDataScriptEditor : Editor
{
    private static AudioSource previewAudioSource;

    public override void OnInspectorGUI()
    {
        // Draw the default Inspector fields
        DrawDefaultInspector();

        // Get a reference to the target script
        PlayerSpeechData speechData = (PlayerSpeechData)target;

        // Add a play button in the inspector
        if (speechData.clip != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Audio Preview", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Play Audio")) PlayClip(speechData.clip);
            if (GUILayout.Button("Stop Audio")) StopClip();
        }
        else EditorGUILayout.HelpBox("No AudioClip assigned.", MessageType.Info);
    }

    private void OnEnable()
    {
        PlayerSpeechData speechData = (PlayerSpeechData)target;

        if (speechData.clip != null) PlayClip(speechData.clip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (previewAudioSource == null)
        {
            GameObject audioSourceObject = new GameObject("EditorAudioSource");
            previewAudioSource = audioSourceObject.AddComponent<AudioSource>();
            previewAudioSource.hideFlags = HideFlags.HideAndDontSave;
        }

        previewAudioSource.clip = clip;
        previewAudioSource.Play();
    }

    private void StopClip()
    {
        if (previewAudioSource != null && previewAudioSource.isPlaying) previewAudioSource.Stop();
    }
}

#endif