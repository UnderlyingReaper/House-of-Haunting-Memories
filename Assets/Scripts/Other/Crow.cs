using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> behavioursMonoList;
    public Animator animator;

    [Header("Crow Sound")]
    [SerializeField] private AudioClip crowNoise;
    [SerializeField] private AudioClip crowFlySound;
    [SerializeField, Range(0,1)] private float noiseVolume = 1;
    [SerializeField, Range(0,1)] private float flyVolume = 1;
    [SerializeField] private float noiseDelay = 1;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource crowNoiseSource;
    [SerializeField] private AudioSource crowFlySource;


    private List<CrowBehaviour> _behavioursList;
    private bool _noiseLoop;


    private void Awake()
    {
        _behavioursList = new();
        if(behavioursMonoList != null || behavioursMonoList.Count > 0)
        {
            foreach(MonoBehaviour monoBehaviour in behavioursMonoList)
                _behavioursList.Add(monoBehaviour as CrowBehaviour);
        }

        animator = GetComponentInChildren<Animator>();
    }

    private void Start() => StartCoroutine(CrowNoise());

    private void Update()
    {
        if(!enabled) return;
        
        foreach(CrowBehaviour behaviour in _behavioursList)
            behaviour.UpdateFunc(this);
    }

    IEnumerator CrowNoise()
    {
        while(_noiseLoop)
        {
            yield return new WaitForSeconds(noiseDelay);
            MakeCrowNoise();
        }
    }

    public void MakeCrowNoise()
    {
        float vol;
        if(noiseVolume >= 0.3) vol = noiseVolume + Random.Range(-0.2f, 0.2f);
        else vol = noiseVolume + Random.Range(0.05f, 0.3f);

        crowNoiseSource.volume = vol;
        crowNoiseSource.pitch = Random.Range(0.8f, 1.2f);
        crowNoiseSource.PlayOneShot(crowNoise);
    }

    public void StopCrowNoiseLoop() => _noiseLoop = false;
    public void CrowFlySound()
    {
        crowFlySource.volume = flyVolume;
        crowFlySource.PlayOneShot(crowFlySound);
    }
}
