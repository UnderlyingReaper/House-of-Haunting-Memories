using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenesEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] float delay = 0;
    [SerializeField] private string sceneName;
    [SerializeField] private string creditSceneName = "Credits";

    public event EventHandler OnExecutionEnd;

    public void EndExecute() => Invoke(nameof(SwitchScene), delay);

    void SwitchScene()
    {
        if(SceneUtility.GetBuildIndexByScenePath(sceneName) != -1) SceneManager.LoadScene(sceneName);
        else SceneManager.LoadScene(creditSceneName);

        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }
}