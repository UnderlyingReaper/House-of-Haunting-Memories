using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class Microwave : MonoBehaviour, IInteractible, ISpecialInteraction
{
    [Header("IInteractible Settings")]
    [SerializeField] private bool allowSpecialInteraction;
    [SerializeField] private int priority;
    [SerializeField] private string text = "Heat";
    [SerializeField] private string text2 = "Take out";

    [Space(20)]
    [SerializeField] InventoryItem itemToHeat;
    [SerializeField] InventoryItem heatedItem;
    [SerializeField] SpriteRenderer chamberSprite;
    [SerializeField] Color glowColor;
    [SerializeField] float heatingTime;

    [Header("Player Speech")]
    [SerializeField] MonoBehaviour _playerSpeakMono;
    [SerializeField] PlayerSpeechData itsHeatingSpeechData;
    [SerializeField] PlayerSpeechData dontHeatFingerData;

    [Header("Audio")]
    [SerializeField] AudioClip heatingEndClip;

    public Action OnFoodGetOut;

    
    IPlayerSpeak _playerSpeak;
    AudioSource _audioSource;
    bool _isPrepared;
    bool _isHeating;
    Color _orgColor;


    void Awake()
    {
        _playerSpeak = _playerSpeakMono as IPlayerSpeak;
        _orgColor = chamberSprite.color;
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void InteractCancel(Transform interactorTransform) {}

    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;
        
        if(!allowSpecialInteraction)
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, dontHeatFingerData);
            return;
        }

        if(allowSpecialInteraction && InventoryManager.Instance.CheckForItem(itemToHeat) == false && !_isPrepared && !_isHeating)
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            return;
        }
        else if(allowSpecialInteraction && _isHeating)
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, itsHeatingSpeechData);
            return;
        }

        
        if(!_isPrepared) StartCoroutine(heatFood());
        else if(_isPrepared && InventoryManager.Instance.CheckForItem(heatedItem) == false)
        {
            InventoryManager.Instance.AddItem(heatedItem);
            OnFoodGetOut?.Invoke();
            allowSpecialInteraction = false;
        }
        else
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Hint);
        }
    }
    public void InteractStart(Transform interactorTransform) {}
    public int GetPriority()
    {
        return priority;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    IEnumerator heatFood()
    {
        _isHeating = true;
        InventoryManager.Instance.RemoveItem(itemToHeat);
        chamberSprite.DOColor(glowColor, 0.15f);

        _audioSource.volume = 0;
        _audioSource.DOFade(0.3f, 1);
        _audioSource.Play();

        yield return new WaitForSeconds(heatingTime);

        _isPrepared = true;
        _isHeating = false;
        chamberSprite.color = _orgColor;

        _audioSource.Stop();
        _audioSource.PlayOneShot(heatingEndClip);
    }

    public string GetText()
    {
        if(!enabled) return null;
        else if(_isHeating || (_isPrepared && InventoryManager.Instance.CheckForItem(heatedItem) == false)) return text2;
        else return text;
    }

    public void AllowSpecialInteraction()
    {
        allowSpecialInteraction = true;
    }
}