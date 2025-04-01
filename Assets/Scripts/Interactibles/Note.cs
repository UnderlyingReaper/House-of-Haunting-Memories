using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour, IInteractible
{
    [SerializeField] bool disableObjOnDisable = true;
    [SerializeField] private int priority;
    [SerializeField] private string interactText;
    [SerializeField] private AudioClip readClip;


    public Action OnInteract;
    private bool _isOpen;
    private CanvasGroup _canvasGroup;
    private AudioSource _audioSource;

    private PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }
    private PlayerControls.UIActions ui {
        get { return GameplayInputManager.Instance.playerControls.UI; }
    }



    private void Awake()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _audioSource = GetComponentInChildren<AudioSource>();

        _canvasGroup.alpha = 0;
    }
    private void Start() => ui.Back.performed += (InputAction.CallbackContext context) => Close();
    private void OnEnable()
    {
        if(disableObjOnDisable) gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        if(disableObjOnDisable) gameObject.SetActive(false);
    }

    public int GetPriority() => priority;
    public string GetText()
    {
        if(!enabled) return null;
        else return interactText;
    }
    public Transform GetTransform() => transform;

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(!_isOpen)
        {
            _audioSource.PlayOneShot(readClip);
            _canvasGroup.DOFade(1, 1);
            _isOpen = true;

            controls.Disable();
            ui.Enable();
        }

        OnInteract?.Invoke();
    }
    public void InteractStart(Transform interactorTransform) {}

    private void Close()
    {
        _canvasGroup.DOFade(0, 1);
        _isOpen = false;

        controls.Enable();
        ui.Disable();
    }
}
