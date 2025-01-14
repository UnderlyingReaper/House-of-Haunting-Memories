using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private float appearDuration;
    [SerializeField] private float appearDelay;

    [SerializeField] private CanvasGroup fadeImg;
    [SerializeField] private SettingsMenu settingsMenu;

    [Header("Other")]
    [SerializeField] private float focusDistance;
    [SerializeField] private float focusDuration = 0.3f;

    [SerializeField] private RectTransform decorLine;
    [SerializeField] private float scaleDuration;


    private DepthOfField _depthOfField;
    [HideInInspector] public CanvasGroup canvasGroup;



    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Volume vol = GameObject.FindGameObjectWithTag("Global Volume").GetComponent<Volume>();
        vol.profile.TryGet(out _depthOfField);

        GameplayInputManager.Instance.playerControls.Gameplay.Pause.performed += (InputAction.CallbackContext context) => {
            StartCoroutine(Enable());
        };

        GameplayInputManager.Instance.playerControls.UI.Back.performed += (InputAction.CallbackContext context) => {
            Disable();
        };

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator Enable()
    {
        GameplayInputManager.Instance.enabled = false;
        GameplayInputManager.Instance.playerControls.UI.Enable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DOVirtual.Float(_depthOfField.focusDistance.value, focusDistance, focusDuration, value => { _depthOfField.focusDistance.value = value; }).SetUpdate(true);

        yield return new WaitForSeconds(appearDelay);

        decorLine.DOScaleX(1, scaleDuration).SetUpdate(true);

        canvasGroup.DOFade(1, appearDuration).SetUpdate(true);
        Time.timeScale = 0;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void Disable()
    {
        if(!gameObject.activeSelf || canvasGroup.alpha < 0.2f) return;

        DOVirtual.Float(_depthOfField.focusDistance.value, 3, focusDuration, value => { _depthOfField.focusDistance.value = value; }).SetUpdate(true);
        decorLine.DOScaleX(0, scaleDuration).SetUpdate(true);

        GameplayInputManager.Instance.enabled = true;
        GameplayInputManager.Instance.playerControls.UI.Disable();
        canvasGroup.DOFade(0, appearDuration);

        Time.timeScale = 1;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ResumeBtn() => Disable();
    public void RestartBtn()
    {
        fadeImg.DOFade(1, 1).SetUpdate(true).OnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    public void SettingsBtn()
    {
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true);
        settingsMenu.canvasGroup.DOFade(1, 0.5f).SetUpdate(true);
        
        settingsMenu.canvasGroup.interactable = true;
        settingsMenu.canvasGroup.blocksRaycasts = true;
        settingsMenu.previousCanvasGroup = canvasGroup;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void MainMenuBtn()
    {
        fadeImg.DOFade(1, 1).SetUpdate(true).OnComplete(() => {
            SceneManager.LoadScene(1);
        });
    }

    void OnDestroy()
    {
        _depthOfField.focusDistance.value = 3;    
    }
}
