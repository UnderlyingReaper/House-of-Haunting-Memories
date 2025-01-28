using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TV : MonoBehaviour, IInteractible, ISpecialInteraction
{
    [Header("IInteractible Settings")]
    [SerializeField] private bool allowSpecialInteraction;
    [SerializeField] private int priority;
    [SerializeField] private string mainText = "Watch", text1 = "Turn On", text2 = "Turn Off";
    [SerializeField] private GameObject noise;

    private CanvasGroup _canvasgGroup;
    private AudioSource _audioSource;
    private DayNightHandler _dayNightHandler;
    private bool _isOn;

    public Action OnFinishWatching;


    private void Awake()
    {
        _canvasgGroup = GetComponentInChildren<CanvasGroup>();
        _audioSource = GetComponentInChildren<AudioSource>();

        _dayNightHandler = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<DayNightHandler>();
    }

    public int GetPriority()
    {
        return priority;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(allowSpecialInteraction) StartCoroutine(WatchTV());
        else if(!_isOn) TurnOnTV();
        else TurnOffTV();
    }
    public void InteractStart(Transform interactorTransform) {}

    public void TurnOnTV()
    {
        noise.SetActive(true);
        _audioSource?.Play();
        _isOn = true;
    }
    public void TurnOffTV()
    {
        noise.SetActive(false);
        _audioSource?.Stop();
        _isOn = false;
    }
    
    private IEnumerator WatchTV()
    {
        allowSpecialInteraction = false;

        GameplayInputManager.Instance.enabled = false;
        _canvasgGroup.DOFade(1, 2);

        yield return new WaitForSeconds(7);
        if(_dayNightHandler.isDay) _dayNightHandler.SetAfternoon();

        _canvasgGroup.DOFade(0, 2);
        GameplayInputManager.Instance.enabled = true;

        OnFinishWatching?.Invoke();
    }

    public string GetText()
    {
        if(!enabled) return null;
        else if(allowSpecialInteraction) return mainText;
        else if(_isOn) return text2;
        else return text1;
    }

    public void AllowSpecialInteraction()
    {
        allowSpecialInteraction = true;
    }
}
