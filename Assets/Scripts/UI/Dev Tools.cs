using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
    public static DevTools instance;

    public DevControls devControls;
    [SerializeField] RectTransform uiHolder;

    [Header("Refrences")]
    [SerializeField] private GameObject devCamera;

    [Header("Toggles")]
    [SerializeField] private Toggle hidePlayerToggle;
    [SerializeField] private Toggle hideInteractionPromptToggle;
    [SerializeField] private Toggle hidePlayerSubtitlesToggle;
    [SerializeField] private Toggle hideInventoryToggle;
    [SerializeField] private Toggle hidePlayerLightToggle;
    [SerializeField] private Toggle stopTimeToggle;
    [SerializeField] private Toggle hideLetterBoxToggle;
    [SerializeField] private Toggle enableDevCameraToggle;

    [Header("Sliders")]
    [SerializeField] private Slider camSpeedSlider;
    [SerializeField] private Slider camZoomSlider;

    private string keyCodeSequence = "HOHMD"; // The key sequence to activate the dev tool
    private string currentInput = ""; // Tracks the player's current input
    private GameObject _playerBody;
    private GameObject _interactionPrompt;
    private GameObject _playerVoiceSubtitles;
    private CanvasGroup _inventoryHandler;
    private GameObject _letterBoxHandler;
    private SimpleCameraController _devCamController;
    private CinemachineCamera _devCinemachine;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            devControls = new();
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        // Toggles
        hidePlayerToggle.onValueChanged.AddListener(HidePlayer);
        hideInteractionPromptToggle.onValueChanged.AddListener(HideInteractionPrompt);
        hidePlayerSubtitlesToggle.onValueChanged.AddListener(HidePlayerSubtitles);
        hideInventoryToggle.onValueChanged.AddListener(HideInventory);
        hidePlayerLightToggle.onValueChanged.AddListener(HidePlayerLight);
        stopTimeToggle.onValueChanged.AddListener(StopTime);
        hideLetterBoxToggle.onValueChanged.AddListener(HideLetterBox);
        enableDevCameraToggle.onValueChanged.AddListener(EnableDevCamera);

        // Sliders
        camSpeedSlider.onValueChanged.AddListener(CameraSpeedSlider);
        camZoomSlider.onValueChanged.AddListener(CameraZoomSlider);
        
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


    #region Toggles
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
    private void HideLetterBox(bool val)
    {
        if(_letterBoxHandler == null) _letterBoxHandler = GameObject.Find("Letter Box");

        _letterBoxHandler.SetActive(!val);
    }
    private void EnableDevCamera(bool val)
    {
        devCamera.SetActive(val);
    }
    #endregion



    #region Sliders
    private void CameraSpeedSlider(float val)
    {
        if(_devCamController == null) _devCamController = devCamera.GetComponent<SimpleCameraController>();
        _devCamController.movementSpeed = val;
    }
    private void CameraZoomSlider(float val)
    {
        if (_devCinemachine == null) _devCinemachine = devCamera.GetComponent<CinemachineCamera>();
        _devCinemachine.Lens.OrthographicSize = val;
    }
    #endregion



    public void CloseButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        uiHolder.DOAnchorPosX(-Mathf.Abs(uiHolder.anchoredPosition.x), 0.5f).SetUpdate(true);
    }
}
