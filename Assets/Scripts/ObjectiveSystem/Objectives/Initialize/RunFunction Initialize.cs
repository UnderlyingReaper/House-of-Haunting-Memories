using System.Reflection;
using UnityEngine;

public class RunFunctionInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private MonoBehaviour script;
    [SerializeField] private string funcName;

    public void Initialize()
    {
        // Get the type of the target script
        var scriptType = script.GetType();

        // Get the method info using the function name
        MethodInfo method = scriptType.GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (method != null) method.Invoke(script, null); // Pass arguments if required in the second parameter
        else Debug.LogError($"Method {funcName} not found on {script.GetType().Name}");
    }
}