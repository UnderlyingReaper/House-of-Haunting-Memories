using System;
using System.Collections;
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
    public Slider resolutionScaleSlider;
    public TextMeshProUGUI resolutionScaleDisplay;
    public TMP_Dropdown resolutionDropDown;
    public CanvasGroup resolutionCanvasGroup;
    public GameObject confirmResolutionMenu;
    public TextMeshProUGUI countDownTMP;


    [HideInInspector] public int orgResolutionIndex;
    [HideInInspector] public int prevResolutionIndex;

    private int _currResolutionIndex;

    [HideInInspector] public Bloom bloom;
    [HideInInspector] public Resolution[] resolutions;



    public void DefaultGraphicsSettingsBtn()
    {
        bloomToggle.isOn = true;
        qualityDropDown.value = 2;

        frameLimitDropDown.value = 0;
        frameLimitDropDown.RefreshShownValue();

        windowModeDropDown.value = 0;
        windowModeDropDown.RefreshShownValue();

        resolutionScaleSlider.value = 1;

        resolutionDropDown.value = orgResolutionIndex;
        resolutionDropDown.RefreshShownValue();
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
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }

        if(Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            resolutionCanvasGroup.DOFade(0.2f, 0.5f).SetUpdate(true);
            resolutionCanvasGroup.interactable = false;
            resolutionCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            resolutionCanvasGroup.DOFade(1, 0.5f).SetUpdate(true);
            resolutionCanvasGroup.interactable = true;
            resolutionCanvasGroup.blocksRaycasts = true;
        }

        PlayerPrefs.SetInt("Window Mode Setting", val);
        settingsMenu.RotateCogWheel();
    }

    public void OnResolutionScaleValueChange(float val)
    {
        int newWidth = (int)(Screen.currentResolution.width * val);
        int newHeight = (int)(Screen.currentResolution.height * val);

        resolutionScaleDisplay.text = val.ToString("F1");

        Screen.SetResolution(newWidth, newHeight, Screen.fullScreen);

        PlayerPrefs.SetFloat("Resolution Scale Setting", val);
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