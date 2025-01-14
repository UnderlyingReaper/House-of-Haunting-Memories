using System.Collections.Generic;
using UnityEngine;

public class CloseDoorsInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private List<OpenCloseBehaviour> doorsList;

    public void Initialize()
    {
        foreach(OpenCloseBehaviour door in doorsList)
        {
            door.CloseDoorWithoutSound(door.GetComponent<Door>());
        }
    }
}