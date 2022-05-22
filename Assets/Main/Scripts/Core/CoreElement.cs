using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class CoreElement : MonoBehaviour
{
    [Inject] protected GameManager _gameManager;
    protected virtual void Awake()
    {
        Initialize();
    }
    public virtual void Initialize()
    {
        GameManager.OnGameStateChange += StateUpdate;
    }

    protected abstract void StateUpdate(GameState gameState);
}
