using System.Collections.Generic;
using UnityEngine;

public class CloseCupboardsInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] List<Cupboard> cupboardList;
    public void Initialize()
    {
        foreach(Cupboard cupboard in cupboardList)
        {
            cupboard.Close();
        }
    }
}