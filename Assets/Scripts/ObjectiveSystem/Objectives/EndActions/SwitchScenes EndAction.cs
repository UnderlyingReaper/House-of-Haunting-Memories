using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenesEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private string sceneName;
    [SerializeField] private string creditSceneName = "Credits";

    public event EventHandler OnExecutionEnd;

    public void EndExecute()
    {
        if(SceneUtility.GetBuildIndexByScenePath(sceneName) != -1) SceneManager.LoadScene(sceneName);
        else SceneManager.LoadScene(creditSceneName);

        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}