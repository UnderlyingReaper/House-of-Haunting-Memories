using UnityEngine;

public class SimpleLockBehaviour : MonoBehaviour, ILockable
{
    [SerializeField] bool isLocked;
    [SerializeField] AudioClip unlockSound;
    [SerializeField] InventoryItem keyItem;
    [SerializeField] MonoBehaviour playerSpeakMono;



    IPlayerSpeak _playerSpeak;

    void Awake()
    {
        _playerSpeak = playerSpeakMono as IPlayerSpeak;
    }

    public bool CheckIfLocked()
    {
        return isLocked;
    }

    public void TryUnlock()
    {
        if(InventoryManager.Instance.CheckForItem(keyItem) == false)
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            return;
        }

        isLocked = false;
        InventoryManager.Instance.RemoveItem(keyItem);
        GetComponent<AudioSource>().PlayOneShot(unlockSound);

        _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Hint);
    }
}