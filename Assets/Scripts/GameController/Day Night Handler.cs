using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightHandler : MonoBehaviour
{
    public bool isDay = true;

    [Header("Refrences")]
    [SerializeField] private Light2D sunLight;
    [SerializeField] private List<Light2D> lightRays;
    [SerializeField] private CinemachineCamera virtualCam_Night;
    [SerializeField] private AudioSource ambienceSource;

    [Header("Morning")]
    [SerializeField] private AudioSource forestDayAmb;
    [SerializeField] private float sunMorningIntensity = 0.3f;
    [SerializeField] private float raysMorningIntensity = 1;
    [SerializeField] private Color raysMorningColor;
    [SerializeField] private Volume morningVol;


    [Header("Afteroon")]
    [SerializeField] private float sunAfternoonIntensity;
    [SerializeField] private float raysAfternoonIntensity;
    [SerializeField] private Color raysAfternoonColor;

    [Header("Night")]
    [SerializeField] private AudioSource forestNightScaryAmb;
    [SerializeField] private float sunNightIntensity;
    [SerializeField] private float raysNightIntensity;
    [SerializeField] private Color raysNightColor;
    [SerializeField] private Volume nightVol;



    public event Action OnNightSet;
    private float _ambienceOrgVal;

    private void Awake()
    {

        if(isDay)
        {
            _ambienceOrgVal = ambienceSource.volume;
            ambienceSource.volume = 0;
            ambienceSource.DOFade(_ambienceOrgVal, 3);
        }
        else
        {
            forestNightScaryAmb.volume = 0;
            forestNightScaryAmb.DOFade(1, 2);
        }
    }
    
    public void SetAfternoon()
    {
        sunLight.intensity = sunAfternoonIntensity;

        if(!isDay)
        {
            forestDayAmb.gameObject.SetActive(true);
            forestDayAmb.Play();
        }

        foreach(Light2D light in lightRays)
        {
            light.intensity = raysAfternoonIntensity;
            light.color = raysAfternoonColor;
        }

        nightVol.gameObject.SetActive(false);
        morningVol.gameObject.SetActive(true);

        virtualCam_Night.gameObject.SetActive(false);

        isDay = true;
    }

    public void SetMorning()
    {
        sunLight.intensity = sunMorningIntensity;

        forestDayAmb.gameObject.SetActive(true);
        forestDayAmb.Play();


        foreach(Light2D light in lightRays)
        {
            light.intensity = raysMorningIntensity;
            light.color = raysMorningColor;
        }

        nightVol.gameObject.SetActive(false);
        morningVol.gameObject.SetActive(true);

        virtualCam_Night.gameObject.SetActive(false);

        isDay = true;
    }

    public void SetNight()
    {
        sunLight.intensity = sunNightIntensity;

        forestDayAmb.gameObject.SetActive(false);
        forestDayAmb.Stop();

        forestNightScaryAmb.gameObject.SetActive(true);
        forestNightScaryAmb.volume = 0;
        forestNightScaryAmb.DOFade(1, 2);

        foreach(Light2D light in lightRays)
        {
            light.intensity = raysNightIntensity;
            light.color = raysNightColor;
        }

        virtualCam_Night.gameObject.SetActive(true);

        nightVol.gameObject.SetActive(true);
        morningVol.gameObject.SetActive(false);

        ambienceSource.DOFade(0, 2);

        isDay = false;
        OnNightSet?.Invoke();
    }
}
