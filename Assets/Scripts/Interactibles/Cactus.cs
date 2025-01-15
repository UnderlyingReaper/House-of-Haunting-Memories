using UnityEngine;

public class Cactus : MonoBehaviour, IInteractible, ISpecialInteraction
{
    public bool specialInteraction;
    [SerializeField] private int priority;
    [SerializeField] private string text1 = "Observe", text2 = "Search";
    [SerializeField] private InventoryItem item;

    

    MakePlayerSpeak _makePlayerSpeak;
    

    private void Awake() => _makePlayerSpeak = GetComponent<MakePlayerSpeak>();

    public int GetPriority()
    {
        return priority;
    }
    public void AllowSpecialInteraction() => specialInteraction = true;
    public string GetText()
    {
        if(!enabled) return null;
        else if(specialInteraction) return text2;
        else return text1;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void InteractCancel(Transform interactorTransform) {}
    public void InteractPerform(Transform interactorTransform)
    {
        if(!enabled) return;

        if(specialInteraction)
        {
            _makePlayerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Hint);
            InventoryManager.Instance.AddItem(item);

            specialInteraction = false;
        }
        else _makePlayerSpeak.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
    }
    public void InteractStart(Transform interactorTransform) {}
}