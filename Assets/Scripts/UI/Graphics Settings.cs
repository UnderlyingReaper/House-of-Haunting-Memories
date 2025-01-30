using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private SettingsMenu settingsMenu;

    [Space(20)]
    public RectTransform graphicsTab;
    public Toggle bloomToggle;
    public TMP_Dropdown qualityDropDown;
    public TMP_Dropdown frameLimitDropDown;
    public TMP_Dropdown windowModeDropDown;
    public TextMeshProUGUI resolutionScaleDisplay;
    public TMP_Dropdown resolutionDropDown;
    public GameObject confirmResolutionMenu;
    public TextMeshProUGUI countDownTMP;


    [HideInInspector] public int orgResolutionIndex;
    [HideInInspector] public int prevResolutionIndex;

    private int _currResolutionIndex;

    [HideInInspector] public Bloom bloom;
    [HideInInspector] public Resolution[] resolutions;


    // Apply Settings Val Holder
    private FullScreenMode _fullScreenModeChosen;
    private int _fullScreenModeChosenInt;




    public void DefaultGraphicsSettingsBtn()
    {
        bloomToggle.isOn = true;
        qualityDropDown.value = 2;

        frameLimitDropDown.value = 0;
        frameLimitDropDown.RefreshShownValue();

        windowModeDropDown.value = 0;
        windowModeDropDown.RefreshShownValue();

        resolutionDropDown.value = orgResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void ApplyBtn()
    {
        // Fullscreen Mode
        Screen.fullScreenMode = _fullScreenModeChosen;
        PlayerPrefs.SetInt("Window Mode Setting", _fullScreenModeChosenInt);
    }

    public void LoadValues()
    {
        settingsMenu.OnSettingsClose += OnSettingsClose;
        
        // Graphics Settings
        bloomToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Bloom Setting", 1));
        qualityDropDown.value = PlayerPrefs.GetInt("Quality Setting", 2);

        qualityDropDown.value = PlayerPrefs.GetInt("Quality Setting", 2);
        qualityDropDown.RefreshShownValue();

        frameLimitDropDown.value = PlayerPrefs.GetInt("Frame Rate Limit Setting", 0);
        frameLimitDropDown.RefreshShownValue();

        windowModeDropDown.value = PlayerPrefs.GetInt("Window Mode Setting", 0);
        windowModeDropDown.RefreshShownValue();

        int indexVal = PlayerPrefs.GetInt("Resolution Setting", resolutionDropDown.value);
        resolutionDropDown.SetValueWithoutNotify(indexVal);
        resolutionDropDown.RefreshShownValue();
        ResolutionValueChangeWithoutNotify(indexVal);
    }

    public void OnSettingsClose()
    {
        windowModeDropDown.value = PlayerPrefs.GetInt("Window Mode Setting", 0);
        windowModeDropDown.RefreshShownValue();

        int indexVal = PlayerPrefs.GetInt("Resolution Setting", resolutionDropDown.value);
        resolutionDropDown.SetValueWithoutNotify(indexVal);
        resolutionDropDown.RefreshShownValue();
        ResolutionValueChangeWithoutNotify(indexVal);
    }


    public void OnBloomToggle(bool isOn)
    {
        if(isOn) bloom.intensity.value = 1.1f;
        else if(!isOn) bloom.intensity.value = 0;

        PlayerPrefs.SetInt("Bloom Setting", Convert.ToInt32(isOn));
        settingsMenu.RotateCogWheel();
    }

    public void OnQualityValueChange(int val)
    {
        QualitySettings.SetQualityLevel(val);

        PlayerPrefs.SetInt("Quality Setting", val);
        settingsMenu.RotateCogWheel();
    }

    public void OnFramesLimitValueChange(int val)
    {
        switch(val)
        {
            case 0:
                Application.targetFrameRate = -1;
                break;

            case 1:
                Application.targetFrameRate = 30;
                break;

            case 2:
                Application.targetFrameRate = 60;
                break;
            
            case 3:
                Application.targetFrameRate = 120;
                break;
        }

        PlayerPrefs.SetInt("Frame Rate Limit Setting", val);
        settingsMenu.RotateCogWheel();
    }

    public void OnWindowModeValueChange(int val)
    {
        switch(val)
        {
            case 0:
                _fullScreenModeChosen = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                _fullScreenModeChosen = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                _fullScreenModeChosen = FullScreenMode.MaximizedWindow;
                break;
            
            case 3:
                _fullScreenModeChosen = FullScreenMode.Windowed;
                break;
        }

        _fullScreenModeChosenInt = val;
        settingsMenu.RotateCogWheel();
    }

    public void OnResolutionValueChange(int val)
    {
        Resolution resolution = resolutions[val];
        _currResolutionIndex = val;

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution Setting", val);
        settingsMenu.RotateCogWheel();

        DisplayResolutionConfirm();
    }

    public void ResolutionValueChangeWithoutNotify(int index)
    {
        resolutionDropDown.SetValueWithoutNotify(index);
        resolutionDropDown.RefreshShownValue();

        Resolution resolution = resolutions[index];
        _currResolutionIndex = index;

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution Setting", index);
        settingsMenu.RotateCogWheel();
        PlayerPrefs.SetFloat("Resolution Setting", index);
    }

    private Sequence _currSequence;
    public void DisplayResolutionConfirm()
    {
        confirmResolutionMenu.SetActive(true);
        StartCountDown();
    }

    void StartCountDown()
    {
        int count = 10;

        _currSequence = DOTween.Sequence()
        .AppendCallback(() => {
            countDownTMP.text = $"Reverting in {count}";
            count--;
        })
        .AppendInterval(1f) // Wait for 1 second
        .SetLoops(count, LoopType.Restart) // Repeat this block count times
        .OnComplete(() => {
            confirmResolutionMenu.SetActive(false);
            ResolutionValueChangeWithoutNotify(prevResolutionIndex);
        }).SetUpdate(true);
    }
    
    public void ConfirmResolutionChangeBtn()
    {
        confirmResolutionMenu.SetActive(false);
        _currSequence?.Kill();
        prevResolutionIndex = _currResolutionIndex;
    }

    public void RevertResolutionChangeBtn()
    {
        confirmResolutionMenu.SetActive(false);
        _currSequence?.Kill();

        ResolutionValueChangeWithoutNotify(prevResolutionIndex);
    }
}