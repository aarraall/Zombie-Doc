using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataHandler
{
    public static event Action<int> OnLevelChange, OnCoinChange;
    public static void PlayerProgress()
    {
        SaveManager.Player.level++;
        OnLevelChange?.Invoke(SaveManager.Player.level);
    }

    public static void HandleCoin(int amount)
    {
        SaveManager.Player.coin += amount;
        OnCoinChange?.Invoke(SaveManager.Player.coin);
    }

}
