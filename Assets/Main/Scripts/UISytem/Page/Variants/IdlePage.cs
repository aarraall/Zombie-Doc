using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdlePage : PageBase
{
    public void OnTapToPlay()
    {
        _gameManager.SetGameState(GameState.Gameplay);
    }
}
