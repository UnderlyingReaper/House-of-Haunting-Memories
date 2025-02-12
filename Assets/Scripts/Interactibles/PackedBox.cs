using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PackedBox : MonoBehaviour, IInteractible
{
    [Header("IInteractible Settings")]
    [SerializeField] private int priority;
    [SerializeField] private string grabText = "Grab";
    [SerializeField] private string dropText = "Drop";

    [Header("Packed Box Settings")]
    public bool isGrabbed;
    [SerializeField] private List<GameObject> objectList;

    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer[] _sprites;
    private Collider2D _playerCollider;
    private Collider2D _handCollider;

    private void Awake()
    {
        foreach(GameObject gameObject in objectList)
        {
            gameObject.SetActive(false);
        }

        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _sprites = transform.GetComponentsInChildren<SpriteRenderer>();
    }

    public void InteractStart(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        Interact interactor = interactorTransform.GetComponent<Interact>();

        if(!enabled) return;
        if(AnimationController.Instance.isGrabbingBox && transform.parent != interactor.handHolder) return;

        if(!isGrabbed) StartCoroutine(GrabBox(interactor.handHolder, interactorTransform));
        else StartCoroutine(DropBox());
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

    private IEnumerator GrabBox(Transform handPoint, Transform interactor)
    {
        GameplayInputManager.Instance.enabled = false;
        AnimationController.Instance.isGrabbingBox = true;
        isGrabbed = true;

        yield return new WaitForSeconds(.5f);
        transform.SetParent(handPoint);
        transform.localPosition = Vector3.zero;
        transform.DOLocalRotate(Vector3.zero, 0.5f);

        if(_playerCollider == null) _playerCollider = interactor.GetComponent<Collider2D>();
        if(_handCollider == null) _handCollider = interactor.GetComponent<Interact>().handCollider;

        _rb.bodyType = RigidbodyType2D.Kinematic;
        _boxCollider.isTrigger = true;

        for(int i = 0; i < _sprites.Length; i++)
        {
            _sprites[i].sortingLayerName = "Player";
            _sprites[i].sortingOrder = 7;
        }
        

        yield return new WaitForSeconds(AnimationController.Instance.animator.GetCurrentAnimatorClipInfo(0).Length);

        GameplayInputManager.Instance.enabled = true;
        _handCollider.enabled = true;
        priority += 100;
    }

    private IEnumerator DropBox()
    {
        GameplayInputManager.Instance.enabled = false;
        AnimationController.Instance.isGrabbingBox = false;
        isGrabbed = false;

        yield return new WaitForSeconds(.5f);
        transform.SetParent(null);
        transform.DOLocalRotate(Vector3.zero, 0.5f);

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _boxCollider.isTrigger = false;

        for(int i = 0; i < _sprites.Length; i++)
        {
            _sprites[i].sortingLayerName = "Default";
            _sprites[i].sortingOrder = 1;
        }


        yield return new WaitForSeconds(AnimationController.Instance.animator.GetCurrentAnimatorClipInfo(0).Length);

        GameplayInputManager.Instance.enabled = true;
        _handCollider.enabled = false;
        priority -= 100;
    }

    public void Unbox()
    {
        foreach(GameObject gameObject in objectList)
        {
            gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }

    public string GetText()
    {
        if(!enabled) return null;
        else if(isGrabbed) return dropText;
        else return grabText;
    }
}