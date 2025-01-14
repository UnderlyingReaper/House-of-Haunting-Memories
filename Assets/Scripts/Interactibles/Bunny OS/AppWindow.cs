using DG.Tweening;
using UnityEngine;

public class AppWindow : MonoBehaviour
{
    public GameObject appTask;
    public bool isFocused;
    public bool isMinimized;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void CloseWindow()
    {
        DOTween.Kill(rectTransform);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.DOScale(Vector2.zero, 0.15f).OnComplete(() => Destroy(gameObject));
    }

    public void MinimizeWindow()
    {
        DOTween.Kill(rectTransform);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.DOScale(Vector2.zero, 0.15f);
        isMinimized = true;
    }

    public void OpenWindow()
    {
        DOTween.Kill(rectTransform);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.DOScale(Vector2.one, 0.15f).OnComplete(() => rectTransform.pivot = new Vector2(0.5f, 0.5f));
        isMinimized = false;
    }
}