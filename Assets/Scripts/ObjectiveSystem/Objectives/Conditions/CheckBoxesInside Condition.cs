using System.Collections.Generic;
using UnityEngine;

public class CheckBoxesInsideCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private  List<PackedBox> totalBoxList;
    [SerializeField] private  float xVal;
    List<PackedBox> boxInsideList;



    void Awake()
    {
        boxInsideList = new();
    }

    public bool IsConditionMet()
    {
        foreach(PackedBox box in totalBoxList)
        {
            if(box.transform.position.x < xVal && !boxInsideList.Contains(box) && !box.isGrabbed)
            {
                boxInsideList.Add(box);
            }
            else if(box.transform.position.x > xVal && boxInsideList.Contains(box)) boxInsideList.Remove(box);
        }

        if(boxInsideList.Count == totalBoxList.Count) return true;
        else return false;
    }
}