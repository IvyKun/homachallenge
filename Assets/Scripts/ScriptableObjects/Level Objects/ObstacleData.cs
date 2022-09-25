using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HomaChallenge/Level Object/Obstacle")]
public class ObstacleData: LevelObjectData
{
    [Header("Models")]
    public List<GameObject> modelList;

}
