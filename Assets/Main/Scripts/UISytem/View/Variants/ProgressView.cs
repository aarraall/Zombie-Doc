using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressView : ViewBase
{
    [SerializeField] Slider slider;

    public override void Initialize()
    {
    }

    private void Reset()
    {
        slider = GetComponentInChildren<Slider>();
    }
}
