using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private Image fadeGroup;

    [Space(20)]
    [SerializeField] private RectTransform creditsHolder;
    [SerializeField] private float maxYCreditPos;
    [SerializeField] private float scrollTime;

    [Space(20)]
    [SerializeField] private CanvasGroup infoCanvas;
    [SerializeField] private CanvasGroup demoInfoCanvas;


    private CanvasGroup creditsCanvas;


    private void Awake()
    {
        creditsCanvas = creditsHolder.GetComponent<CanvasGroup>();

        fadeGroup.color = new Color(fadeGroup.color.r, // Red
                                    fadeGroup.color.g, // Green
                                    fadeGroup.color.g, // Blue
                                    1);                // Alpha



        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(fadeGroup.DOFade(0, 2))
        .AppendInterval(1)
        .Append(creditsCanvas.DOFade(1, 3))
        .Append(creditsHolder.DOAnchorPosY(maxYCreditPos, scrollTime).SetEase(Ease.Linear));

        if(SceneManager.GetSceneByName("Day 3") != null)
        {
            sequence.Append(infoCanvas.DOFade(1, 3))
            .AppendInterval(3)
            .Append(infoCanvas.DOFade(0, 3));
        }
        else
        {
            sequence.Append(demoInfoCanvas.DOFade(1, 3))
            .AppendInterval(3)
            .Append(demoInfoCanvas.DOFade(0, 3));
        } 

        sequence.Join(fadeGroup.DOFade(1, 3))
        .OnComplete(() => SceneManager.LoadScene("Main Menu"));
    }
}
