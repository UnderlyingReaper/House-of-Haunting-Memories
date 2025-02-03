using UnityEngine;

public class Garbage : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;


    public int GetPriority()
    {
        return priority;
    }

    public string GetText()
    {
        if(!enabled) return null;
        else return text;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {

    }
    public void InteractStart(Transform interactorTransform) {}
}
