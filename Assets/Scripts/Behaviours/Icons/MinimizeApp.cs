using UnityEngine;

public class MinimizeApp : MonoBehaviour, IIconBehaviour
{
    [SerializeField] GameObject mainWindow;

    AppWindow _mainWindowApp;

    void Awake()
    {
        _mainWindowApp = mainWindow.GetComponent<AppWindow>();
    }
    public void Run()
    {
        BunnyOSTaskManager.Instance.MinimizeApp(_mainWindowApp);
    }
}