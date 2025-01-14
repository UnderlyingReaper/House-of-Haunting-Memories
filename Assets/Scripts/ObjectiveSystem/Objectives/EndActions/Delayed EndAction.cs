using System;
using System.Collections;
using UnityEngine;

public class DelayedEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private float delayTime;
    public event EventHandler OnExecutionEnd;

    public void EndExecute()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}