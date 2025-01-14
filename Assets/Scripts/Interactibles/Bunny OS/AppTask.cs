using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AppTask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject connectedApp;
    public bool isFocused;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float yOffset;

    private Color _orgColor;
    private Image _Image;
    private Sequence _currSequence;


    private void Awake()
    {
        _Image = GetComponent<Image>();
        _orgColor = _Image.color;

        isFocused = true;
    }

    private void OnEnable()
    {
        rectTransform.anchoredPosition = new Vector2(0, yOffset);
        rectTransform.localScale = Vector2.zero;

        rectTransform.DOAnchorPosY(0, 0.3f);
        rectTransform.DOScale(Vector2.one, 0.3f);
    }

    public void CloseTask()
    {
        rectTransform.DOAnchorPosY(yOffset, 0.3f);
        rectTransform.DOScale(Vector2.zero, 0.3f).OnComplete(() => Destroy(gameObject));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BunnyOSGUI.Instance.OnTaskClick();
        _currSequence?.Kill();
        
        if(!isFocused)
        {
            BunnyOSTaskManager.Instance.LaunchApp(connectedApp);
            PlayFocusAnimation();
        }
        else
        {
            BunnyOSTaskManager.Instance.MinimizeApp(connectedApp.GetComponent<AppWindow>());
            PlayUnfocusAnimation();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        BunnyOSGUI.Instance.OnTaskEnter(_Image);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        BunnyOSGUI.Instance.OnTaskExit(_Image, _orgColor);
    }

    public void PlayFocusAnimation()
    {
        _currSequence = DOTween.Sequence()
        .Append(rectTransform.DOScale(new Vector2(0.7f, 0.7f), 0.1f))
        .AppendCallback(() => {
            rectTransform.DOScale(Vector2.one, 0.2f);
        })
        .Append(rectTransform.DOAnchorPosY(0.008f, 0.2f))
        .Append(rectTransform.DOAnchorPosY(0, 0.2f));
    }
    public void PlayUnfocusAnimation()
    {
        _currSequence = DOTween.Sequence()
        .Append(rectTransform.DOScale(new Vector2(0.7f, 0.7f), 0.1f))
        .AppendCallback(() => {
            rectTransform.DOScale(Vector2.one, 0.2f);
        })
        .Append(rectTransform.DOAnchorPosY(-0.008f, 0.2f))
        .Append(rectTransform.DOAnchorPosY(0, 0.2f));
    }
}