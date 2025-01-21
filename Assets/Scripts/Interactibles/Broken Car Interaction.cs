using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class BrokenCarInteraction : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;

    [SerializeField] private CinemachineCamera virtualCam;
    [SerializeField] private List<oneSpeech> playerSpeecheList;


    private IPlayerSpeak _playerSpeak;
    private AudioSource _audioSource;

    private PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }




    private void Awake()
    {
        _playerSpeak = GetComponent<IPlayerSpeak>();
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.Play();

        controls.Disable();
        virtualCam.gameObject.SetActive(true);

        foreach(oneSpeech speech in playerSpeecheList)
        {
            sequence.AppendCallback(() => _playerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, speech.speechData));
            sequence.AppendInterval(speech.speechData.time);
            sequence.AppendInterval(speech.pauseTime);
        }

        sequence.OnComplete(() => {
            controls.Enable();
            virtualCam.gameObject.SetActive(false);
        });

        enabled = false;
    }

    public void InteractStart(Transform interactorTransform) {}
}

[Serializable]
struct oneSpeech {
    public PlayerSpeechData speechData;
    public float pauseTime;
}