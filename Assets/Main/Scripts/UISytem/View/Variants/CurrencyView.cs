using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyView : ViewBase
{
    [SerializeField] Text text;

    public override void Initialize()
    {
        PlayerDataHandler.OnCoinChange += UpdateView;
        text.text = $"{SaveManager.Player.coin}";
    }

    void UpdateView(int coinAmount)
    {
        text.text = $"{coinAmount}";
    }

    private void Reset()
    {
        text = GetComponentInChildren<Text>();
    }
}
