using System;
using System.Collections.Generic;
using UnityEngine;

public class BunnyOSTaskManager : MonoBehaviour
{
    public static BunnyOSTaskManager Instance { get; private set; }
    [SerializeField] private RectTransform windowsHolder;
    [SerializeField] private RectTransform tasksHolder;

    [SerializeField] public List<App> activeApps;

    [SerializeField] private AppWindow currActiveWindow;

    public event Action OnAllAppsClose; 


    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        activeApps = new();
    }


    public void LaunchApp(GameObject windowPrefab)
    {
        // Check if the App is already launched and set it as the active app instead
        foreach(App app in activeApps)
        {
            if(app.window.name != windowPrefab.name) continue;

            AppWindow appWindow = app.window.GetComponent<AppWindow>();

            // If app is Minimized then open it and set it as the current active App
            if(!appWindow.isFocused)
            {
                app.window.transform.SetAsLastSibling();

                // unfocus the previous window
                if(currActiveWindow != null && currActiveWindow != appWindow)
                {
                    currActiveWindow.isFocused = false;
                    currActiveWindow.appTask.GetComponent<AppTask>().isFocused = false;
                    currActiveWindow = null;
                }
                
                // Focus the new active App
                appWindow.isFocused = true;
                appWindow.OpenWindow();

                AppTask appTask = appWindow.appTask.GetComponent<AppTask>();
                appTask.isFocused = true;
                appTask.PlayFocusAnimation();
                currActiveWindow = appWindow;
            }

            return;
        }

        // Launch & Add Window
        GameObject newAppInst = Instantiate(windowPrefab, windowsHolder);
        newAppInst.name = windowPrefab.name;
        

        AppWindow newAppWindow = newAppInst.GetComponent<AppWindow>();

        // Launch & Add Task
        GameObject newTaskInst = Instantiate(newAppWindow.appTask, tasksHolder);
        newTaskInst.name = newAppWindow.appTask.name;
    
        // Connect the Window And The Task
        newAppWindow.appTask = newTaskInst;
        newTaskInst.GetComponent<AppTask>().connectedApp = newAppWindow.gameObject;

        // unfocus the previous window
        if(currActiveWindow != null && currActiveWindow.isFocused)
        {
            currActiveWindow.isFocused = false;
            currActiveWindow.appTask.GetComponent<AppTask>().isFocused = false;
            currActiveWindow = null;
        }

        // Add App To List
        App newApp = new App {
            window = newAppInst,
            task = newTaskInst
        };
        activeApps.Add(newApp);

        // Set the App Active
        currActiveWindow = newAppWindow;
        newAppWindow.isFocused = true;
        newAppWindow.appTask.GetComponent<AppTask>().isFocused = true;

        // Ready The Window For Animation
        RectTransform newAppRect = newAppInst.GetComponent<RectTransform>();
        newAppRect.localScale = new Vector3(0, 0, 0);
        newAppRect.anchoredPosition = new Vector3(0, 0, 0);

        BunnyOSGUI.Instance.LaunchApp(newAppRect);
    }

    public void MinimizeApp(AppWindow window)
    {
        // Minimize Window
        AppTask appTask = window.appTask.GetComponent<AppTask>();

        appTask.isFocused = false;
        appTask.PlayUnfocusAnimation();
        
        window.isFocused = false;
        window.MinimizeWindow();

       SetNewActiveApp();
    }

    GameObject GetLastActiveWindow()
    {
        for (int i = windowsHolder.childCount - 2; i >= 0; i--)
        {
            Transform child = windowsHolder.GetChild(i);
            
            // Check if the current child is active and not being destroyed
            if (child.gameObject.activeSelf && child.TryGetComponent(out AppWindow appWindow) && !appWindow.isMinimized)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    void SetNewActiveApp()
    {
        // Set Last Window As The Current Active Window
        GameObject lastActiveWindow = GetLastActiveWindow();
        if(lastActiveWindow != null && lastActiveWindow.TryGetComponent(out AppWindow newActiveApp) && currActiveWindow != newActiveApp)
        {
            currActiveWindow = newActiveApp;
            newActiveApp.isFocused = true;
            newActiveApp.appTask.GetComponent<AppTask>().isFocused = true;
        }
        else currActiveWindow = null;
    }

    public void CloseApp(GameObject window)
    {
        // Closes the desired Apps Task
        foreach(App app in activeApps)
        {
            if(app.window.name != window.name) continue;

            activeApps.Remove(app);
            app.window.GetComponent<AppWindow>().CloseWindow();
            app.task.GetComponent<AppTask>().CloseTask();
            break;
        }

        SetNewActiveApp();
    }

    // Closes All Apps
    public void CloseAllApps()
    {
        // Close All apps
        foreach(App app in activeApps)
        {
            app.window.GetComponent<AppWindow>().CloseWindow();
            app.task.GetComponent<AppTask>().CloseTask();
        }
        activeApps.Clear();

        currActiveWindow = null;

        OnAllAppsClose?.Invoke();
    }
}

[System.Serializable]
public struct App
{
    public GameObject window;
    public GameObject task;
}