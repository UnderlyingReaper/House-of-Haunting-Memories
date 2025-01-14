using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class OpenCloseBehaviour : MonoBehaviour, IDoorBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float duration;
    [SerializeField] private Collider2D doorCollider;

    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private ShadowCaster2D _shadowCaster;


    void Awake()
    {
        _shadowCaster = GetComponent<ShadowCaster2D>();
    }

    public void InteractCancel(Door door) {}

    public void InteractPerform(Door door)
    {
        if(!isOpen) OpenDoor(door);
        else if(isOpen) CloseDoor(door);
    }

    public void InteractStart(Door door) {}

    void CloseDoor(Door door)
    {
        isOpen = false;
        doorTransform.DOScaleX(0.01f, duration);
        doorCollider.enabled = true;
        _shadowCaster.enabled = true;

        PlaySound(door.audioSource, closeClip);
    }

    public void OpenDoor(Door door, bool withSound = true)
    {
        isOpen = true;
        doorTransform.DOScaleX(1, duration);
        doorCollider.enabled = false;
        _shadowCaster.enabled = false;
        
        if(withSound) PlaySound(door.audioSource, openClip);
    }

    public void CloseDoorWithoutSound(Door door)
    {
        isOpen = false;
        doorTransform.DOScaleX(0.01f, duration);
        doorCollider.enabled = true;
        _shadowCaster.enabled = true;
    }

    void PlaySound(AudioSource source, AudioClip clip)
    {
        source.volume = Random.Range(0.6f, 0.9f);
        source.pitch = Random.Range(0.85f, 1.2f);

        source.PlayOneShot(clip);
    }
}