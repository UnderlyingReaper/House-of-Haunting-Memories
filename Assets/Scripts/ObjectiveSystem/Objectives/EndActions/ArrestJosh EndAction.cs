using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ArrestJoshEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private AudioMixer mixer;
    private Animator _animator;

    public event EventHandler OnExecutionEnd;

    private AudioSource _audioSource;
    private float _defaultVolume = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void EndExecute()
    {
        _animator.SetTrigger("Arrest");
        GameplayInputManager.Instance.playerControls.Gameplay.Disable();

        OnExecutionEnd?.Invoke(this, EventArgs.Empty);
    }

    public void TeleportToNewsScene()
    {
        GameplayInputManager.Instance.playerControls.Gameplay.Enable();
        mixer.SetFloat("MasterVol", _defaultVolume);

        SceneManager.LoadScene("The News");
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void MuteAllAudio()
    {
        _defaultVolume = PlayerPrefs.GetFloat("MasterVol");
        mixer.DOSetFloat("MasterVol", -80f, 1);
    }
}
