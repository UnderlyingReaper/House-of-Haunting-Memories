using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDisplayUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private RectTransform lineR, lineL;
    [SerializeField] private RectTransform mainHolder;
    [SerializeField] private RectTransform closeButton;
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup contentCanvas, titleCanvas;



    private Image _crossImage;

    private void Awake()
    {
        mainMenu.OnUpdateInfoBtnClicked += DisplayUpdate;

        contentCanvas.alpha = 0;
        titleCanvas.alpha = 0;
    }
    public void DisplayUpdate()
    {
        _crossImage = closeButton.GetComponent<Image>();

        DOTween.Sequence()
        .Append(mainMenuCanvas.DOFade(0, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineL.DOAnchorPosX(0, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DOAnchorPosX(0, 1.5f).SetEase(Ease.InOutSine))

        .Append(lineL.DORotate(new Vector3(0, 0, 0), 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DORotate(new Vector3(0, 0, 0), 1.5f).SetEase(Ease.InOutSine))
        
        .Append(mainHolder.DOScaleY(1, 1.5f).SetEase(Ease.InOutSine))
        .Join(contentCanvas.DOFade(1, 1.5f).SetEase(Ease.InOutSine))
        .Join(titleCanvas.DOFade(1, 1.5f).SetEase(Ease.InOutSine))

        .Join(lineL.DOAnchorPosY(350, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DOAnchorPosY(-350, 1.5f).SetEase(Ease.InOutSine))

        .Append(_crossImage.DOFade(1, 1))
        .Join(closeButton.DOAnchorPosX(Mathf.Abs(closeButton.anchoredPosition.x), 1));
    }

    public void closeUpdateMenu()
    {
        DOTween.Sequence()
        .Append(_crossImage.DOFade(0, 1f))
        .Join(contentCanvas.DOFade(0, 1f).SetEase(Ease.InOutSine))
        .Join(titleCanvas.DOFade(0, 1f).SetEase(Ease.InOutSine))

        .Join(closeButton.DOAnchorPosX(-Mathf.Abs(closeButton.anchoredPosition.x), 1))
        .Join(mainHolder.DOScaleY(0, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineL.DOAnchorPosY(0, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DOAnchorPosY(0, 1.5f).SetEase(Ease.InOutSine))

        .Append(lineL.DORotate(new Vector3(0, 0, 90), 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DORotate(new Vector3(0, 0, 90), 1.5f).SetEase(Ease.InOutSine))
        
        .Append(lineL.DOAnchorPosX(-1000, 1.5f).SetEase(Ease.InOutSine))
        .Join(lineR.DOAnchorPosX(1000, 1.5f).SetEase(Ease.InOutSine))

        .Append(mainMenuCanvas.DOFade(1, 1.5f).SetEase(Ease.InOutSine));
    }
}
