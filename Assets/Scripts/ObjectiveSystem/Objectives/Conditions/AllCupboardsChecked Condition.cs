using System;
using System.Collections.Generic;
using UnityEngine;

public class AllCupboardsCheckedCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private List<CupboardStateData> cupboardStateDataList;

    [Serializable]
    private struct CupboardStateData {
        public Cupboard cupboard;
        public bool isChecked;
    }

    private void Awake()
    {
        foreach(CupboardStateData cupboardStateData in cupboardStateDataList)
        {
            cupboardStateData.cupboard.OnCupboardCheck += OnCupboardChecked;
        }
    }

    public bool IsConditionMet()
    {
        foreach(CupboardStateData cupboardStateData in cupboardStateDataList)
        {
            if(!cupboardStateData.isChecked) return false;
        }

        return true;
    }

    private void OnCupboardChecked(Cupboard cupboard)
    {
        for(int i = 0; i <= cupboardStateDataList.Count-1; i++)
        {
            if(cupboardStateDataList[i].cupboard == cupboard)
            {
                CupboardStateData data = cupboardStateDataList[i]; // Retrieve a copy of the struct

                // Modify the copy
                data.isChecked = true; 
                data.cupboard.OnCupboardCheck -= OnCupboardChecked;

                cupboardStateDataList[i] = data; // Save the modified copy back to the list
                break;
            }
        }
    }
}