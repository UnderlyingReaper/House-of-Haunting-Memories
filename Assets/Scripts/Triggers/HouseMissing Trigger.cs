using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HouseMissingTrigger : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSourceList;
    [SerializeField] public AudioSource heavyBreathingSource;
    [SerializeField] public AudioSource heartBeatingSource;

    [SerializeField] private float muteDuration = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!enabled) return;
        if(other.tag == "Player")
        {
            foreach(AudioSource audioSource in audioSourceList)
            {
                audioSource.DOFade(0, muteDuration);
                enabled = false;
            }

            float orgVol = heavyBreathingSource.volume;
            heavyBreathingSource.volume = 0;
            heavyBreathingSource.Play();
            heavyBreathingSource.DOFade(orgVol, 5);

            orgVol = heartBeatingSource.volume;
            heartBeatingSource.volume = 0;
            heartBeatingSource.Play();
            heartBeatingSource.DOFade(orgVol, 5);
        }
    }
}