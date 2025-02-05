using DG.Tweening;
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    CanvasGroup _image;
    TextMeshProUGUI _textTMP;



    void Awake()
    {
        _image = GetComponent<CanvasGroup>();
        _textTMP = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        if(Interact.Instance == null)
        {
            enabled = false;
            return;
        }
        
        Interact.Instance.OnInteractEnter += DisplayText;
        Interact.Instance.OnInteractExit += HideText;
    }

    private void DisplayText(string desiredText)
    {
        if(!enabled) return;

        _textTMP.text = desiredText;
        _image.DOFade(1, 1);
    }

    private void HideText()
    {
        if(!enabled) return;

        _image.DOFade(0, 1);
    }
}
