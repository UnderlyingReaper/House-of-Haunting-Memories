using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractible
{
    [Header("IInteractible Settings")]
    [SerializeField] private int priority;
    [SerializeField] private string text = "Sleep";

    CanvasGroup _canvasGroup;
    DayNightHandler _dayNightHandler;
    public Action OnSleepComplete;

    void Awake()
    {
        _dayNightHandler = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<DayNightHandler>();

        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        _canvasGroup.alpha = 0;
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
        
        StartCoroutine(Sleep());
    }
    public void InteractStart(Transform interactorTransform) {}

    IEnumerator Sleep()
    {
        enabled = true;
        GameplayInputManager.Instance.enabled = false;
        _canvasGroup.DOFade(1, 2);

        yield return new WaitForSeconds(6);

        if(_dayNightHandler.isDay)
        {
            _dayNightHandler.SetNight();
            yield return new WaitForSeconds(2);

            _canvasGroup.DOFade(0, 2);
            GameplayInputManager.Instance.enabled = true;
        }
        
        OnSleepComplete?.Invoke();
        enabled = false;
    }

    public string GetText()
    {
        if(!enabled) return null;
        return text;
    }
}

