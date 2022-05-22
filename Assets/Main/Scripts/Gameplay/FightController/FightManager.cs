using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : CoreElement
{
    [SerializeField] PlayerController playerController;
    [SerializeField] List<EnemyController> enemies;

    [SerializeField] LevelManager levelManager;

    protected override void StateUpdate(GameState gameState)
    {
        if(gameState == GameState.Loading)
        {
            enemies.Clear();
        }
        if (gameState == GameState.Idle)
            enemies = levelManager.CurrentLevel.Enemies.ToList();


        if (gameState == GameState.PostGameplay)
        {
            playerController.ZombieContainer.ForEach(zombie =>
            {
                zombie.OnDeath += CheckZombies;
                zombie.SetFightReady(enemies);
            });
            enemies.ForEach(enemy =>
            {
                enemy.OnDeath += CheckEnemies;
                enemy.SetFightReady(playerController.ZombieContainer);
            });

            _gameManager.SetGameState(GameState.Fight);
        }
    }

    private void CheckZombies()
    {
        if (playerController.ZombieContainer.All(zombie => zombie.IsDead))
            _gameManager.SetGameState(GameState.Lose);
    }
    private void CheckEnemies()
    {
        if (enemies.All(enemy => enemy.IsDead))
            _gameManager.SetGameState(GameState.Win);
    }


    private void Reset()
    {
        enemies = GetComponentsInChildren<EnemyController>().ToList();
    }


}
