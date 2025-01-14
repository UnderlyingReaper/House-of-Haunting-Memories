using System;
using DG.Tweening;
using UnityEngine;

public class ShopItemSpawner : MonoBehaviour, IIconBehaviour
{
    [SerializeField] public bool isPurchased;
    [SerializeField] public ShopItem itemData;
    [SerializeField] private RectTransform soldImgRect;
    public PackedBox box;

    [Header("Shake")]
    [SerializeField] private float duration = 0.5f; // Duration of the shake
    [SerializeField] private float strength = 50f; // Strength of the shake
    [SerializeField] private int vibrato = 10; // Number of shakes
    [SerializeField] private float randomness = 90f; // Randomness of the shake

    private RectTransform rectTransform;
    private Tween _shakeTween;
    public event Action<ShopItemSpawner> OnPurchase;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        box?.gameObject.SetActive(false);
        soldImgRect.localScale = Vector2.zero;

        if(isPurchased) soldImgRect.DOScale(Vector2.one, 0.3f);
    }

    public void Run()
    {
        if(isPurchased)
        {
            _shakeTween?.Kill();
            _shakeTween = rectTransform.DOShakeAnchorPos(duration, strength, vibrato, randomness);
            return;
        }

        OnPurchase?.Invoke(this);
        soldImgRect.DOScale(Vector2.one, 0.3f);
        isPurchased = true;
    }
}