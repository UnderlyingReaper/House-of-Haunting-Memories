using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public static Interact Instance { get; private set; }



    [SerializeField] private Transform interactOffsetPoint;
    [SerializeField] private Vector2 boxRange;
    public Transform handHolder;
    public Collider2D handCollider;
    


    IInteractible _interactible, _previousInteractible;


    PlayerControls.GameplayActions controls {
        get { return GameplayInputManager.Instance.playerControls.Gameplay; }
    }

    public event Action<string> OnInteractEnter;
    public event Action OnInteractExit;



    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        controls.Interact.started += InteractStart;
        controls.Interact.performed += InteractPerform;
        controls.Interact.canceled += InteractCancel;
    }
    void Update()
    {
        CheckForInteractible();
        CheckInteractibleChange();
    }

    void InteractStart(InputAction.CallbackContext context)
    {
        _interactible?.InteractStart(transform);
    }
    void InteractPerform(InputAction.CallbackContext context)
    {
        _interactible?.InteractPerform(transform);
    }
    void InteractCancel(InputAction.CallbackContext context)
    {
        _interactible?.InteractCancel(transform);
    }

    void CheckForInteractible()
    {
        // Get all colliders within the box region
        Collider2D[] colliders = Physics2D.OverlapBoxAll(interactOffsetPoint.position, boxRange, 0);
        // Check if any colliders are detected and return if nothing is detected
        if(colliders.Length == 0) 
        {
            if (_interactible != null)
            {
                OnInteractExit?.Invoke();
                _interactible = null;
            }

            _previousInteractible = null;
            return;
        }

        // Now get the interactibles from the colliders detcted if any
        List<IInteractible> interactiblesList = new();
        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent<IInteractible>(out IInteractible interactible))
            {
                // Check if the interactible can even be interacted with or not
                MonoBehaviour interfaceMono = interactible as MonoBehaviour;
                if(!interfaceMono.enabled) continue;

                interactiblesList.Add(interactible);
            }
        }
        // Check if any interactibles were found and return if nothing is detected
        if(interactiblesList.Count == 0)
        {
            if (_interactible != null)
            {
                OnInteractExit?.Invoke();
                _interactible = null;
            }
            
            _previousInteractible = null;
            return;
        }

        // Now get the closest and highest priority interactible from the list
        IInteractible closestInteractible = null;
        foreach(IInteractible interactible in interactiblesList)
        {
            if(closestInteractible == null) closestInteractible = interactible;
            // Get the object with the highest priority
            else if(interactible.GetPriority() > closestInteractible.GetPriority())
            {
                closestInteractible = interactible;
            }
            // Get the object with the highest priority and the smallest distance
            else if(interactible.GetPriority() == closestInteractible.GetPriority() && (Vector3.Distance(transform.position, interactible.GetTransform().position) < Vector3.Distance(transform.position, closestInteractible.GetTransform().position)))
            {
                closestInteractible = interactible;
            }
        }
        
        _interactible = closestInteractible;
    }

    void CheckInteractibleChange()
    {
        // Check if current interactible has changed
        if (_interactible == _previousInteractible) return;
        
        if (_interactible != null && _previousInteractible == null) // Invoke the action if the value of the interactible changes AND is not null
        {
            string text = _interactible.GetText();
            
            if(text != null)
                OnInteractEnter?.Invoke(text);
            else OnInteractExit?.Invoke();
        }
        else if (_interactible == null && _previousInteractible != null) // Invoke the action if the value of the interactible changes AND is now null
            OnInteractExit?.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(interactOffsetPoint.position, boxRange);
    }
}