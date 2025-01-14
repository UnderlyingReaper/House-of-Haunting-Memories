using UnityEngine;

[CreateAssetMenu(fileName = "new Item Data", menuName = "ScriptableObjects/Inventory Item Data")]
public class InventoryItem : ScriptableObject
{
    public Sprite imageSprite;
    public string id;
};