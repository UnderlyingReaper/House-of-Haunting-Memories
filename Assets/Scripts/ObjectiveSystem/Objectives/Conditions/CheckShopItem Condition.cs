using NUnit.Framework;
using UnityEngine;

public class CheckShopItemCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private LaunchApp shopLauncher;

    private ShopApp _shopApp;
    private int _count;
    private bool _isConditionMet;



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
    {           
        foreach(ShopItemSpawner itemSpawner in _shopApp.itemSpawnersList)
        {
            itemSpawner.OnPurchase += OnPurchaseItem;
        }
    }

    private void OnPurchaseItem(ShopItemSpawner spawner)
    {
        _count++;
        spawner.OnPurchase -= OnPurchaseItem;

        if(_count == _shopApp.itemSpawnersList.Count) _isConditionMet = true;
    }
}