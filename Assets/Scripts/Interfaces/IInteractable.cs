using UnityEngine;

public interface IInteractible
{
    public void InteractStart(Transform interactorTransform);

    public void InteractPerform(Transform interactorTransform);

    public void InteractCancel(Transform interactorTransform);

    public Transform GetTransform();

    public int GetPriority();
    public string GetText();
}