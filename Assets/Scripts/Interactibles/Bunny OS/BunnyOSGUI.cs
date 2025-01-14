using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BunnyOSGUI : MonoBehaviour
{
    public static BunnyOSGUI Instance { get; private set; }

    [SerializeField] Vector3 defaultScale;
    [SerializeField] float defaultScaleDuration;
    [SerializeField] float defaultLaunchDuration;

    [Header("Tasks")]
    [SerializeField] Color hoverColor;
    [SerializeField] float defaultColorDuration;

    [Header("Sound")]
    [SerializeField] AudioClip hover_Clip;
    [SerializeField] AudioClip click_Clip;



    AudioSource _audioSource;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    public void OnHoverEnter(RectTransform rectTransform, bool playSound)
    {
        rectTransform.DOScale(defaultScale, defaultScaleDuration);
        if(playSound) _audioSource.PlayOneShot(hover_Clip);
    }
    public void OnClick()
    {
        _audioSource.PlayOneShot(click_Clip);
    }
    public void OnHoverExit(RectTransform rectTransform, Vector3 orgScale)
    {
        rectTransform.DOScale(orgScale, defaultScaleDuration);
    }

    public void FadeEnter(Image image)
    {
        image.DOFade(1, defaultColorDuration);
        _audioSource.PlayOneShot(hover_Clip);
    }
    public void FadeExit(Image image)
    {
        image.DOFade(0, defaultColorDuration);
    }


    public void OnTaskEnter(Image image)
    {
        image.DOColor(hoverColor, defaultColorDuration);
        _audioSource.PlayOneShot(hover_Clip);
    }
    public void OnTaskClick()
    {
        _audioSource.PlayOneShot(click_Clip);
    }
    public void OnTaskExit(Image image, Color orgColor)
    {
        image.DOColor(orgColor, defaultColorDuration);
    }
    

    public void LaunchApp(RectTransform windowTransform)
    {
        windowTransform.DOScale(Vector3.one, defaultLaunchDuration);
    }
}