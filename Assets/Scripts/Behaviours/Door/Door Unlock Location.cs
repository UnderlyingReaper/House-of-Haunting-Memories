using UnityEngine;

public class DoorUnlockLocation : MonoBehaviour, IDoorBehaviour
{
    [SerializeField] private LocationButton locationButton;



    public void InteractCancel(Door door) {}
    public void InteractPerform(Door door)
    {
        locationButton.enabled = true;
    }
    public void InteractStart(Door door) {}
}