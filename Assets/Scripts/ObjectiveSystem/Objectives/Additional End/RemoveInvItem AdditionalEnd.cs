using UnityEngine;

public class RemoveInvItemAdditionalEnd : MonoBehaviour, IObjectiveAdditionalEnd
{
    [SerializeField] private InventoryItem item;

    public void AdditionalCode()
    {
        InventoryManager.Instance.RemoveItem(item);
    }
}