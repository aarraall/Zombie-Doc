using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    public static int DEFAULT_BOSS_DAMAGE = Random.Range(60, 80);
    public static int BOSS_DAMAGE_WITH_LEVEL_MODIFIER => Mathf.RoundToInt((float)DEFAULT_BOSS_DAMAGE + (float)SaveManager.Player.level * 1.05f) + Random.Range(1,10);
    public static int CARD_DAMAGE_MODIFIER => SaveManager.Player.level * 10;
    public static int DNA_VALUE => 15;
    public static int PRICE_RELATED_CARD_DAMAGE(int basePrice) => basePrice * SaveManager.Player.level * 5;
}
