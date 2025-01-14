using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HouseLight : MonoBehaviour
{
    [SerializeField] List<Light2D> lights;
    [SerializeField] SpriteRenderer lightSourceSprite;

    void Awake()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<DayNightHandler>().OnNightSet += DisableLights;
    }

    void DisableLights()
    {
        foreach(Light2D light in lights)
        {
            light.gameObject.SetActive(false);
        }

        lightSourceSprite.color = new Color(lightSourceSprite.color.r, lightSourceSprite.color.g, lightSourceSprite.color.b, 0);
    }
}