using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScalingGameObject : ViewBase
{
    [SerializeField] float loopAmount, scaleModifier, duration;


    public override void Initialize()
    {
    }

    public override void Show()
    {
        base.Show();
        transform.DOScale(transform.localScale * scaleModifier, duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void Hide()
    {
        base.Hide();
    }
}
