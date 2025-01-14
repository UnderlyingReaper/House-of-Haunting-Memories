using UnityEngine;

public class GameplayInputManager : MonoBehaviour
{
    public PlayerControls playerControls;

    public static GameplayInputManager Instance { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            playerControls = new();
            Instance = this;
        }
        else Destroy(gameObject);

        playerControls.Mouse.Enable();
    }

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerControls.Gameplay.Enable();
    }

    void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }

    void OnDestroy()
    {
        playerControls.Mouse.Disable();
        playerControls.Gameplay.Disable();
        playerControls.UI.Disable();
    }
}