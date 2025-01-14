using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private float maxSizeX;
    [SerializeField] private float time;

    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;


    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(new Vector2(maxSizeX, _rectTransform.sizeDelta.y), time);
        _canvasGroup.DOFade(0, time);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}