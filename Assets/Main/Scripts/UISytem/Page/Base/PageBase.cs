using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PageBase : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected ViewBase[] views;

    [Inject]
    public virtual void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;

        foreach (var view in views)
            view.Initialize();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        foreach (var view in views)
            view.Show();
    }
    public virtual void Hide()
    {
        foreach (var view in views)
            view.Hide();
        gameObject.SetActive(false);
    }

    private void Reset()
    {
        views = GetComponentsInChildren<ViewBase>();
    }
}
