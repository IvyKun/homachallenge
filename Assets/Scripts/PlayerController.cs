using Cinemachine;
using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour of a the player.
/// Controls the movement and person management.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public PlayerData playerData;

    public Transform playerContainer;
    public Transform personsContainer;

    float forwardSpeed;
    float horizontalSpeed;

    // Movement
    Vector3 direction;
    Vector3 movement;

    //Persons
    public List<PersonController> personsList;
    public PersonController playerPerson;
    

    public List<Vector3> personPositions = new List<Vector3>();
    public List<Vector3> availablePositions = new List<Vector3>();
    int maxPersonPositions = 50;
    float distanceBetweenPersons = 0.5f;
    float personZPosition;
    float personXPosition;
    Vector3 personPosition;

    public GameObject vfxGetPerson;

    void Awake()
    {
        instance = this;

    }

    void Start()
    {
        forwardSpeed = playerData.forwardSpeed;
        horizontalSpeed = playerData.horizontalSpeed;

        UIManager.instance.joystickScript.JoystickExecuted = JoystickExecuted;

        // Add player person to container
        int characterSelected = PlayerPreferences.GetCharacterSelected();
        GameObject goPlayer = Instantiate(playerData.playerList[characterSelected], playerContainer);
        goPlayer.transform.localPosition = Vector3.zero;
        goPlayer.transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerPerson = goPlayer.GetComponent<PersonController>();
        playerPerson.isPlayer = true;
        playerPerson.Added();

        CreatePersonPositions();

    }

    void Update()
    {
        if (GameController.instance.gameState == GameStateEnum.Started)
        {
            Move();
        }

    }

    void JoystickExecuted(FingersJoystickScript script, Vector2 joystickDirection)
    {
        direction = new Vector3(joystickDirection.x, 0, 0);
    }

    /// <summary>
    /// Manages the forward movent that is fixed.
    /// Manages the horizontal movement based on the user input.
    /// </summary>
    void Move()
    {
        movement = new Vector3(direction.x * horizontalSpeed, 0, forwardSpeed);

        transform.Translate(movement * Time.deltaTime);

    }

    /// <summary>
    /// Called when player touches a person on the level.
    /// Adds the person to the followers list and set the position in the queue
    /// </summary>
    /// <param name="person"></param>
    public void AddPerson(PersonController person)
    {
        if (person.isInPlayerGroup == false && person.isPlayer == false)
        {
            person.isInPlayerGroup = true;
            person.transform.parent = personsContainer;
            personsList.Add(person);

            person.Added();

            personPosition = availablePositions[0];
            availablePositions.RemoveAt(0);

            person.MoveToPositition(personPosition, playerData.speedMovePersonToPosition);

            // VFX
            if (vfxGetPerson.activeSelf) 
            {
                vfxGetPerson.SetActive(false);
            }
            vfxGetPerson.SetActive(true);
        }
    }

    /// <summary>
    /// Called when a person crash with an obstacle.
    /// Remove the person from the list and set that person position to available for another person.
    /// </summary>
    /// <param name="person"></param>
    public void LosePerson(PersonController person)
    {
        Vector3 newAvailablePosition = person.transform.localPosition;
        newAvailablePosition.y = 0;
        availablePositions.Insert(0, newAvailablePosition);

        person.isInPlayerGroup = false;
        person.transform.parent = null;

        personsList.Remove(person);
    }

    /// <summary>
    /// Start the player walk animation
    /// </summary>
    public void StartRun()
    {
        playerPerson.StartMoving();
    }

    /// <summary>
    /// Finish the run and start the end level minigame
    /// </summary>
    public void FinishRun()
    {
        if (GameController.instance.gameState == GameStateEnum.Started)
        {
            GameController.instance.SetLevelCompleted();

            StartCoroutine(MoveToEndPosition());
        }
    }

   /// <summary>
   /// Create a list of positions behind the player to position new followers.
   /// </summary>
    void CreatePersonPositions()
    {
        for(int i = 1; i <= maxPersonPositions; i++) 
        { 
            // This is just set some assigned position to the persons following
            personZPosition = distanceBetweenPersons * i * -1;
            personXPosition = distanceBetweenPersons;
            if (i % 2 == 0)
            {
                personXPosition = personXPosition * -1;
            }

            Vector3 personPosition = new Vector3(personXPosition, 0, personZPosition);
            personPositions.Add(personPosition);

        }

        availablePositions.AddRange(personPositions);
    }

    /// <summary>
    /// Move the player and followers to a position behind the finish lane to start the minigame
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToEndPosition()
    {
        Vector3 targetPosition = LevelController.instance.endPosition.position;
        float moveSpeed = forwardSpeed / 1.5f;

        while (transform.position.z < targetPosition.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        playerPerson.LookAtCamera();
        playerPerson.Dance();

        CreateCircleOfPersons();

        GameController.instance.ShowLevelCompleted();
    }

    void CreateCircleOfPersons()
    {
        PersonController person;

        int groupLevel = 1;
        
        int numPeople = personsList.Count;

        float radius = groupLevel * 2f;
        float angle;
        Vector3 newPos;

        for (int i = 0; i < numPeople; i++)
        {
            person = personsList[i];

            angle = i * Mathf.PI * 2f / numPeople;
            newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            person.MoveToPositition(newPos, playerData.speedMovePersonToPositionEnd);
            person.LookAtCamera();
            person.Dance();

        }
    }

}
