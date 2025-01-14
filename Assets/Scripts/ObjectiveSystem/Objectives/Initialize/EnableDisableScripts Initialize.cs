using System.Collections.Generic;
using UnityEngine;

public class EnableDisableScriptsInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private List<MonoBehaviour> scriptsToEnable;
    [SerializeField] private List<MonoBehaviour> scriptsToDisable;
    
    public void Initialize()
    {
        foreach(MonoBehaviour script in scriptsToEnable)
        {
            script.enabled = true;
            script.gameObject.SetActive(true);
        }

        foreach(MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = false;
        }
    }
}
