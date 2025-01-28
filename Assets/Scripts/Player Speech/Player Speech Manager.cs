using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerSpeechManager : MonoBehaviour
{
    public static PlayerSpeechManager Instance { get; private set;}
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] AudioSource playerVoiceSource;

    Sequence _currTextSequence;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayPlayerSpeech(PlayerSpeechData playerSpeech)
    {
        if(_currTextSequence != null) _currTextSequence.Kill();
        
        if(playerSpeech.clip != null) playerVoiceSource?.PlayOneShot(playerSpeech.clip);

        DisplayText(playerSpeech.text, playerSpeech.time);
    }

    void DisplayText(string text, float time)
    {
        _currTextSequence = DOTween.Sequence()
                                .AppendCallback(() => { displayText.text = text;} )
                                .Append(displayText.DOFade(1, 1))
                                .AppendInterval(time)
                                .Append(displayText.DOFade(0, 1))
                                .SetUpdate(true);
    }
}
