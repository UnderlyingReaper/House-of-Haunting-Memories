using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickEnableLocation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Note note;

    private bool _allow = false;
    public Action OnLocate;

    private void Start()
    {
        note.OnInteract += Allow;
    }

    private void Allow() => _allow = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_allow) return;

        LocationButton locationButton = GetComponent<LocationButton>();
        locationButton.enabled = true;
        OnLocate?.Invoke();

        enabled = false;
    }
}