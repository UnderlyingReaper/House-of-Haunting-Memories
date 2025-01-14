using UnityEngine;

public class Fridge : MonoBehaviour, IInteractible, ISpecialInteraction
{
    [Header("IInteractible Settings")]
    [SerializeField] private bool allowSpecialInteraction;
    [SerializeField] private int priority;
    [SerializeField] private string text = "Take Food";

    [Space(20)]
    [SerializeField] private InventoryItem item;
    [SerializeField] private MonoBehaviour playerSpeakMono;
    [SerializeField] private PlayerSpeechData customSpeechData;
    [SerializeField] private PlayerSpeechData customSpeechData2;


    private IPlayerSpeak _playerSpeak;
    private bool hasTaken = false;

    void Awake()
    {
        _playerSpeak = playerSpeakMono as IPlayerSpeak;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(allowSpecialInteraction && InventoryManager.Instance.CheckForItem(item) == false && !hasTaken)
        {
            allowSpecialInteraction = false;
            InventoryManager.Instance.AddItem(item);
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            hasTaken = true;
        }
        else if(hasTaken) _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, customSpeechData);
        else _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Custom, customSpeechData2);
    }
    public void InteractStart(Transform interactorTransform) {}
    public int GetPriority()
    {
        return priority;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public string GetText()
    {
        if(!enabled) return null;
        return text;
    }

    public void AllowSpecialInteraction()
    {
        allowSpecialInteraction = true;
    }
}