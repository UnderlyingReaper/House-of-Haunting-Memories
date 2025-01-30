using DG.Tweening;
using UnityEngine;

public class Sink : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string offText = "Turn Off", onText = "Turn On";
    [SerializeField] private ParticleSystem waterVfx;

    [Header("Sound")]
    [SerializeField] private AudioClip stateChangeSound;
    [SerializeField] private AudioClip waterSound;

    private bool _isOn;
    private AudioSource _audioSource;
    private float _orgVol;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _orgVol = _audioSource.volume;
        _audioSource.volume = 0;
    }

    public int GetPriority()
    {
        return priority;
    }

    public string GetText()
    {
        if(!_isOn) return onText;
        else return offText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}

    public void InteractPerform(Transform interactorTransform)
    {
        if(_isOn)
        {
            waterVfx.Stop();
            _isOn = false;

            
            _audioSource.PlayOneShot(stateChangeSound, _orgVol);

            _audioSource.DOKill();
            _audioSource.DOFade(0, 0.5f).OnComplete(() => _audioSource.Stop());
        }
        else
        {
            waterVfx.Play();
            _isOn = true;

            _audioSource.PlayOneShot(stateChangeSound, _orgVol);

            _audioSource.DOKill();
            _audioSource.Play();
            _audioSource.DOFade(_orgVol, 0.5f);
        }
    }

    public void InteractStart(Transform interactorTransform) {}
}
