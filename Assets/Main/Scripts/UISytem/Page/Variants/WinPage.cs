using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPage : PageBase
{
    public void OnNextGameButton()
    {
        PlayerDataHandler.PlayerProgress();
        _gameManager.SetGameState(GameState.Loading);
    }
}
