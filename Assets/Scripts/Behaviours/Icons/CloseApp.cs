using UnityEngine;

public class CloseApp : MonoBehaviour, IIconBehaviour
{
    [SerializeField] GameObject mainWindow;



    public void Run()
    {
        BunnyOSTaskManager.Instance.CloseApp(mainWindow);
    }
}