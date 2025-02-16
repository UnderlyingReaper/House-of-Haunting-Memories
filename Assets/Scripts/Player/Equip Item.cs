using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipItem : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private Collider2D handCollider;
    [SerializeField] private AudioClip equipSound;

    public event Action OnUnequip;

    private AudioSource _audioSource;


    private void Start()
    {
        GameplayInputManager.Instance.playerControls.Gameplay.EquipItem.performed += ItemHandle;
        _audioSource = GetComponentInChildren<AudioSource>();

        EnableFlashlight flashlight = GetComponent<EnableFlashlight>();

        flashlight.OnFlashlightEnabled += Equip;
    }

    private void ItemHandle(InputAction.CallbackContext context)
    {
        if(InventoryManager.Instance.CheckForItem(inventoryItem) == false) return;

        if(AnimationController.Instance.equipGun)
        {
            AnimationController.Instance.equipGun = false;
            OnUnequip?.Invoke();
            handCollider.enabled = false;
        }
        else Equip();
    }

    private void Equip()
    {
        if(AnimationController.Instance.equipGun) return;

        AnimationController.Instance.equipGun = true;
        handCollider.enabled = true;
        _audioSource.PlayOneShot(equipSound);
    }
}
