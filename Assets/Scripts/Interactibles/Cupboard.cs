using System;
using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Cupboard : MonoBehaviour, IInteractible, ISpecialInteraction
{
    [Header("IInteractible Settings")]
    [SerializeField] private bool allowSpecialInteraction;
    [SerializeField] private int priority;
    [SerializeField] private string textOpen = "Open", textClose = "Close";

    [Space(20)]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float finalLens;
    [SerializeField] private float duration;
    [SerializeField] private float stareDuraton;

    [Header("Doors")]
    [SerializeField] private float openTime;
    [SerializeField] private Transform doorR;
    [SerializeField] private Transform doorL;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tensionBuildUpClip;
    [SerializeField] private AudioClip cupboardDoorCloseClip;


    public Action<Cupboard> OnCupboardCheck;


    private BoxCollider2D _boxCollider;
    private float _virtualCamLensOrgSize;
    private float _virtualCamLensAvgSize;
    private bool _isOpen;
    private Transform _player;
    private PlayerMovement _playerMovement;
    private Sequence _currSequence;


    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerMovement = _player.GetComponent<PlayerMovement>();

        _virtualCamLensOrgSize = virtualCamera.Lens.OrthographicSize;
        _virtualCamLensAvgSize = (_virtualCamLensOrgSize + finalLens) / 2;
    }

    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(allowSpecialInteraction)
        {
            CheckPlayer();
            StartCoroutine(Interaction());
        }
        else if(_isOpen) Close();
        else
        {
            DOTween.Kill(_currSequence);
            _currSequence = DOTween.Sequence()
            .Append(doorR.DOScaleX(0, 1))
            .Join(doorL.DOScaleX(0, 1));

            _isOpen = true;
        }
    }
    public void InteractCancel(Transform interactorTransform) {}
    public void InteractStart(Transform interactorTransform) {}
    public Transform GetTransform()
    {
        return transform;
    }
    public int GetPriority()
    {
        return priority;
    }

    public void Close()
    {
        DOTween.Kill(_currSequence);
        _currSequence = DOTween.Sequence()
        .Append(doorR.DOScaleX(1, 1))
        .Join(doorL.DOScaleX(1, 1))
        .InsertCallback(0.9f, () => audioSource.PlayOneShot(cupboardDoorCloseClip));
            
        _isOpen = false;
    }

    void CheckPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0);

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
        else if(transform.position.x >= _player.position.x || transform.position.x < _player.position.x)
        {
            if(_player.localScale.x != -1)
            {
                _player.localScale = new Vector3(-1, 1, 1);
                _playerMovement.isFacingRight = false;
            }

            _player.DOMoveX(transform.position.x - 2, 1.2f).SetEase(Ease.Linear).OnComplete(() => {
                _player.localScale = new Vector3(1, 1, 1);
                _playerMovement.isFacingRight = true;
            });
        }
    }

    IEnumerator Interaction()
    {
        allowSpecialInteraction = false;
        
        virtualCamera.Lens.OrthographicSize = _virtualCamLensOrgSize;
        virtualCamera.gameObject.SetActive(true);

        GameplayInputManager.Instance.enabled = false;
        bool wasEquippingGun = AnimationController.Instance.equipGun;
        AnimationController.Instance.equipGun = false;


        audioSource.PlayOneShot(tensionBuildUpClip);

        DOVirtual.Float(virtualCamera.Lens.OrthographicSize, finalLens, duration, value => { virtualCamera.Lens.OrthographicSize = value; }).SetEase(Ease.Linear);
        doorR.DOScaleX(0.9f, duration);
        doorL.DOScaleX(0.9f, duration);

        yield return new WaitForSeconds(duration);

        doorR.DOScaleX(0, openTime);
        doorL.DOScaleX(0, openTime);

        DOVirtual.Float(virtualCamera.Lens.OrthographicSize, _virtualCamLensAvgSize, openTime, value => { virtualCamera.Lens.OrthographicSize = value; });

        yield return new WaitForSeconds(stareDuraton);

        GameplayInputManager.Instance.enabled = true;
        AnimationController.Instance.equipGun = wasEquippingGun;

        virtualCamera.gameObject.SetActive(false);

        OnCupboardCheck?.Invoke(this);

        doorR.DOScaleX(1, 1);
        doorL.DOScaleX(1, 1);

        yield return new WaitForSeconds(0.9f);
        audioSource.PlayOneShot(cupboardDoorCloseClip);
    }

    public string GetText()
    {
        if(!enabled) return null;
        else if(!_isOpen || allowSpecialInteraction) return textOpen;
        else return textClose;
    }

    public void AllowSpecialInteraction()
    {
        allowSpecialInteraction = true;
    }
}
