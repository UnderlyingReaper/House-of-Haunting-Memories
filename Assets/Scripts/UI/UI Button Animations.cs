using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private float fadeAmount = 0.5f;
    [SerializeField] private AnimationType animationType;

    public enum AnimationType {
        Fade
    }

    Color _orgColor;
    Image _image;

    void Awake()
    {
        if(animationType == AnimationType.Fade)
        {
            _image = GetComponent<Image>();
            _orgColor = _image.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(animationType == AnimationType.Fade)
            _image.DOColor(new Color(_orgColor.r, _orgColor.g, _orgColor.b, fadeAmount), animationDuration).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(animationType == AnimationType.Fade)
            _image.color = _orgColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(animationType == AnimationType.Fade)
            _image.DOColor(_orgColor, animationDuration).SetUpdate(true);
    }
}
