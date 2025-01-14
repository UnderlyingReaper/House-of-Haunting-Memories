using System;
using UnityEngine;

public class LaunchApp : MonoBehaviour, IIconBehaviour
{
    [SerializeField] private GameObject windowPrefab;
    public event Action<GameObject> OnWindowLaunch;


    public void Run()
    {
        BunnyOSTaskManager.Instance.LaunchApp(windowPrefab);

        foreach(App app in BunnyOSTaskManager.Instance.activeApps)
        {
            if(app.window.name == windowPrefab.name)
            {
                OnWindowLaunch?.Invoke(app.window);
                break;
            }
        }
        
    }
}