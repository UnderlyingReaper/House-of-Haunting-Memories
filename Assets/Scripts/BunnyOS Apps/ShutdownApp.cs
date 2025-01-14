using UnityEngine;

public class ShutdownApp : MonoBehaviour, IIconBehaviour
{
    [SerializeField] Computer computer;


    public void Start()
    {
        BunnyOSTaskManager.Instance.OnAllAppsClose += Shutdown;
    }

    public void Run()
    {
        BunnyOSTaskManager.Instance.CloseAllApps();
    }

    public void Shutdown()
    {
        computer.Shutdown();
    }
}