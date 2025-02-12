using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Garbage : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;
    [SerializeField] private AudioClip unzipClip;

    [SerializeField] private string sceneName;

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
        _audioSource.PlayOneShot(unzipClip);

        DOTween.Sequence()
        .Append(_canvas.DOFade(1, 3))
        .AppendInterval(2)
        .OnComplete(() => SceneManager.LoadScene(sceneName));
    }
    public void InteractStart(Transform interactorTransform) {}
}
