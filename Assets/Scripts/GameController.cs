using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Classic controller to control the flow and status of the game.
/// It would start by loading the level, the player configuration, etc.
/// Also it would keep the status of the current gameplay for everyone else to access it
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public ConfigData configData;

    public GameStateEnum gameState = GameStateEnum.Idle;

    public int level;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        level = PlayerPreferences.GetLevel();

        InstantiateLevel();
    }

    void InstantiateLevel()
    {
        if (level <= configData.levels.Count)
        {
            Instantiate(configData.levels[level - 1]);
        }
        else
        {
            int levelForLoop = (level - configData.levels.Count) % configData.levels.Count;
            if (levelForLoop == 0)
            {
                levelForLoop = configData.levels.Count;
            }

            Instantiate(configData.levels[levelForLoop - 1]);
        }


    }

    public void SetGameStarted()
    {
        gameState = GameStateEnum.Started;

        PlayerController.instance.StartRun();
    }

    public void SetGameOver() 
    {
        gameState = GameStateEnum.Over;

        UIManager.instance.ShowGameOver();
    }

    public void SetLevelCompleted()
    {
        gameState = GameStateEnum.Over;

        PlayerPreferences.IncreaseLevel();

        LevelController.instance.ShowConfetti();

        //ShowLevelCompleted();
    }

    public void ShowLevelCompleted()
    {
        int currentCoins = PlayerPreferences.GetCoins();

        int numPersons = PlayerController.instance.personsList.Count;
        int coinsPerPerson = configData.coinsPerPerson;
        int earnedCoins = numPersons * coinsPerPerson;

        int newCoins = currentCoins + earnedCoins;

        PlayerPreferences.SetCoins(newCoins);

        UIManager.instance.ShowLevelComplete(numPersons, earnedCoins, newCoins);
    }

    public void SetGamePaused()
    {
        gameState = GameStateEnum.Paused;
    }


}
