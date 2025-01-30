using System.Collections.Generic;
using UnityEngine;

public class CheckShopItemCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private LaunchApp shopLauncher;

    private ShopApp _shopApp;
    private int _count;
    private bool _isConditionMet;
    private List<ShopItemSpawner> _purchasableShopItems;



    private void Start()
    {
        shopLauncher.OnWindowLaunch += GetShopItems;
    }

    public bool IsConditionMet()
    {
        return _isConditionMet;
    }

    private void GetShopItems(GameObject window)
    {
        _shopApp = window.GetComponent<ShopApp>();
        _shopApp.OnInitializeComplete += SubscribeToItems;

        
    }

    private void SubscribeToItems()
    {   _purchasableShopItems = new();

        foreach(ShopItemSpawner itemSpawner in _shopApp.itemSpawnersList)
        {
            if(!itemSpawner.isPurchased)
            {
                _purchasableShopItems.Add(itemSpawner);
                itemSpawner.OnPurchase += OnPurchaseItem;
            }
        }
    }

    private void OnPurchaseItem(ShopItemSpawner spawner)
    {
        _count++;
        spawner.OnPurchase -= OnPurchaseItem;

        if(_count == _purchasableShopItems.Count) _isConditionMet = true;
    }
}