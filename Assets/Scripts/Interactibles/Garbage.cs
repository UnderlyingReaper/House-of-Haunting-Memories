using DG.Tweening;
using UnityEngine;

public class Garbage : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;
    [SerializeField] private AudioClip unzipClip;

    private CanvasGroup _canvas;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _canvas = GetComponentInChildren<CanvasGroup>();
    }

    public int GetPriority()
    {
        return priority;
    }

    public string GetText()
    {
        if(!enabled) return null;
        else return text;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        enabled = false;
        _canvas.DOFade(1, 2);
        _audioSource.PlayOneShot(unzipClip);
    }
    public void InteractStart(Transform interactorTransform) {}
}
