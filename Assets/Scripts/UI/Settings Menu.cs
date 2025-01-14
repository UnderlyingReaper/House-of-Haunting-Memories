using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private GraphicsSettings graphicsSettings;
    [SerializeField] private RectTransform cogWheelR;
    [SerializeField] private RectTransform cogWheelL;
    [SerializeField] private float maxTurnAngle;
    


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
        graphicsSettings.resolutions = Screen.resolutions;
        graphicsSettings.resolutionDropDown.ClearOptions();

        List<string> options = new();
        graphicsSettings.orgResolutionIndex = 0;
        for(int i = 0; i < graphicsSettings.resolutions.Length; i++)
        {
            string option = graphicsSettings.resolutions[i].width + "x" + graphicsSettings.resolutions[i].height;
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
        graphicsSettings.resolutionScaleSlider.onValueChanged.AddListener(graphicsSettings.OnResolutionScaleValueChange);
        graphicsSettings.resolutionDropDown.onValueChanged.AddListener(graphicsSettings.OnResolutionValueChange);

        // Sound Settings
        soundSettings.masterVolSlider.onValueChanged.AddListener(soundSettings.OnMasterVolChange);
        soundSettings.sfxVolSlider.onValueChanged.AddListener(soundSettings.OnSFXVolChange);
        soundSettings.musicVolSlider.onValueChanged.AddListener(soundSettings.OnMusicVolChange);

        LoadValues();
    }





    #region Background Processes
    private void LoadValues()
    {
        // Graphics Settings
        graphicsSettings.bloomToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Bloom Setting", 1));
        graphicsSettings.qualityDropDown.value = PlayerPrefs.GetInt("Quality Setting", 2);

        graphicsSettings.frameLimitDropDown.value = PlayerPrefs.GetInt("Frame Rate Limit Setting", 0);
        graphicsSettings.frameLimitDropDown.RefreshShownValue();

        graphicsSettings.windowModeDropDown.value = PlayerPrefs.GetInt("Window Mode Setting", 0);
        graphicsSettings.windowModeDropDown.RefreshShownValue();

        graphicsSettings.resolutionScaleSlider.value = PlayerPrefs.GetFloat("Resolution Scale Setting", 1);

        int indexVal = PlayerPrefs.GetInt("Resolution Setting", graphicsSettings.resolutionDropDown.value);
        graphicsSettings.resolutionDropDown.SetValueWithoutNotify(indexVal);
        graphicsSettings.resolutionDropDown.RefreshShownValue();
        graphicsSettings.ResolutionValueChangeWithoutNotify(indexVal);

        // Sound Settings
        soundSettings.masterVolSlider.value = PlayerPrefs.GetInt("MasterVol", 100);
        soundSettings.sfxVolSlider.value = PlayerPrefs.GetInt("SFXVol", 100);
        soundSettings.musicVolSlider.value = PlayerPrefs.GetInt("MusicVol", 100);
    }
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
    }

    public void OpenGraphicsTabBtn()
    {
        soundSettings.soundTab.gameObject.SetActive(false);
        graphicsSettings.graphicsTab.gameObject.SetActive(true);
        
        graphicsSettings.graphicsTab.anchoredPosition = new Vector2(graphicsSettings.graphicsTab.anchoredPosition.x, -150);
        graphicsSettings.graphicsTab.DOAnchorPosY(-130, 0.5f).SetUpdate(true);
    }

    public void OpenSoundTabBtn()
    {
        soundSettings.soundTab.gameObject.SetActive(true);
        graphicsSettings.graphicsTab.gameObject.SetActive(false);
        
        soundSettings.soundTab.anchoredPosition = new Vector2(soundSettings.soundTab.anchoredPosition.x, -150);
        soundSettings.soundTab.DOAnchorPosY(-130, 0.5f).SetUpdate(true);
    }
    #endregion
}