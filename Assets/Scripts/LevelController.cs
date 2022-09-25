using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a level and hold the current level configuration.
/// Create the level structure and obstalces.
/// </summary>
public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public Transform finishLine;
    public Transform endPosition;
    public GameObject vfxConfetti;

    public LevelData levelData;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InstantiateObstacles();
    }

    void Update()
    {
        if (GameController.instance.gameState == GameStateEnum.Started)
        {
            SetLevelProgress();
        }
    }

    /// <summary>
    /// Create the level obstacles in the configured position
    /// </summary>
    void InstantiateObstacles() 
    {
        Vector3 objectPosition;
        float nextZPosition = 0;

        ObstacleData obstacleData;
        PersonData personData;

        foreach (LevelObjectWithPosition levelObjectData in levelData.objectList) 
        {
            GameObject goLevelObject;

            nextZPosition += levelObjectData.distance;
            objectPosition = new Vector3(GetPositionBasedOnLane(levelObjectData.lanePosition), 0, nextZPosition);

            if (levelObjectData.levelObject is PersonData)
            {
                personData = (PersonData)levelObjectData.levelObject;

                goLevelObject = Instantiate(personData.modelList[Random.Range(0, personData.modelList.Count)], transform);
                goLevelObject.transform.localPosition = objectPosition;
                goLevelObject.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            }
            else if (levelObjectData.levelObject is ObstacleData)
            {
                obstacleData = (ObstacleData)levelObjectData.levelObject;

                goLevelObject = Instantiate(obstacleData.modelList[Random.Range(0, obstacleData.modelList.Count)], transform);
                goLevelObject.transform.localPosition = objectPosition;
                goLevelObject.transform.localRotation = Quaternion.identity;
            }
            else 
            { 
                // It's an empty space, so do nothing because the distance is already added
            }

        
        }
    
    }

    /// <summary>
    /// Gets the X position of an object in a lane
    /// </summary>
    /// <param name="lanePosition"></param>
    /// <returns></returns>
    float GetPositionBasedOnLane(LanePositionEnum lanePosition) 
    {
        float position = 0;
        switch (lanePosition) 
        { 
            case LanePositionEnum.Left:
                position = - 2.5f;
                break;

            case LanePositionEnum.Right:
                position = 2.5f;
                break;

            default:
                position = 0;
                break;
        }

        return position;
    }

    /// <summary>
    /// Updates the level progress slider using the player position
    /// </summary>
    void SetLevelProgress()
    {
        float percentage = 0;
        if (PlayerController.instance.transform.position.z > 0)
        {
            percentage = PlayerController.instance.transform.position.z / finishLine.position.z;
        }

        UIManager.instance.SetLevelProgress(percentage);

    }

    /// <summary>
    /// Shows the confetti VFX at the finish line
    /// </summary>
    public void ShowConfetti() 
    {
        vfxConfetti.SetActive(true);
    }
}
