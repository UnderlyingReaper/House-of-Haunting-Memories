using UnityEngine;

public class PlayerSpeakAnimationTrigger : MonoBehaviour
{
    public void SpeakPlayer(PlayerSpeechData customSppechData)
    {
        PlayerSpeechManager.Instance?.PlayPlayerSpeech(customSppechData);
    }
}