using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HomaChallenge/ConfigData")]
public class ConfigData : ScriptableObject
{
    [Header("Misc")]
    public LayerMask groundLayer;

    [Header("Economy")]
    [Tooltip("How many coins the player gets for each follower at the end of the level")]
    public int coinsPerPerson = 10;

    [Header("Materials")]
    public Material materialPersonActive;
    public Material materialPersonInactive;

    [Header("Animation Controllers")]
    public RuntimeAnimatorController animationControllerPlayer;
    public RuntimeAnimatorController animationControllerPerson;

    [Header("Levels")]
    public List<GameObject> levels;

}
