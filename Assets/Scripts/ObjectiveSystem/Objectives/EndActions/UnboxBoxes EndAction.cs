using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnboxBoxesEndAction : MonoBehaviour, IObjectiveEndAction
{
    [SerializeField] private List<PackedBox> totalBoxList;

    CanvasGroup _canvasGroup;
    public event EventHandler OnExecutionEnd;

    void Awake()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void EndExecute()
    {
        GameplayInputManager.Instance.playerControls.Gameplay.Disable();

        DOTween.Sequence()
            .Append(_canvasGroup.DOFade(1, 2))
            .AppendCallback(() => {
                foreach(PackedBox box in totalBoxList)
                    box.Unbox();
            })
            .Append(_canvasGroup.DOFade(0, 2))
            .AppendCallback(()=> {
                GameplayInputManager.Instance.playerControls.Gameplay.Enable();
                OnExecutionEnd?.Invoke(this, EventArgs.Empty);
            });   
    }
}