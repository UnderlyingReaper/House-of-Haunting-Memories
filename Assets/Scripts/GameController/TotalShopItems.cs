using System;
using System.Collections.Generic;
using UnityEngine;

public class TotalShopItems : MonoBehaviour
{
    public List<ShopItemStruct> shopItemList;

    public void OnItemBaught(ShopItemSpawner spawner)
    {
        for(int i = 0; i < shopItemList.Count; i++)
        {
            if(shopItemList[i].item == spawner.itemData && shopItemList[i].box == spawner.box)
            {
                ShopItemStruct tempItem = shopItemList[i];
                tempItem.isPurchased = true;
                shopItemList[i] = tempItem;

                spawner.OnPurchase -= OnItemBaught;
                break;
            }
        }
    }
}

[Serializable]
public struct ShopItemStruct
{
    public ShopItem item;
    public PackedBox box;
    public bool isPurchased;
}
