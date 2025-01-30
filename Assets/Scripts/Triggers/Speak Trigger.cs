using UnityEngine;

public class SpeakTrigger : MonoBehaviour
{
    private IPlayerSpeak _playerSpeak;

    private void Start() => _playerSpeak = GetComponent<IPlayerSpeak>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!enabled) return;
        
        if(other.tag == "Player")
        {
            _playerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            enabled = false;
        }
    }
}