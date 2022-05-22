using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ViewBase : MonoBehaviour
{
    [Inject] protected GameManager _gameManager;
    public abstract void Initialize();
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
