using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractible
{
    [Header("IInteractible Settings")]
    [SerializeField] private bool isUsing;
    [SerializeField] private int priority;
    [SerializeField] private string text = "Use";

    [Space(20)]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer chair;
    [SerializeField] private BunnyOS operatingSystem;

    [Header("Camera Settings")]
    [SerializeField] private float maxOffset;
    [SerializeField] private CinemachineCamera virtualCam;


    private Transform _player;
    private PlayerMovement _playerMovement;
    
    private CinemachinePositionComposer _virtualCamPosComposer;

    private PlayerControls.MouseActions mouse {
        get { return GameplayInputManager.Instance.playerControls.Mouse; }
    }

    private Vector3 _boxPos3D, _boxSize3D;


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _virtualCamPosComposer = virtualCam.GetComponent<CinemachinePositionComposer>();

        _boxSize3D = new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.x);
        _boxPos3D = new Vector3(transform.position.x + boxCollider.offset.x,
                                transform.position.y + boxCollider.offset.y,
                                transform.position.z);
    }
    void Update()
    {
        if(!isUsing || !enabled) return;

        Vector2 normalMousePos = new Vector2(mouse.MousePosition.ReadValue<Vector2>().x / Screen.width, mouse.MousePosition.ReadValue<Vector2>().y / Screen.height);
        Vector2 mousePosCenter = (normalMousePos - new Vector2(0.5f, 0.5f)) * 2;
        
        _virtualCamPosComposer.TargetOffset = mousePosCenter * maxOffset;
    }

    void CheckPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_boxPos3D, boxCollider.size, 0);

        bool playerFound = false;
        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                playerFound = true;
                break;
            }
        }
        
        if(!playerFound) return;

        if(transform.position.x < _player.position.x)
        {
            if(_player.localScale.x != 1)
            {
                _player.localScale = new Vector3(1, 1, 1);
                _playerMovement.isFacingRight = true;
            }

            _player.DOMoveX(transform.position.x + 2, 0.7f).SetEase(Ease.Linear).OnComplete(() => {
                _player.localScale = new Vector3(-1, 1, 1);
                _playerMovement.isFacingRight = false;
            });
        }
        else if(transform.position.x >= _player.position.x)
        {
            if(_player.localScale.x != -1)
            {
                _player.localScale = new Vector3(-1, 1, 1);
                _playerMovement.isFacingRight = false;
            }

            _player.DOMoveX(transform.position.x - 2, 0.7f).SetEase(Ease.Linear).OnComplete(() => {
                _player.localScale = new Vector3(1, 1, 1);
                _playerMovement.isFacingRight = true;
            });
        }
    }

    public void InteractStart(Transform interactorTransform) {}

    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(!isUsing)
        {
            CheckPlayer();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isUsing = true;
            virtualCam.gameObject.SetActive(true);
            chair.DOFade(0, 2);
            GameplayInputManager.Instance.enabled = false;

            operatingSystem.StopAllCoroutines();
            StartCoroutine(operatingSystem.StartOS());
        }
        else if(isUsing) Shutdown();
    }

    public void Shutdown()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isUsing = false;
        virtualCam.gameObject.SetActive(false);
        chair.DOFade(1, 2);
        GameplayInputManager.Instance.enabled = true;

        operatingSystem.StopAllCoroutines();
        StartCoroutine(operatingSystem.ShutdownOS());
    }

    public void InteractCancel(Transform interactorTransform) {}

    public Transform GetTransform()
    {
        return transform;
    }

    public int GetPriority()
    {
        return priority;
    }

    public string GetText()
    {
        if(!enabled || isUsing) return null;
        return text;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxPos3D, _boxSize3D);
    }
}
