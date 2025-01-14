using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCrashTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private OnTriggerEnterEventTrigger car;
    [SerializeField] private HouseMissingTrigger houseMissingTrigger;
    [SerializeField] private float carSpeed;
    [SerializeField] private AudioSource honkAudioSource;
    [SerializeField] private AudioSource carDriveAudioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip bassHitClip;



    private float _time;
    private CanvasGroup canvasGroup;
    private AudioSource _audioSource;
    


    private void Awake()
    {
        car.OnTriggerEnter += OnCarHit;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
        _audioSource = GetComponentInChildren<AudioSource>();

        _time = Vector3.Distance(car.transform.position, transform.position) / carSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            SyncDataManager.Instance.DidSeeSophiaDie = true;
            car.gameObject.SetActive(true);
            car.transform.DOMoveX(-45, _time).SetEase(Ease.Linear);
            enabled = false;
        }
    }

    private void OnCarHit()
    {
        DOTween.Sequence()
        .AppendCallback(() => {
            canvasGroup.alpha = 1;
            _audioSource.PlayOneShot(bassHitClip);

            honkAudioSource.Stop();
            carDriveAudioSource.Stop();
            houseMissingTrigger.heavyBreathingSource.DOFade(0, 1);
            houseMissingTrigger.heartBeatingSource.DOFade(0, 1);
        })
        .AppendInterval(6)
        .AppendCallback(() => SceneManager.LoadScene(sceneName));
    }
}