using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    [Header("IInteractible Settings")]
    [SerializeField] private int priority;
    [SerializeField] private string text = "Interact";

    [Space(20)]
    public Transform doorHandle;
    public Transform mainDoor;

    [SerializeField] private List<MonoBehaviour> additionalBehaviours;
    [SerializeField] private MonoBehaviour lockModeMono;
    [SerializeField] private MonoBehaviour doorBehaviour;


    private IDoorBehaviour _doorBehaviourInterface;
    private ILockable _lock;
    private List<IDoorBehaviour> _additonalBehavioursInterface;
    [HideInInspector] public AudioSource audioSource;
    public Action OnDoorInteract;

    private void Awake()
    {
        // Attempt to get the behavior interface from the assigned component
        _doorBehaviourInterface = doorBehaviour as IDoorBehaviour;
        _lock = lockModeMono as ILockable;

        audioSource = GetComponent<AudioSource>();
        
        _additonalBehavioursInterface = new();
        foreach(MonoBehaviour monoBehaviour in additionalBehaviours)
        {
            _additonalBehavioursInterface.Add(monoBehaviour as IDoorBehaviour);
        }
    }

    public int GetPriority()
    {
        return priority;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractStart(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;
        
        if(_lock?.CheckIfLocked() == true)
        {
            _lock.TryUnlock();
            return;
        }

        _doorBehaviourInterface?.InteractPerform(this);
        
        foreach(IDoorBehaviour behaviour in _additonalBehavioursInterface)
        {
            behaviour?.InteractPerform(this);
        }
        OnDoorInteract?.Invoke();
    }
    public void InteractCancel(Transform interactorTransform) {}

    public string GetText()
    {
        if(!enabled) return null;
        return text;
    }
}