using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipItem : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItem;
    public event Action OnUnequip;


    private void Start()
    {
        GameplayInputManager.Instance.playerControls.Gameplay.EquipItem.performed += ItemHandle;

        EnableFlashlight flashlight = GetComponent<EnableFlashlight>();

        flashlight.OnFlashlightEnabled += () => {
            AnimationController.Instance.equipGun = true;
        };
    }

    private void ItemHandle(InputAction.CallbackContext context)
    {
        if(InventoryManager.Instance.CheckForItem(inventoryItem) == false) return;

        if(AnimationController.Instance.equipGun)
        {
            AnimationController.Instance.equipGun = false;
            OnUnequip?.Invoke();
        }
        else AnimationController.Instance.equipGun = true;
    }
}
