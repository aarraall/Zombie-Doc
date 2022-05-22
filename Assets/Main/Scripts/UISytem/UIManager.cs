using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIManager : CoreElement
{
    [SerializeField]
    List<PageBase> _pages = new List<PageBase>();

    [SerializeField] bool _findChildrenOnInitialize;

    public void ShowPage(int index)
    {
        HideAll();
        _pages[index].Show();
    }
    public void HideAll()
    {
        _pages.ForEach(page => page.Hide());
    }
    protected override void StateUpdate(GameState gameState)
    {
        HideAll();
        switch (gameState)
        {
            case GameState.Loading:
                _pages[0].Show();
                break;
            case GameState.Idle:
                _pages[1].Show();
                break;
            case GameState.Gameplay:
                _pages[2].Show();
                break;
            case GameState.Win:
                _pages[3].Show();
                break;
            case GameState.Lose:
                _pages[4].Show();
                break;
        }
    }
    private void Reset()
    {
        _pages = GetComponentsInChildren<PageBase>().ToList();
    }
}
