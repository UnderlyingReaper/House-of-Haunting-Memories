using DG.Tweening;
using UnityEngine;

public class EaseSoundOnLoad : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    private AudioSource _source;
    private float _maxVol;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _maxVol = _source.volume;
        _source.volume = 0;
    }

    private void OnEnable()
    {
        _source.DOFade(_maxVol, duration);
    }
}
