using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class DereksCar : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip carInteract;



    private CanvasGroup _canvas;
    private MapUI _mapUI;
    private bool _isOpen;
    private bool _isMoving;


    private PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }
    private PlayerControls.UIActions ui {
        get { return GameplayInputManager.Instance.playerControls.UI; }
    }



    private void Awake()
    {
        _canvas = GetComponentInChildren<CanvasGroup>();
        _mapUI = _canvas.GetComponent<MapUI>();

        _mapUI.OnMoveStart += () => _isMoving = true;
        _mapUI.OnMoveEnd += () => _isMoving = false;
    }
    private void Start()
    {
        ui.Back.performed += (InputAction.CallbackContext context) => {
            if(_isOpen && !_isMoving) Close();
            
        };
    }

    public int GetPriority()
    {
        return priority;
    }
    public string GetText()
    {
        return text;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(_isOpen) return;

        controls.Disable();
        ui.Enable();
        LetterBox.Instance.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _canvas.DOFade(1, 1);
        _mapUI.enabled = true;
        _isOpen = true;

        audioSource.PlayOneShot(carInteract);
    }
    public void InteractStart(Transform interactorTransform) {}

    private void Close()
    {
        controls.Enable();
        ui.Disable();
        LetterBox.Instance.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource.PlayOneShot(carInteract);

        _canvas.DOFade(0, 2);
        _mapUI.enabled = false;
        _isOpen = false;
    }
}
