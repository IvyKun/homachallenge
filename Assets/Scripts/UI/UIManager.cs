using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DigitalRubyShared;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Menus")]
    public GameObject panelMenu;
    public GameObject panelGame;
    public GameObject panelLevelComplete;
    public GameObject panelGameOver;

    [Header("Common")]
    public GameObject panelCoins;
    public Text textCoins;

    [Header("Game")]
    public GameObject panelOnboarding;
    public Slider levelProgressSlider;
    public Text textLevel;

    [Header("Level Complete")]
    public Text textPersons;
    public Text textEarnedCoins;

    [Header("Joystick")]
    public FingersJoystickScript joystickScript;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        panelMenu.SetActive(true);
        panelGame.SetActive(true);
        panelLevelComplete.SetActive(false);
        panelGameOver.SetActive(false);

        textCoins.text = PlayerPreferences.GetCoins().ToString("n0");

        textLevel.text = PlayerPreferences.GetLevel().ToString("n0");
    }

    public void StarGame() 
    {
        panelMenu.SetActive(false);
        

        GameController.instance.SetGameStarted();
    }

    public void ShowLevelComplete(int persons, int earnedCoins, int totalCoins) 
    {
        textPersons.text = string.Concat("x", persons.ToString("n0"));
        textEarnedCoins.text = earnedCoins.ToString("n0");

        textCoins.text = totalCoins.ToString("n0");

        panelGame.SetActive(false);
        panelLevelComplete.SetActive(true);
    }

    public void ShowGameOver() 
    {
        panelMenu.SetActive(false);
        panelGameOver.SetActive(true);
    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void SetLevelProgress(float value)
    {
        levelProgressSlider.value = value;
    }

}
