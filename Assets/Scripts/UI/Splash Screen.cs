using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private List<Logo> logos;
    [SerializeField] private float fadeDuration = 1;


    private void Awake()
    {
        Sequence sequence = DOTween.Sequence();

        foreach(Logo logo in logos)
        {
            sequence.Append(logo.image.DOFade(1, fadeDuration));
            sequence.AppendInterval(logo.timeDuration);
            sequence.Append(logo.image.DOFade(0, fadeDuration));
        }

        sequence.OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
}

[Serializable]
struct Logo {
    public CanvasGroup image;
    public float timeDuration;
}
