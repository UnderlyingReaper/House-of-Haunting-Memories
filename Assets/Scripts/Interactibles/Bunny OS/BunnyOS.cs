using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BunnyOS : MonoBehaviour
{

    [Header("Start / Shutdown")]
    [SerializeField] CanvasGroup introBg;
    [SerializeField] CanvasGroup startingTxt;
    [SerializeField] CanvasGroup osTxt;

    [Header("Operating System GUI")]
    [SerializeField] GameObject osGUI;

    [Header("Sounds")]
    [SerializeField] AudioClip startupClip;
    [SerializeField] AudioClip shutDownClip;


    AudioSource _audioSource;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public IEnumerator StartOS()
    {
        LetterBox.Instance.enabled = false;

        StartCoroutine(TxtAnimation("Starting"));
        introBg.DOFade(1, 2);
        yield return new WaitForSeconds(3);

        _audioSource.PlayOneShot(startupClip);

        startingTxt.DOFade(1, 1);
        osTxt.DOFade(1, 1);

        yield return new WaitForSeconds(6);

        startingTxt.alpha = 0;
        osTxt.alpha = 0;

        yield return new WaitForSeconds(2);

        osGUI.SetActive(true);
        introBg.DOFade(0, 0.5f);
        StopAllCoroutines();
    }

    public IEnumerator ShutdownOS()
    {
        LetterBox.Instance.enabled = true;
        
        StartCoroutine(TxtAnimation("Shutting Down"));
        introBg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);

        _audioSource.PlayOneShot(shutDownClip);

        startingTxt.DOFade(1, 0.5f);

        yield return new WaitForSeconds(1.5f);

        startingTxt.alpha = 0;
        osGUI.SetActive(false);
        introBg.alpha = 0;

        StopAllCoroutines();
    }

    IEnumerator TxtAnimation(string textToSet)
    {
        TextMeshProUGUI textUGUI = startingTxt.GetComponent<TextMeshProUGUI>();
        textUGUI.text = textToSet;
        float delay = 0.7f;

        while(true)
        {
            textUGUI.text += ".";
            yield return new WaitForSeconds(delay);
            textUGUI.text += ".";
            yield return new WaitForSeconds(delay);
            textUGUI.text += ".";
            yield return new WaitForSeconds(delay);
            textUGUI.text = textToSet;
            yield return new WaitForSeconds(delay);
        }
    }
}