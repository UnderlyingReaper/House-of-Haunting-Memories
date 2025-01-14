using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BrokenCarInteraction : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;
    [SerializeField] private List<oneSpeech> playerSpeecheList;


    private IPlayerSpeak _playerSpeak;




    private void Awake()
    {
        _playerSpeak = GetComponent<IPlayerSpeak>();
    }

    public int GetPriority()
    {
        return priority;
    }
    public string GetText()
    {
        return text;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}

    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        Sequence sequence = DOTween.Sequence();

        foreach(oneSpeech speech in playerSpeecheList)
        {
            sequence.AppendCallback(() => _playerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, speech.speechData));
            sequence.AppendInterval(speech.speechData.time);
            sequence.AppendInterval(speech.pauseTime);
        }

        enabled = false;
    }

    public void InteractStart(Transform interactorTransform) {}
}

[Serializable]
struct oneSpeech {
    public PlayerSpeechData speechData;
    public float pauseTime;
}