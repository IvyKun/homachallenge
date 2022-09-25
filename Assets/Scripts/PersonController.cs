using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Behaviour of a person
/// </summary>
public class PersonController : MonoBehaviour
{
    Animator animatorComponent;
    SkinnedMeshRenderer skinnedMeshRenderer;
    CapsuleCollider capsuleCollider;

    // Ground check
    LayerMask groundLayer;
    float sphereCheckRadius = 0.2f;
    bool isGrounded;
    Vector3 sphereCheckOffset;

    // Movement
    float gravity;
    Vector3 movement;
  
    // People group or player
    public bool isPlayer;
    public bool isInPlayerGroup;

    float deactivateOffset;

    PlayerController playerController;

    //VFX
    List<GameObject> vfxDeath = new List<GameObject>();
    List<GameObject> vfxHappy = new List<GameObject>();

    void Awake()
    {
        animatorComponent = this.GetComponent<Animator>();
        skinnedMeshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();

        playerController = PlayerController.instance; // I put it here in case we don't want to use singletons
    }

    void Start()
    {
        // Get some values from the player controller
        groundLayer = GameController.instance.configData.groundLayer;
        sphereCheckOffset = new Vector3(0, sphereCheckRadius / 2, 0);

        gravity = playerController.playerData.gravity;
        deactivateOffset = playerController.playerData.personDeactivateOffset;

        // Set the material to inactive
        if (isPlayer)
        {
            skinnedMeshRenderer.material = GameController.instance.configData.materialPersonActive;
            animatorComponent.runtimeAnimatorController = GameController.instance.configData.animationControllerPlayer;
        }
        else 
        {
            skinnedMeshRenderer.material = GameController.instance.configData.materialPersonInactive;
            animatorComponent.runtimeAnimatorController = GameController.instance.configData.animationControllerPerson;
        }

    }

    void Update()
    {
        if (GameController.instance.gameState == GameStateEnum.Started)
        {
            GroundCheck();
            Move();

            // Deactivate if player already passed this obstacle
            if (Vector3.Distance(transform.position, playerController.transform.position) > deactivateOffset && transform.position.z < playerController.transform.position.z && isInPlayerGroup == false )
            {
                this.gameObject.SetActive(false);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            CrashWithObstacle();
        }
        else if (other.CompareTag("Water"))
        {
            FallIntoWater();
        }
        else if (other.CompareTag("Person") && (isPlayer || isInPlayerGroup))
        {
            playerController.AddPerson(other.GetComponent<PersonController>());
        }
        else if (other.CompareTag("Finish") && isPlayer)
        {
            playerController.FinishRun();
        }

    }

    /// <summary>
    /// Movement of a person. It only manages vertical movement to be able to fall into the water or jump.
    /// The forward movement is handled by the player controller that it's the parent.
    /// </summary>
    void Move()
    {
        // Apply gravity
        if (isGrounded)
        {
            movement.y = 0;
        }
        else
        {
            movement.y += gravity * Time.deltaTime;
        }

        // Movement
        transform.Translate(movement * Time.deltaTime);
    }

    /// <summary>
    /// Set the moving animation
    /// </summary>
    public void StartMoving()
    {
        animatorComponent.SetTrigger("Move");
    }

    /// <summary>
    /// Do some basic configuration when a person controller is added to the player controller
    /// </summary>
    public void Added()
    {
        skinnedMeshRenderer.material = GameController.instance.configData.materialPersonActive;

        if(isPlayer == false) 
        {
            StartMoving();

            StartCoroutine(RotateToFrontCoroutine());
        }

    }

    /// <summary>
    /// Set the victory animation
    /// </summary>
    public void Dance()
    {
        animatorComponent.SetTrigger("Victory");
    }

    /// <summary>
    /// Called when a person falls into the water. 
    /// If it's the player, the game ends. If it's a person, that person is removed from the player.
    /// </summary>
    void FallIntoWater()
    {
        capsuleCollider.enabled = false;
        //isInPlayerGroup = false;
        //transform.parent = null;

        if (isPlayer)
        {
            GameController.instance.SetGameOver();
        }
        else 
        {
            PlayerController.instance.LosePerson(this);
        }
        
    }

    /// <summary>
    /// Called when a person crash with an obstacle
    /// If it's the player, the game ends. If it's a person, that person is removed from the player.
    /// </summary>
    void CrashWithObstacle()
    {
        capsuleCollider.enabled = false;
        //isInPlayerGroup = false;
        //transform.parent = null;

        animatorComponent.SetTrigger("Crash");

        if (isPlayer)
        {
            GameController.instance.SetGameOver();
        }
        else
        {
            PlayerController.instance.LosePerson(this);
        }
    }

    /// <summary>
    /// Moves the person to a desired position. Used to get the person to the position behind the player or at the level end.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="moveSpeed"></param>
    public void MoveToPositition(Vector3 targetPosition, float moveSpeed) 
    {
        StartCoroutine(MoveToPositionCoroutine(targetPosition, moveSpeed));
    }

    IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, float moveSpeed)
    {
        float speed = moveSpeed;
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = targetPosition;
    }

    IEnumerator RotateToFrontCoroutine()
    {
        float duration = playerController.playerData.timeMovePersonRotateToFront;

        Quaternion newRotation = Quaternion.Euler(Vector3.zero);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, counter / duration);
            yield return null;
        }
    }

    public void LookAtCamera() 
    {
        StartCoroutine(RotateToBackCoroutine());
    }

    IEnumerator RotateToBackCoroutine()
    {
        float duration = playerController.playerData.timeMovePersonRotateToFront;

        Quaternion newRotation = Quaternion.Euler(0, 180, 0);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, counter / duration);
            yield return null;
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position + sphereCheckOffset, sphereCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

}
