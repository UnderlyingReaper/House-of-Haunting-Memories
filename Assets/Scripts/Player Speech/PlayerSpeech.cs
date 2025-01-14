using UnityEngine;

[CreateAssetMenu(fileName = "new Speech Data", menuName = "ScriptableObjects/Player Speech Data")]
public class PlayerSpeechData : ScriptableObject
{
    public AudioClip clip;
    public string text;
    public float time;
}