using System;
using DG.Tweening;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform playerMarker;
    [SerializeField] private Vector2 markerOffset = new Vector3(0, -50);

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip engineStart;

    

    public event Action OnMoveStart;
    public event Action OnMoveEnd;



    public void OnHousePressButton(HouseInfo house)
    {
        if(house.gameObject.activeSelf) return;

        house.gameObject.SetActive(true);
        player.transform.position = house.playerSpawnPoint.position;
        transform.parent.transform.position = house.carSpawnPoint.position;

        house.SetCameraConfiner();

        audioSource.PlayOneShot(engineStart);
        audioSource.Play();
        audioSource.DOFade(1, 0.5f);

        OnMoveStart?.Invoke();
        playerMarker.DOAnchorPos(house.correspondingButtonRect.anchoredPosition + markerOffset, 10).SetEase(Ease.InOutSine)
        .OnComplete(() => {
            OnMoveEnd?.Invoke();
            audioSource.DOFade(0, 0.5f).OnComplete(() => audioSource.Stop());
        });

        
    }
} 
