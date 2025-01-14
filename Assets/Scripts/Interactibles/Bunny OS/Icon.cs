using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private bool changeRect = true;
    [SerializeField] private MonoBehaviour behaviour;
    [SerializeField] private bool playHoverSound;

    private Vector3 _orgSize;
    private Image _image;
    private RectTransform _rectTransform;
    private IIconBehaviour _appBehaviour;


    private void Awake()
    {
        if(changeRect) 
        {
            _rectTransform = GetComponent<RectTransform>();
            _orgSize = _rectTransform.localScale;
        }
        else _image = GetComponent<Image>();

        _appBehaviour = behaviour as IIconBehaviour;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BunnyOSGUI.Instance.OnClick();
        _appBehaviour?.Run();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(changeRect) BunnyOSGUI.Instance.OnHoverEnter(_rectTransform, playHoverSound);
        else BunnyOSGUI.Instance.FadeEnter(_image);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(changeRect) BunnyOSGUI.Instance.OnHoverExit(_rectTransform, _orgSize);
        else BunnyOSGUI.Instance.FadeExit(_image);
    }
}