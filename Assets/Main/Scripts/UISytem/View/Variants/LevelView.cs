using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : ViewBase
{
    [SerializeField] string prefix, postfix;
    [SerializeField] Text levelText;
    public override void Initialize()
    {
        PlayerDataHandler.OnLevelChange += UpdateView;
        levelText.text = $"{prefix} {SaveManager.Player.level} {postfix}";
    }

    private void UpdateView(int level)
    {
        levelText.text = $"{prefix} {level} {postfix}";
    }
}
