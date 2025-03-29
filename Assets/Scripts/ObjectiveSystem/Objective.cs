using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public string objectiveTask;
    public bool start = false;

    [Space(20)]
    [Header("Trigger Settings")]
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private MonoBehaviour triggerScript;
    [SerializeField] private string actionName;

    [Space(20)]
    [Header("Main Components")]
    [SerializeField] private List<MonoBehaviour> InitializeMonoList;

    [Tooltip("REQUIRED")]
    [SerializeField] private MonoBehaviour conditionMono;
    [SerializeField] private MonoBehaviour progressMono;
    [SerializeField] private MonoBehaviour endMono;

    [Space(5)]
    [Header("Additional Components")]
    [SerializeField] private List<MonoBehaviour> additionalEndMono;

    [Space(20)]
    [Header("Objective Additional Information")]
    [SerializeField] private Objective nextObjective;
    [SerializeField] private MonoBehaviour makePlayerSpeakMono;
    [SerializeField] private float speechDelay = 0;
    [SerializeField] private float timeToFinish;


    private float _timePassed;

    public event Action OnObjectiveEnd;

    private List<IObjectiveInitialize> _initialize;
    private IObjectiveCondition _condition;
    private IObjectiveTrackProgress _progress;
    private IObjectiveEndAction _endAction;
    private List<IObjectiveAdditionalEnd> _additionalEnd;
    private IPlayerSpeak _makePlayerSpeak;



    void Awake()
    {
        if(isTriggered) SetTrigger();

        _initialize = new();
        foreach(MonoBehaviour mono in InitializeMonoList)
        {
            _initialize.Add(mono as IObjectiveInitialize);
        }
        
        _condition = conditionMono as IObjectiveCondition;
        _progress = progressMono as IObjectiveTrackProgress;
        _endAction = endMono as IObjectiveEndAction;

        _additionalEnd = new();
        foreach(MonoBehaviour mono in additionalEndMono)
        {
            _additionalEnd.Add(mono as IObjectiveAdditionalEnd);
        }

        _makePlayerSpeak = makePlayerSpeakMono as IPlayerSpeak;


        if(_endAction != null) _endAction.OnExecutionEnd += (object sender, EventArgs e) => { FinishObjective(); };
    }

    void Update()
    {
        if(!start) return;

        _progress?.UpdateProgress();

        if(_condition.IsConditionMet())
        {
            start = false;
            
            if(_endAction == null) FinishObjective();
            else _endAction.EndExecute();

            if(_additionalEnd == null) return;
            foreach(IObjectiveAdditionalEnd additionalEnd in additionalEndMono)
            {
                additionalEnd.AdditionalCode();
            }
        }


        _timePassed += Time.deltaTime;
        if(_timePassed >= timeToFinish)
        {
            _timePassed = 0;
            _makePlayerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Hint);
        }
    }

    private void SetTrigger()
    {
        // Access the delegate field using reflection
        var _fieldInfo = triggerScript.GetType().GetField(actionName);

        #if UNITY_EDITOR
        Assert.IsNotNull(_fieldInfo);
        Assert.AreEqual(_fieldInfo.FieldType, typeof(Action));
        #endif

        // Create a new delegate of the existing action delegate
        var _actionDelegate = _fieldInfo.GetValue(triggerScript) as Action;

        // Subscribe to the delegate
        _actionDelegate += StartObjective;

        // Reassign the modified delegate back to the original field
        _fieldInfo.SetValue(triggerScript, _actionDelegate);
    }

    public void StartObjective()
    {
        if(!ObjectiveHandler.Instance.CanRunObjective(this)) return;

        ObjectiveHandler.Instance.StartObjective(this);

        start = true;
        
        DOTween.Sequence()
        .AppendInterval(speechDelay)
        .AppendCallback(() => _makePlayerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main));

        #if UNITY_EDITOR
        Assert.IsNotNull(_initialize);
        #endif

        foreach(IObjectiveInitialize initializeScript in _initialize)
        {
            #if UNITY_EDITOR
            Assert.IsNotNull(initializeScript);
            #endif

            initializeScript.Initialize();
        }
    }

    public void FinishObjective()
    {
        OnObjectiveEnd?.Invoke();
        start = false;

        if(nextObjective != null) nextObjective.StartObjective();
    }
}