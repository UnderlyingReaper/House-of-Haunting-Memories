using UnityEngine;

public class SpeechInteraction : MonoBehaviour, IInteractible
{
    [SerializeField] private int priority;
    [SerializeField] private string text;
    [SerializeField] private int numberOfInteractions;


    IPlayerSpeak _playerSpeak;




    void Awake()
    {
        _playerSpeak = GetComponent<IPlayerSpeak>();
    }

    public int GetPriority()
    {
        return priority;
    }
    public string GetText()
    {
        return text;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}

    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;
        
        if(numberOfInteractions > 0 || numberOfInteractions == -1)
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            if(numberOfInteractions != -1) numberOfInteractions--; 
        }
        else enabled = false;
    }

    public void InteractStart(Transform interactorTransform) {}
}
