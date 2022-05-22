using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePage : PageBase
{
    public void OnReplayGameButton()
    {
        _gameManager.SetGameState(GameState.Loading);
    }
}
