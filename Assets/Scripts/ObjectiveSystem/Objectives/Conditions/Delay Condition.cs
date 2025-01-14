using UnityEngine;

public class DelayCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private float delayTime;

    private float _timePassed;

    public bool IsConditionMet()
    {
        _timePassed += Time.deltaTime;

        if(_timePassed >= delayTime) return true;
        else return false;
    }
}