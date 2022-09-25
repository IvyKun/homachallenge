using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HomaChallenge/Level Data")]
public class LevelData : ScriptableObject
{
    public List<LevelObjectWithPosition> objectList;
}

[Serializable]
public class LevelObjectWithPosition
{
    public LevelObjectData levelObject;
    public LanePositionEnum lanePosition;
    public float distance = 10f;
}