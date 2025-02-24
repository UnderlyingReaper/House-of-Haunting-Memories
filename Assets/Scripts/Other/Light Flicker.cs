using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float minIntensity;
    [SerializeField] private float initDelay;
    [SerializeField] private float singleFlickDuration;
    [SerializeField] private float flickerDelay;
    [SerializeField] private int flickerAmt;

    [Header("Randomness")]
    [SerializeField] private float initDelayRandomness;
    [SerializeField] private int flickerAmtRandomness;
    


    private List<Light2D> _lights;
    private float _maxIntensity;

    private void Awake()
    {
        _lights = new List<Light2D>(GetComponentsInChildren<Light2D>());
        _maxIntensity = _lights[0].intensity;
    }
    private void OnEnable() => StartCoroutine(Flicker());

    private IEnumerator Flicker()
    {
        while(true)
        {
            yield return new WaitForSeconds(initDelay + Random.Range(-initDelayRandomness, initDelayRandomness));

            for(int i = 0; i < flickerAmt + Random.Range(-flickerAmtRandomness, flickerAmtRandomness); i++)
            {
                foreach(Light2D light in _lights)
                    DOVirtual.Float(light.intensity, minIntensity, singleFlickDuration, value => { light.intensity = value; });

                yield return new WaitForSeconds(flickerDelay);

                foreach(Light2D light in _lights)
                    DOVirtual.Float(light.intensity, _maxIntensity, singleFlickDuration, value => { light.intensity = value; });

                yield return new WaitForSeconds(flickerDelay);
            }

        }
    }
}
