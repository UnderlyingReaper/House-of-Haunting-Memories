using TMPro;
using UnityEngine;

public class ObjectiveUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveTxt;
    [SerializeField] string noObjectiveTxt;



    void Start()
    {
        ObjectiveHandler.Instance.OnNewObjectiveStart += DisplayNewObjective;
        ObjectiveHandler.Instance.OnObjectiveFinish += DisplayNoObjective;
    }

    void DisplayNewObjective(string text)
    {
        objectiveTxt.text = text;
    }

    void DisplayNoObjective()
    {
        objectiveTxt.text = noObjectiveTxt;
    }
}