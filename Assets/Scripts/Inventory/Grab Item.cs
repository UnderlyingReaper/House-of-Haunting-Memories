using UnityEngine;

public class GrabItem : MonoBehaviour, IInteractible
{
    [SerializeField] private InventoryItem invItem;
    [SerializeField] private int priority;
    [SerializeField] private string text;

    public void InteractStart(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        InventoryManager.Instance.AddItem(invItem);
        Destroy(gameObject);
    }
    public void InteractCancel(Transform interactorTransform) {}

    public Transform GetTransform() => transform;
    public int GetPriority() => priority;
    public string GetText()
    {
        return text;
    }

}
