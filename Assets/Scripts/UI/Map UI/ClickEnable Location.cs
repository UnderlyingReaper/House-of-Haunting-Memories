using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEnableLocation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Note note;


    private bool _allow = false;

    private void Start()
    {
        note.OnInteract += OnInteract;
    }

    private void OnInteract() => _allow = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_allow) return;

        LocationButton locationButton = GetComponent<LocationButton>();
        locationButton.enabled = true;

        enabled = false;
    }
}