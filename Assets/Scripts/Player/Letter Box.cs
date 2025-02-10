using DG.Tweening;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public static LetterBox Instance { get; private set; }

    [SerializeField] private float transitionDuration = 2;
    [SerializeField] private RectTransform top, bottom;

    private CanvasGroup _canvasGroup;
    private Sequence _currSequence;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        if(!gameObject.activeSelf) return;

        _currSequence?.Kill();
        _currSequence = DOTween.Sequence()
        .Append(top.DOAnchorPosY(-Mathf.Abs(top.anchoredPosition.y), transitionDuration))
        .Join(bottom.DOAnchorPosY(Mathf.Abs(bottom.anchoredPosition.y), transitionDuration));
    }

    void OnDisable()
    {
        if(!gameObject.activeSelf) return;
        
        _currSequence?.Kill();
        _currSequence = DOTween.Sequence()
        .Append(top.DOAnchorPosY(Mathf.Abs(top.anchoredPosition.y), transitionDuration))
        .Join(bottom.DOAnchorPosY(-Mathf.Abs(bottom.anchoredPosition.y), transitionDuration));
    }

    void OnDestroy()
    {
        _currSequence?.Kill();
    }
}
