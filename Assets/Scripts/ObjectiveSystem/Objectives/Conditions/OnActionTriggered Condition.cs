using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class OnActionTriggeredCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private MonoBehaviour actionSource;
    [SerializeField] private string actionName;



    private FieldInfo _fieldInfo;
    private Action _actionDelegate;
    private Objective _objective;
    private bool isConditionComplete;

    private void Awake()
    {
        _objective = GetComponent<Objective>();

        // Access the delegate field using reflection
        _fieldInfo = actionSource.GetType().GetField(actionName);

        #if UNITY_EDITOR
        Assert.IsNotNull(_fieldInfo);
        Assert.AreEqual(_fieldInfo.FieldType, typeof(Action));
        #endif

        // Create a new delegate of the existing action delegate
        _actionDelegate = _fieldInfo.GetValue(actionSource) as Action;

        // Subscribe to the delegate
        _actionDelegate += SetConditionTrue;

        // Reassign the modified delegate back to the original field
        _fieldInfo.SetValue(actionSource, _actionDelegate);
    }
    public bool IsConditionMet()
    {
        return isConditionComplete;
    }
    private void SetConditionTrue()
    {
        if(!_objective.start) return;

        isConditionComplete = true;
    }
}