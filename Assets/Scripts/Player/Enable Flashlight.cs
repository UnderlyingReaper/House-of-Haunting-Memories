using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class EnableFlashlight : MonoBehaviour
{
    [SerializeField] private List<LightData> lightList;
    [SerializeField] private AudioClip stateChangeClip;

    private bool _enabled;
    private AudioSource _audioSource;

    public event Action OnFlashlightEnabled;
    public event Action OnFlashlightDisabled;

    private void Start()
    {
        GameplayInputManager.Instance.playerControls.Gameplay.FlashLight.performed += FlashlightHandle;
        _audioSource = GetComponentInChildren<AudioSource>();

        EquipItem equipItem = GetComponent<EquipItem>();

        equipItem.OnUnequip += () => {
            if(_enabled)
            {
                foreach(LightData lightData in lightList)
                    lightData.light.intensity = 0;

                _enabled = false;
                OnFlashlightDisabled?.Invoke();
            }
        };
    }

    private void FlashlightHandle(InputAction.CallbackContext context)
    {
        if(_enabled)
        {
            foreach(LightData lightData in lightList)
                lightData.light.intensity = 0;

            _enabled = false;
            OnFlashlightDisabled?.Invoke();
        }
        else
        {
            foreach(LightData lightData in lightList)
                lightData.light.intensity = lightData.defaultIntensity;

            _enabled = true;
            OnFlashlightEnabled?.Invoke();
        }

        _audioSource.PlayOneShot(stateChangeClip);
    }
}

[Serializable]
struct LightData {
    public Light2D light;
    public float defaultIntensity;
}