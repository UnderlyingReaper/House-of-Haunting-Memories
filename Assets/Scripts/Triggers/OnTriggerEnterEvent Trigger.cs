using System;
using UnityEngine;

public class OnTriggerEnterEventTrigger : MonoBehaviour
{
    public event Action OnTriggerEnter;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerEnter?.Invoke();
    }
}