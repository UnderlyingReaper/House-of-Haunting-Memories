using UnityEngine;

public class PlaySoundInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;


    public void Initialize()
    {
        audioSource.PlayOneShot(audioClip);
    }
}