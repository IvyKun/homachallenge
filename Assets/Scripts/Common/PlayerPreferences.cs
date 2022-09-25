using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for read and write the player preferences
/// </summary>
public static class PlayerPreferences
{
    public static int GetCharacterSelected()
    {
        return PlayerPrefs.GetInt("CHARACTER_SELECTED", 0);
    }

    public static void SetCharacterSelected(int value)
    {
        PlayerPrefs.SetInt("CHARACTER_SELECTED", value);
    }

    public static void SetCoins(int value)
    {
        PlayerPrefs.SetInt("COINS", value);
    }

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt("COINS", 0);
    }

    public static int GetLevel()
    {
        return PlayerPrefs.GetInt("LEVEL", 1);
    }

    public static void IncreaseLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("LEVEL", 1);
        currentLevel += 1;
        PlayerPrefs.SetInt("LEVEL", currentLevel);
    }
}
