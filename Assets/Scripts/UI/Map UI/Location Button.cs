using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool isInActiveOnAwake;
    public Button button;


    private TextMeshProUGUI _locationName;
    private RectTransform rectTransform;



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        _locationName = transform.GetChild(transform.childCount - 1).GetComponent<TextMeshProUGUI>();

        if(isInActiveOnAwake) enabled = false;
    }

    void OnEnable()
    {
        _locationName.DOFade(1, 1);
        button.enabled = true;
    }
    void OnDisable()
    {
        _locationName.DOFade(0, 1);
        button.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.15f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOScale(Vector3.one, 0.15f);
    }
}