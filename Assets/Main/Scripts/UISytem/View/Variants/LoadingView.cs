using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingView : ViewBase
{
    [SerializeField] float loadingTime;

    WaitForSeconds wait;

    public override void Initialize()
    {
        wait = new WaitForSeconds(loadingTime);
    }
    public override void Show()
    {
        base.Show();

        StartCoroutine(WaitForLoad());

        IEnumerator WaitForLoad()
        {
            yield return wait;
            _gameManager.SetGameState(GameState.Idle);
        }
    }
}
