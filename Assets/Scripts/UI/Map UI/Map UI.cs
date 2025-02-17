using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private List<LocationInfo> locationsList;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform playerMarker;
    [SerializeField] private Vector2 markerOffset = new Vector3(0, -50);

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip engineStart;

    

    public event Action OnMoveStart;
    public event Action OnMoveEnd;



    public void OnLocationPressButton(LocationInfo location)
    {
        if(location.gameObject.activeSelf) return;

        foreach(LocationInfo location2 in locationsList)
            location2.gameObject.SetActive(false);


        location.gameObject.SetActive(true);
        player.transform.position = location.playerSpawnPoint.position;
        transform.parent.transform.position = location.carSpawnPoint.position;
        location.SetCameraConfiner();


        audioSource.PlayOneShot(engineStart);
        audioSource.Play();
        audioSource.DOFade(1, 0.5f);

        OnMoveStart?.Invoke();
        playerMarker.DOAnchorPos(location.correspondingButtonRect.anchoredPosition + markerOffset, 15).SetEase(Ease.InOutSine)
        .OnComplete(() => {
            OnMoveEnd?.Invoke();
            audioSource.DOFade(0, 0.5f).OnComplete(() => audioSource.Stop());
        });

        
    }
} 
