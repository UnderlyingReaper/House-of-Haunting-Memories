using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup elementsCanvas;
    [SerializeField] private Image fade;
    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private List<AudioSource> audioSourceList;
    [SerializeField] private MenuMusicHandeler menuMusicHandeler;

    [Header("Door")]
    [SerializeField] private Door door;

    [Header("Camera Settings")]
    [SerializeField] private float maxOffset;
    [SerializeField] private CinemachineCamera virtualCam;


    private CinemachineCameraOffset _virtualCamPosComposer;
    private OpenCloseBehaviour _doorBehaviour;


    public event Action OnSceneLoaded;
    public event Action OnUpdateInfoBtnClicked;

    private PlayerControls.MouseActions mouse {
        get { return GameplayInputManager.Instance.playerControls.Mouse; }
    }



    private void Awake()
    {
        _virtualCamPosComposer = virtualCam.GetComponent<CinemachineCameraOffset>();
        canvasGroup = GetComponent<CanvasGroup>();
        _doorBehaviour = door.GetComponent<OpenCloseBehaviour>();


        fade.color = new Color(fade.color.r,
                               fade.color.g,
                               fade.color.b,
                               1);

        foreach(AudioSource source in audioSourceList) source.Play();
        SetVolume(0.12f, 4);
    }
    private void Start()
    {   
        GameplayInputManager.Instance.playerControls.UI.Enable();
        GameplayInputManager.Instance.enabled = false;

        Time.timeScale = 1;

        GameplayInputManager.Instance.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void OnEnable() => fade.DOFade(0, 3).OnComplete(() => OnSceneLoaded?.Invoke());

    private void Update()
    {
        Vector2 normalMousePos = new Vector2(mouse.MousePosition.ReadValue<Vector2>().x / Screen.width, mouse.MousePosition.ReadValue<Vector2>().y / Screen.height);
        Vector2 mousePosCenter = (normalMousePos - new Vector2(0.5f, 0.5f)) * 2;
        
        _virtualCamPosComposer.Offset = mousePosCenter * maxOffset;
    }

    public void Startbtn()
    {
        menuMusicHandeler = FindFirstObjectByType<MenuMusicHandeler>();
        menuMusicHandeler?.GameStart(3);

        canvasGroup.interactable = false;
        
        DOTween.Sequence()
        .Append(elementsCanvas.DOFade(0, 2))
        .AppendInterval(2)
        .AppendCallback(() => _doorBehaviour.OpenDoor(door))
        .AppendInterval(1)
        .Append(fade.DOFade(1, 2))
        .JoinCallback(() => SetVolume(0, 2))
        .OnComplete(() => SceneManager.LoadScene(2));
    }

    public void SettingsBtn()
    {
        canvasGroup.DOFade(0, 0.5f);
        settingsMenu.canvasGroup.DOFade(1, 0.5f);
        settingsMenu.previousCanvasGroup = canvasGroup;

        settingsMenu.canvasGroup.interactable = true;
        settingsMenu.canvasGroup.blocksRaycasts = true;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateBtn()
    {
        OnUpdateInfoBtnClicked?.Invoke();
    }

    public void QuitBtn()
    {
        SetVolume(0, 2);
        fade.DOFade(1, 2).OnComplete(() => {
            Application.Quit();
        });
    }

    private void SetVolume(float desiredVol, float time)
    {
        foreach(AudioSource source in audioSourceList)
        {
            source.DOFade(desiredVol, time);
        }
    }
}
