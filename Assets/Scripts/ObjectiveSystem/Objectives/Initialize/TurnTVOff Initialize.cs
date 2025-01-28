using UnityEngine;

public class TurnTVOffInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private TV tv;

    public void Initialize()
    {
        tv.TurnOffTV();
    }
}