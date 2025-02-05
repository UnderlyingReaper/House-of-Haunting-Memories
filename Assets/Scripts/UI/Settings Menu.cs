using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private GraphicsSettings graphicsSettings;
    [SerializeField] private RectTransform controlsSettings;

    [SerializeField] private RectTransform cogWheelR;
    [SerializeField] private RectTransform cogWheelL;
    [SerializeField] private float maxTurnAngle;

    public event Action OnSettingsClose;
    


    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public CanvasGroup previousCanvasGroup;





    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        soundSettings = GetComponent<SoundSettings>();

        Volume vol = GameObject.FindGameObjectWithTag("Global Volume").GetComponent<Volume>();
        vol.profile.TryGet(out graphicsSettings.bloom);

        GameplayInputManager.Instance.playerControls.UI.Back.performed += (InputAction.CallbackContext context) => {
            ReturnBack();
        };

        // Resolution Setting
        Resolution[] uniqueResolutions = Screen.resolutions
        .GroupBy(r => new { r.width, r.height }) // Group by width and height
        .Select(g => g.First())                  // Select the first entry from each group
        .OrderByDescending(r => r.width)         // Sort by width in descending order
        .ThenByDescending(r => r.height)         // Then sort by height in descending order
        .ToArray();                              // Convert the result back to an array

        graphicsSettings.resolutions = uniqueResolutions;
        graphicsSettings.resolutionDropDown.ClearOptions();

        List<string> options = new();
        graphicsSettings.orgResolutionIndex = 0;
        for(int i = 0; i < graphicsSettings.resolutions.Length; i++)
        {
            string option = $"{graphicsSettings.resolutions[i].width} x {graphicsSettings.resolutions[i].height}";
            options.Add(option);

            if(graphicsSettings.resolutions[i].width == Screen.currentResolution.width && graphicsSettings.resolutions[i].height == Screen.currentResolution.height)
                graphicsSettings.orgResolutionIndex = i;
        }
        graphicsSettings.resolutionDropDown.AddOptions(options);
        graphicsSettings.resolutionDropDown.value = graphicsSettings.orgResolutionIndex;
        graphicsSettings.prevResolutionIndex = graphicsSettings.orgResolutionIndex;
        graphicsSettings.resolutionDropDown.RefreshShownValue();

        // Graphics Settings
        graphicsSettings.bloomToggle.onValueChanged.AddListener(graphicsSettings.OnBloomToggle);
        graphicsSettings.qualityDropDown.onValueChanged.AddListener(graphicsSettings.OnQualityValueChange);
        graphicsSettings.frameLimitDropDown.onValueChanged.AddListener(graphicsSettings.OnFramesLimitValueChange);
        graphicsSettings.windowModeDropDown.onValueChanged.AddListener(graphicsSettings.OnWindowModeValueChange);
        graphicsSettings.resolutionDropDown.onValueChanged.AddListener(graphicsSettings.OnResolutionValueChange);

        // Sound Settings
        soundSettings.masterVolSlider.onValueChanged.AddListener(soundSettings.OnMasterVolChange);
        soundSettings.sfxVolSlider.onValueChanged.AddListener(soundSettings.OnSFXVolChange);
        soundSettings.musicVolSlider.onValueChanged.AddListener(soundSettings.OnMusicVolChange);

        graphicsSettings.LoadValues();
        soundSettings.LoadValues();
    }





    #region Background Processes
    
    public void RotateCogWheel()
    {
        int rand = UnityEngine.Random.Range(0, 2);

        if(rand == 0)
        {
            cogWheelR.DOLocalRotate(new Vector3(0, 0, UnityEngine.Random.Range(-maxTurnAngle, maxTurnAngle)), 0.5f).SetUpdate(true);
        }
        else
        {
            cogWheelL.DOLocalRotate(new Vector3(0, 0, UnityEngine.Random.Range(-maxTurnAngle, maxTurnAngle)), 0.5f).SetUpdate(true);
        }
    }
    #endregion





    #region UI Buttons
    public void ReturnBack()
    {
        if(canvasGroup.alpha < 0.2f || !gameObject.activeSelf) return;
        
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true);
        previousCanvasGroup.DOFade(1, 0.5f).SetUpdate(true);
        
        previousCanvasGroup.interactable = true;
        previousCanvasGroup.blocksRaycasts = true;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnSettingsClose?.Invoke();
    }

    public void OpenGraphicsTabBtn()
    {
        graphicsSettings.graphicsTab.gameObject.SetActive(true);

        soundSettings.soundTab.gameObject.SetActive(false);
        controlsSettings.gameObject.SetActive(false);
        
        graphicsSettings.graphicsTab.anchoredPosition = new Vector2(graphicsSettings.graphicsTab.anchoredPosition.x, -150);
        graphicsSettings.graphicsTab.DOAnchorPosY(-130, 0.5f).SetUpdate(true);
    }

    public void OpenSoundTabBtn()
    {
        soundSettings.soundTab.gameObject.SetActive(true);

        controlsSettings.gameObject.SetActive(false);
        graphicsSettings.graphicsTab.gameObject.SetActive(false);
        
        soundSettings.soundTab.anchoredPosition = new Vector2(soundSettings.soundTab.anchoredPosition.x, -150);
        soundSettings.soundTab.DOAnchorPosY(-130, 0.5f).SetUpdate(true);
    }

    public void OpenControlsTabBtn()
    {
        controlsSettings.gameObject.SetActive(true);

        soundSettings.soundTab.gameObject.SetActive(false);
        graphicsSettings.graphicsTab.gameObject.SetActive(false);
        
        controlsSettings.anchoredPosition = new Vector2(soundSettings.soundTab.anchoredPosition.x, -150);
        controlsSettings.DOAnchorPosY(-130, 0.5f).SetUpdate(true);
    }
    #endregion
}