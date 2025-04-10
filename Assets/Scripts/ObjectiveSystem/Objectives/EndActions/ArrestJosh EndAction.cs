using System;
using UnityEngine;

public class ArrestJoshEndAction : MonoBehaviour, IObjectiveEndAction
{
    private Animator _animator;

    public event EventHandler OnExecutionEnd;



    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void EndExecute()
    {
        _animator.SetTrigger("Arrest");
        GameplayInputManager.Instance.playerControls.Gameplay.Disable();

        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}
