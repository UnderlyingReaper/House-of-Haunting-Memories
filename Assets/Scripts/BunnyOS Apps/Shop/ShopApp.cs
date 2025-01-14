using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopApp : MonoBehaviour
{
    [SerializeField] Transform itemsHolder;
    [SerializeField] TextMeshProUGUI noItemsTxt;
    [SerializeField] GameObject defaultStyle;
    public List<ShopItemSpawner> itemSpawnersList;

    [Header("Custom Item Code Menu")]
    [SerializeField] private string itemCode;
    [SerializeField] private RectTransform menu;
    [SerializeField] private TMP_InputField inputField;

    public event Action OnInitializeComplete;

    private bool _isOpen;






    void Start()
    {
        itemSpawnersList = new();
        TotalShopItems totalShopItems = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<TotalShopItems>();

        if (totalShopItems.shopItemList == null || totalShopItems.shopItemList.Count == 0)
        {
            noItemsTxt.gameObject.SetActive(true);
            return;
        }
        else noItemsTxt.gameObject.SetActive(false);

        foreach (ShopItemStruct shopItem in totalShopItems.shopItemList)
        {
            CreateShopItem(shopItem, totalShopItems);
        }

        
        OnInitializeComplete?.Invoke();
    }

    private void CreateShopItem(ShopItemStruct shopItem, TotalShopItems totalShopItems)
    {
        RectTransform iconInst = Instantiate(shopItem.item.customIconStyle != null ? shopItem.item.customIconStyle : defaultStyle, itemsHolder).GetComponent<RectTransform>();
        iconInst.anchoredPosition = Vector2.zero;

        ShopItemSpawner spawner = iconInst.GetComponent<ShopItemSpawner>();
        spawner.box = shopItem.box;
        spawner.isPurchased = shopItem.isPurchased;
        spawner.itemData = shopItem.item;
        if (!spawner.isPurchased) spawner.OnPurchase += totalShopItems.OnItemBaught;

        foreach (Transform childObj in iconInst)
        {
            UpdateChildObject(childObj, shopItem);
        }

        itemSpawnersList.Add(spawner);
    }

    private void UpdateChildObject(Transform childObj, ShopItemStruct shopItem)
    {
        if (childObj.name.Contains("Image"))
        {
            Image img = childObj.GetComponent<Image>();
            img.sprite = shopItem.item.sprite;
            img.color = shopItem.item.spriteCustomeColor;
        }
        else if (childObj.name.Contains("Price"))
        {
            childObj.GetComponent<TextMeshProUGUI>().text = $"${shopItem.item.price}";
        }
        else if (childObj.name.Contains("Name"))
        {
            childObj.GetComponent<TextMeshProUGUI>().text = shopItem.item.name;
        }
    }

    public void OnArrowClick(RectTransform icon)
    {
        if(!_isOpen)
        {
            _isOpen = true;
            icon.DORotate(new Vector3(0, 0, 180), 0.5f);
            menu.DOAnchorPosY(Mathf.Abs(menu.anchoredPosition.y), 0.5f);
        }
        else
        {
            _isOpen = false;
            icon.DORotate(new Vector3(0, 0, 0), 0.5f);
            menu.DOAnchorPosY(-Mathf.Abs(menu.anchoredPosition.y), 0.5f);
        }
    }

    public void OnSubmit()
    {
        if(SyncDataManager.Instance.HasRope)
        {
            DOTween.Sequence()
            .AppendCallback(() => inputField.text = "Already Ordered!")
            .AppendInterval(1)
            .AppendCallback(() => inputField.text = null);
        }
        else if(inputField.text == itemCode)
        {
            DOTween.Sequence()
            .AppendCallback(() => inputField.text = "Ordered!")
            .AppendInterval(1)
            .AppendCallback(() => inputField.text = null);

            SyncDataManager.Instance.HasRope = true;
        }
        else
        {
            DOTween.Sequence()
            .AppendCallback(() => inputField.text = "Not Found!")
            .AppendInterval(1)
            .AppendCallback(() => inputField.text = null);
        }
    }
}