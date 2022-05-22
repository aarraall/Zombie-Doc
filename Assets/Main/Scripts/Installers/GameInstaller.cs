using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] GameManager _gameManager;
    public override void InstallBindings()
    {
        SaveManager.Initialize();
        Application.targetFrameRate = 60;
        Container.BindInstance(_gameManager).AsSingle().NonLazy();
    }
}