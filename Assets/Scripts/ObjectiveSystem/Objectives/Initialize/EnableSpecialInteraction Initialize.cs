using System.Collections.Generic;
using UnityEngine;

public class EnableSpecialInteractionInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private List<MonoBehaviour> monoBehaviourList;


    private List<ISpecialInteraction> specialInteractionList;
    

    private void Awake()
    {
        specialInteractionList = new();
        foreach(MonoBehaviour script in monoBehaviourList)
        {
            specialInteractionList.Add(script as ISpecialInteraction);
        }
    }
    public void Initialize()
    {
        foreach(ISpecialInteraction script in specialInteractionList)
        {
            script.AllowSpecialInteraction();
        }
    }
}