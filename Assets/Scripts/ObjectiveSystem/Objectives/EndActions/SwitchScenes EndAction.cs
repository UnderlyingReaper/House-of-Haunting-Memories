using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenesEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private string sceneName;

    public event EventHandler OnExecutionEnd;

    public void EndExecute()
    {
        SceneManager.LoadScene(sceneName);
        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}