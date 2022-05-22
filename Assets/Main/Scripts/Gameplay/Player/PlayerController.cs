using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CoreElement
{
    [SerializeField] PlayerModel model;
    [SerializeField] PlayerView view;

    IInputHandler<Vector3> _inputHandler;

    GameState _gameState;

    WaitForSeconds _throwWait;

    List<ZombieController> _zombieContainer = new List<ZombieController>();
    public List<ZombieController> ZombieContainer => _zombieContainer;
    public override void Initialize()
    {
        base.Initialize();

        _inputHandler = new SwerveHandler();
        _throwWait = new WaitForSeconds(model.syringeThrowRate);

        if (view == null)
            view = Instantiate(model.playerPrefab);

        view.Initialize(model.animationSet);

        view.OnTriggerEnterEvent += OnTriggerEnterDetected;
        view.FOV.OnTargetsFound += OnFOVTargetsFound;
    }

    private void OnTriggerEnterDetected(Collider other)
    {
        switch (other.tag)
        {
            case "Currency":
                other.gameObject.SetActive(false);
                PlayerDataHandler.HandleCoin(model.coinIncrement);
                break;
            case "Obstacle":
                break;
            case "Finish":
                _gameManager.SetGameState(GameState.PostGameplay);
                break;
            case "Fight":
                view.Fight();
                break;
        }
    }
    private void OnFOVTargetsFound(List<Transform> targets)
    {
        List<ZombieController> unHitZombies = new List<ZombieController>();

        foreach (var target in targets)
        {
            ZombieController zombie = target.GetComponentInParent<ZombieController>();
            if (!zombie.IsHit)
                unHitZombies.Add(zombie);
        }

        if (SaveManager.Player.coin == 0) return;
        StartCoroutine(ThrowSyringeCoroutine());

        IEnumerator ThrowSyringeCoroutine()
        {
            foreach (var target in unHitZombies)
            {
                Syringe syringe = ObjectPoolProxy.GetItem("Syringe") as Syringe;
                syringe.Throw(view.transform, target.transform, model.syringeThrowSpeed);
                target.IsHit = true;
                PlayerDataHandler.HandleCoin(-1);
                _zombieContainer.Add(target);
                yield return _throwWait;
            }
        }

    }

    protected override void StateUpdate(GameState gameState)
    {
        _gameState = gameState;

        switch (gameState)
        {
            case GameState.Loading:
                _zombieContainer.ForEach(zombie => Destroy(zombie.gameObject));
                _zombieContainer.Clear();
                view.transform.position = Vector3.zero;
                break;
            case GameState.Idle:
                break;
            case GameState.Gameplay:
                view.Move();
                break;
            case GameState.Fight:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void MovePlayerView()
    {
        Vector3 movement = new Vector3(_inputHandler.Output.x * model.sideSpeed, 0, model.forwardSpeed);
        view.transform.position += Time.deltaTime * movement;
        view.transform.position = new Vector3(Mathf.Clamp(view.transform.position.x, -model.clampX, model.clampX), view.transform.position.y, view.transform.position.z);
    }

    private void Update()
    {
        if (_gameState != GameState.Gameplay) return;

        _inputHandler.Update();
        MovePlayerView();
    }
}

