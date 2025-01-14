using System;
using System.Collections.Generic;
using UnityEngine;

public class DisableScriptsEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private List<MonoBehaviour> scriptsToDisable;

    public event EventHandler OnExecutionEnd;

    public void EndExecute()
    {
        foreach(MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = false;
        }

        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}