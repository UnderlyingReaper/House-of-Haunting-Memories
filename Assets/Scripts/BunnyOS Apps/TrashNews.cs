using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrashNews : MonoBehaviour
{
    [SerializeField] private int password = 2801;
    [SerializeField] private GameObject newsGameObject;
    [SerializeField] private GameObject passGameObject;
    [SerializeField] private TMP_InputField inputField;


    void Awake()
    {
        if(SyncDataManager.Instance.DidSeeNews) newsGameObject.SetActive(true);
        else passGameObject.SetActive(true);
    }
    public void OnPasswordSubmit()
    {
        if(inputField.text != password.ToString())
        {
            DOTween.Sequence()
            .AppendCallback(() => inputField.text = "Incorrect")
            .AppendInterval(1)
            .AppendCallback(() => inputField.text = null);
            return;
        }

        newsGameObject.SetActive(true);
        passGameObject.SetActive(false);
        inputField.text = null;
        
        SyncDataManager.Instance.DidSeeNews = true;
    }
}