using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HomaChallenge/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Movement")]
    public float forwardSpeed = 6f;
    public float horizontalSpeed = 8f;

    [Header("Group")]
    public float gravity = -20f;
    public float personDeactivateOffset = 20f;
    [Tooltip("How fast a follower will get to a position on the line")]
    public float speedMovePersonToPosition = 5f;
    [Tooltip("How fast a follower will get to a position at the end of the level")]
    public float speedMovePersonToPositionEnd = 1f;
    [Tooltip("How fast a follower will rotate at the end of the level")]
    public float timeMovePersonRotateToFront = 2f;

    [Header("Models")]
    public List<GameObject> playerList;

}
