using DG.Tweening;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public static LetterBox Instance { get; private set; }

    [SerializeField] private float transitionDuration = 2;

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
        _currSequence?.Kill();
        _currSequence = DOTween.Sequence().Append(_canvasGroup.DOFade(1, transitionDuration));
    }

    void OnDisable()
    {
        _currSequence?.Kill();
        _currSequence = DOTween.Sequence().Append(_canvasGroup.DOFade(0, transitionDuration));
    }

    void OnDestroy()
    {
        _currSequence?.Kill();
    }
}
