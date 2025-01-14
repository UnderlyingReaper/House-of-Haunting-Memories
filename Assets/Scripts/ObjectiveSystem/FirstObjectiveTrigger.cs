using UnityEngine;

public class FirstObjectiveTrigger : MonoBehaviour
{
    [SerializeField] Objective objectiveToStart;


    public void TriggerFirstObjective()
    {
        objectiveToStart?.StartObjective();
    }
}