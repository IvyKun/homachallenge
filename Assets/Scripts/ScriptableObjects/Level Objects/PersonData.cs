using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HomaChallenge/Level Object/Person")]
public class PersonData : LevelObjectData
{
    [Header("Models")]
    public List<GameObject> modelList;
}
