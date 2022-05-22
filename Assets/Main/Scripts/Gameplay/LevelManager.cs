using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : CoreElement
{
    [SerializeField] List<Level> levelPrefabs;
    Level currentLevel; public Level CurrentLevel => currentLevel;
    protected override void StateUpdate(GameState gameState)
    {
        if (gameState == GameState.Loading)
        {
            if (currentLevel != null)
                Destroy(currentLevel.gameObject);


            if (SaveManager.Player.level > levelPrefabs.Count)
                currentLevel = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Count)]);
            else
                currentLevel = Instantiate(levelPrefabs[SaveManager.Player.level - 1]);
        }
    }
}
