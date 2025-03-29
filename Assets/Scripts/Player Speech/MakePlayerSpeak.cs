using UnityEngine;

public class MakePlayerSpeak : MonoBehaviour, IPlayerSpeak
{
    [SerializeField] PlayerSpeechData mainSpeechData;
    [SerializeField] PlayerSpeechData hintSpeechData;

    public void SpeakPlayer(IPlayerSpeak.SpeechType type, PlayerSpeechData customSppechData = null)
    {
        if(type == IPlayerSpeak.SpeechType.Custom && customSppechData) PlayerSpeechManager.Instance.PlayPlayerSpeech(customSppechData);
        else if(type == IPlayerSpeak.SpeechType.Main && mainSpeechData != null) PlayerSpeechManager.Instance.PlayPlayerSpeech(mainSpeechData);
        else if(type == IPlayerSpeak.SpeechType.Hint) PlayerSpeechManager.Instance.PlayPlayerSpeech(hintSpeechData);
    }
}