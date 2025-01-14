using UnityEngine;

public class CheckItemInInvCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private InventoryItem inventoryItem;

    public bool IsConditionMet()
    {
        return InventoryManager.Instance.CheckForItem(inventoryItem);
    }
}