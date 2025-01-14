using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] private List<InventoryItem> itemsList;
    [SerializeField] private Image[] containers;
    [SerializeField] private RectTransform invDisplayerRect;
    [SerializeField] private MonoBehaviour playerSpeakMono;


    private float _orgAnchorPosY;
    private IPlayerSpeak _playerSpeak;
    private DG.Tweening.Sequence _removingSequence;
    



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            _orgAnchorPosY = invDisplayerRect.anchoredPosition.y;

            if(itemsList.Count == 0) invDisplayerRect.DOAnchorPosY(Mathf.Abs(_orgAnchorPosY), 1);

            _playerSpeak = playerSpeakMono as IPlayerSpeak;
        }
        else Destroy(gameObject);
    }

    public bool IsSlotEmpty()
    {
        if(itemsList.Count < containers.Length) return true;
        else return false;
    }

    public void AddItem(InventoryItem itemToStore)
    {
        if(!IsSlotEmpty())
        {
            _playerSpeak?.SpeakPlayer(IPlayerSpeak.SpeechType.Main);
            return;
        }

        _removingSequence?.Kill();
        itemsList.Add(itemToStore);

        for(int i = 0; i < containers.Length; i++)
        {
            if(containers[i].sprite == null)
            {
                containers[i].sprite = itemToStore.imageSprite;
                containers[i].DOFade(1, 1);
                break;
            }
        }

        if(itemsList.Count > 0) invDisplayerRect.DOAnchorPosY(-Mathf.Abs(_orgAnchorPosY), 1);
    }

    public void RemoveItem(InventoryItem itemToRemove)
    {
        itemsList.Remove(itemToRemove);

        for(int i = 0; i < containers.Length; i++)
        {
            if(containers[i].sprite == itemToRemove.imageSprite)
            {
                _removingSequence?.Kill();
                _removingSequence = DOTween.Sequence()
                .Append(containers[i].DOColor(new Color(containers[i].color.r, containers[i].color.g, containers[i].color.b, 0), 0.5f))
                .OnComplete(() => containers[i].sprite = null);
                break;
            }
        }

        if(itemsList.Count == 0) invDisplayerRect.DOAnchorPosY(Mathf.Abs(_orgAnchorPosY), 1);
    }

    public bool CheckForItem(InventoryItem itemToCheck)
    {
        foreach(InventoryItem item in itemsList)
        {
            if(item.id == itemToCheck.id) return true;
        }

        return false;
    }
}
