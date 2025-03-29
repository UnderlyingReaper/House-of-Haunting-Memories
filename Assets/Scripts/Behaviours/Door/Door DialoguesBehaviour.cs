using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoorDialoguesBehaviour : MonoBehaviour, IDoorBehaviour
{
    [SerializeField] private float firstInteractionStartDelay;
    [SerializeField] private float secondInteractionStartDelay;
    [SerializeField] private List<OneSpeech> firstInteractionspeecheList;
    [SerializeField] private List<OneSpeech> secondInteractionspeecheList;

    private bool _firstinteraction = true;
    private IPlayerSpeak _playerSpeak;
    public Action OnStart;
    public Action OnComplete;


    private PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }
    

    private void Start()
    {
        _playerSpeak = GetComponent<IPlayerSpeak>();
    }

    public void InteractCancel(Door door) {}
    public void InteractPerform(Door door)
    {
        OnStart?.Invoke();
        Sequence sequence = DOTween.Sequence();
        
        sequence.AppendCallback(() => AnimationController.Instance.knock = true)
        .AppendInterval(2.5f)
        .AppendInterval(_firstinteraction ? firstInteractionStartDelay : secondInteractionStartDelay);

        door.audioSource.Play();
        controls.Disable();

        List<OneSpeech> listToUse = _firstinteraction ? firstInteractionspeecheList : secondInteractionspeecheList;
        _firstinteraction = false;
        
        foreach(OneSpeech speech in listToUse)
        {
            sequence.AppendCallback(() => _playerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, speech.speechData));
            sequence.AppendInterval(speech.speechData.time);
            sequence.AppendInterval(speech.pauseTime);
        }

        sequence.OnComplete(() => {
            AnimationController.Instance.knock = false;
            controls.Enable();
            OnComplete?.Invoke();
        });
    }
    public void InteractStart(Door door) {}
}