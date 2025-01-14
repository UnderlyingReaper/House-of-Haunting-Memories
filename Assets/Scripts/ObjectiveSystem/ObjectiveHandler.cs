using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public static ObjectiveHandler Instance { get; private set; }

    [SerializeField] List<Objective> objectivesCompleted;
    [SerializeField] Objective currObjective;


    public event Action<string> OnNewObjectiveStart;
    public event Action OnObjectiveFinish;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanRunObjective(Objective objective)
    {
        if(currObjective != null) return false;
        else return true;
    }

    public void StartObjective(Objective objective)
    {
        currObjective = objective;
        OnNewObjectiveStart?.Invoke(objective.objectiveTask);
        currObjective.OnObjectiveEnd += FinishObjective;
    }

    void FinishObjective()
    {
        objectivesCompleted.Add(currObjective);
        currObjective.OnObjectiveEnd -= FinishObjective;

        OnObjectiveFinish?.Invoke();
        
        currObjective.enabled = false;
        currObjective = null;
    }
}
