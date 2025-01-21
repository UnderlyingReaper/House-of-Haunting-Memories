using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TeleportBehaviour : MonoBehaviour, IDoorBehaviour
{
    [SerializeField] Transform teleportPoint;
    [SerializeField] float fadeDuration;
    [SerializeField] float delay;
    [SerializeField] AudioClip openClip;


    Transform _player;
    CanvasGroup _fadeCanvas;
    


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _fadeCanvas = GetComponentInChildren<CanvasGroup>();

        _fadeCanvas.alpha = 0;
    }

    public void InteractCancel(Door door) {}

    public void InteractPerform(Door door)
    {
        StartCoroutine(Teleport(door));
    }

    IEnumerator Teleport(Door door)
    {
        GameplayInputManager.Instance.enabled = false;
        _fadeCanvas.DOFade(1, fadeDuration);

        PlaySound(door.audioSource, openClip);
        Sequence _sequence = DOTween.Sequence()
        .Append(door.doorHandle.DORotate(new Vector3(0, 0, 30), 0.5f))
        .Append(door.mainDoor.DOScaleX(0, fadeDuration*2));

        yield return new WaitForSeconds(fadeDuration);
        _player.transform.position = teleportPoint.position;

        yield return new WaitForSeconds(delay);

        _sequence?.Kill();
        door.doorHandle.rotation = Quaternion.Euler(Vector3.zero);
        door.mainDoor.localScale = Vector3.one;

        _fadeCanvas.DOFade(0, fadeDuration);
        GameplayInputManager.Instance.enabled = true;
    }

    public void InteractStart(Door door) {}

    void PlaySound(AudioSource source, AudioClip clip)
    {
        if(source == null) return;
        
        source.volume = Random.Range(0.6f, 0.9f);
        source.pitch = Random.Range(0.85f, 1.2f);

        source.PlayOneShot(clip);
    }
}