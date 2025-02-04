using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
    [SerializeField] DevControls devControls;
    [SerializeField] RectTransform uiHolder;

    [Header("Toggles")]
    [SerializeField] private Toggle hidePlayerToggle;
    [SerializeField] private Toggle hideInteractionPromptToggle;
    [SerializeField] private Toggle hidePlayerSubtitlesToggle;
    [SerializeField] private Toggle hideInventoryToggle;
    [SerializeField] private Toggle hidePlayerLightToggle;
    [SerializeField] private Toggle stopTimeToggle;

    private string keyCodeSequence = "HOHMD"; // The key sequence to activate the dev tool
    private string currentInput = ""; // Tracks the player's current input
    private GameObject _playerBody;
    private GameObject _interactionPrompt;
    private GameObject _playerVoiceSubtitles;
    private CanvasGroup _inventoryHandler;


    private void Awake()
    {
        devControls = new();
    }
    private void Start()
    {
        hidePlayerToggle.onValueChanged.AddListener(HidePlayer);
        hideInteractionPromptToggle.onValueChanged.AddListener(HideInteractionPrompt);
        hidePlayerSubtitlesToggle.onValueChanged.AddListener(HidePlayerSubtitles);
        hideInventoryToggle.onValueChanged.AddListener(HideInventory);
        hidePlayerLightToggle.onValueChanged.AddListener(HidePlayerLight);
        stopTimeToggle.onValueChanged.AddListener(StopTime);
        
        devControls.DevTools.KeySequence.performed += OnSequenceKeyPressed;
    }
    void OnEnable() => devControls.Enable();

    void OnDisable() => devControls.Disable();
    void OnDestroy() => devControls.Disable();


    private void OnSequenceKeyPressed(InputAction.CallbackContext context)
    {
        string key = context.control.displayName;

        currentInput += key;

        // Check if the current input matches the key code sequence
        if (currentInput.Contains(keyCodeSequence))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            uiHolder.DOAnchorPosX(Mathf.Abs(uiHolder.anchoredPosition.x), 0.5f).SetUpdate(true);
            currentInput = "";
        }

        // Limit the length of the current input to avoid excessive memory usage
        if (currentInput.Length > keyCodeSequence.Length)
            currentInput = currentInput.Substring(1);
    }

    private void HidePlayer(bool val)
    {
        if(_playerBody == null) _playerBody = GameObject.Find("Player Body");

        if(val)
        {
            foreach(Transform child in _playerBody.transform)
            {
                if(child.name == "Camera Target") continue;
                else if(child.name == "bone_1") continue;
                else if(child.name == "Light 2D") continue;

                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach(Transform child in _playerBody.transform)
                child.gameObject.SetActive(true);
        }
    }
    private void HideInteractionPrompt(bool val)
    {
        if(_interactionPrompt == null) _interactionPrompt = GameObject.Find("Interaction Prompt");

        _interactionPrompt.SetActive(!val);
    }
    private void HidePlayerSubtitles(bool val)
    {
        if(_playerVoiceSubtitles == null) _playerVoiceSubtitles = GameObject.Find("Player Voice Subtitles handler");

        _playerVoiceSubtitles.SetActive(!val);
    }
    private void HideInventory(bool val)
    {
        if(_inventoryHandler == null) _inventoryHandler = GameObject.Find("Inventory Handler").GetComponent<CanvasGroup>();

        if(val) _inventoryHandler.alpha = 0;
        else _inventoryHandler.alpha = 1;
    }
    private void HidePlayerLight(bool val)
    {
        if(_playerBody == null) _playerBody = GameObject.Find("Player Body");

        if(val)
        {
            foreach(Transform child in _playerBody.transform)
                if(child.name == "Light 2D") child.gameObject.SetActive(false);
        }
        else
        {
            foreach(Transform child in _playerBody.transform)
                if(child.name == "Light 2D") child.gameObject.SetActive(true);
        }
    }
    private void StopTime(bool val)
    {
        if(val) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void CloseButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        uiHolder.DOAnchorPosX(-Mathf.Abs(uiHolder.anchoredPosition.x), 0.5f).SetUpdate(true);
    }
}
