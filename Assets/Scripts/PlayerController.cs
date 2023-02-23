using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Material selectionYellow;
    public TextMeshProUGUI carriedItemText;
    public Transform carriedObjectPosition; //REPLACABLE.
    public Animator carryingUIAnimator;
    public TextMeshProUGUI carryingUIText;
    public GameObject carriedItemTextObject;
    public float groundDistance = 0.4f;
    public Transform bonkCheck;

    public enum JumpStates { Standing, Jumping, Falling }
    [Header("Jumping")]
    public JumpStates jumpStatesToggle = JumpStates.Standing;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    public enum RunStates { Walking, Gaining, Running }
    [Header("Running")]
    public RunStates runStatesToggle = RunStates.Walking;
    public float playerMovementSpeed;
    public float walkSpeed = 0f;
    public float runSpeed = 0f;
    public float camTiltOffset = 0f;

    public enum CrouchStates { Standing, Crouched, Crouching, Rising }
    [Header("Crouching")]
    public CrouchStates crouchStatesToggle = CrouchStates.Standing;
    public float standHeight = 3.8f;
    public float crouchHeight = 0.2f;
    public float crouchSpeed;
    public float cameraOffset;

    [Header("Item Interaction")]
    public List<Equipment> equipmentInventory = new List<Equipment>();
    public KeyCode interactionKey;
    public LayerMask interactionMask;
    public GameObject carriedObject;
    public LayerMask selectionMask;
    public float selectionDistance;
    public float throwSpeed;
    public Interaction currentInteraction;
    [HideInInspector] public bool hasLookedThisFrame;

    GameManager gameManager;
    float velocityY;
    [ReadOnly] public float jumpHeightModifier;
    [ReadOnly] public float playerSpeedModifier;
    float startTime = 0;
    bool progressBool;
    public bool interactionCheck;
    public GameObject bagPrefab;
    Camera camera;
    Material tempMaterial;
    GameObject selectionObject;
    Vector3 characterControllerBottomPosition;
    GroundCheckTrigger groundCheckTrigger;

    void Start()
    {
        camera = Camera.main;
        groundCheckTrigger = groundCheck.GetComponent<GroundCheckTrigger>();
        gameManager = GameManager.Instance;
        carryingUIAnimator.SetTrigger("Exit");
    }

    void Update()
    {
        Movement();
        Selection();
        CrouchToggle();
        RunToggle();
        PlayerStatAdjust();
        if (progressBool == true)
        {
            InteractionTimeProgress();
        }

        PositionAnchor(groundCheck, 1.5f, false);
        PositionAnchor(camera.transform, 5.5f, true);
        PositionAnchor(bonkCheck, 2, true);

        if (interactionCheck == true)
        {
            if (Input.GetKeyUp(interactionKey))
            {
                interactionCheck = false;
            }
        }
    }

    void PositionAnchor(Transform otherTransform, float offset, bool positive)
    {
        if (positive == true)
            if ((characterController.height / offset) > characterController.radius)
                otherTransform.position = new Vector3(transform.position.x, transform.position.y + (characterController.height / offset), transform.position.z);
            else
                otherTransform.position = new Vector3(transform.position.x, transform.position.y + characterController.radius, transform.position.z);

        else
            if ((characterController.height / offset) > characterController.radius)
                otherTransform.position = new Vector3(transform.position.x, transform.position.y - (characterController.height / offset), transform.position.z);
            else
                otherTransform.position = new Vector3(transform.position.x, transform.position.y - characterController.radius, transform.position.z);
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        switch (jumpStatesToggle)
        {
            case JumpStates.Standing:
                if (velocityY < 0)
                    velocityY = -2f;
                if (Input.GetButtonDown("Jump"))
                {
                    velocityY = Mathf.Sqrt((jumpHeight - jumpHeightModifier) * -2f * gravity);
                    jumpStatesToggle = JumpStates.Jumping;
                }
                break;
        }

        velocityY += gravity * Time.deltaTime;
        Vector3 move = (transform.right * x * playerMovementSpeed) + (transform.forward * z * playerMovementSpeed) + (transform.up * velocityY);
        Vector3.ClampMagnitude(move, playerMovementSpeed);
        characterController.Move(move * Time.deltaTime);

    }

    void Selection()
    {
        /*if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, selectionMask))
        {
            Interactable interactableCache = null;
            //Selection Code.
            if (Vector3.Distance(camera.transform.position, hit.point) < selectionDistance && hit.transform.GetComponent<Interactable>() != null)
            {
                interactableCache = hit.transform.GetComponent<Interactable>();
                if (selectionObject != null)
                    selectionObject.GetComponent<MeshRenderer>().material = tempMaterial;
                //Text Stuff.
                selectionObject = hit.transform.gameObject;
                tempMaterial = selectionObject.GetComponent<MeshRenderer>().material;
                selectionObject.GetComponent<MeshRenderer>().material = selectionYellow;

                if (interactableCache is Pickup pickup)
                {
                    gameManager.pickupText.transform.gameObject.SetActive(true);
                    gameManager.pickupText.transform.position = camera.WorldToScreenPoint((hit.point + camera.transform.forward) + new Vector3(0, 0.5f, 0));
                    pickup.SetName();
                    pickup.displayText.text = pickup.displayName;
                    gameManager.pickupText.text = pickup.displayName;
                }
            }
            else if (selectionObject != null)
            {
                selectionObject.GetComponent<MeshRenderer>().material = tempMaterial;
                gameManager.pickupText.transform.gameObject.SetActive(false);
                selectionObject = null;
            }

            //Interaction code.
            if (Input.GetKeyDown(interactionKey))
            {
                if (carriedObject != null)
                    Throw();
            }
        }*/
        hasLookedThisFrame = false;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, interactionMask, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Hit Interaction!, Name is " + hit.collider.transform.name, hit.collider.gameObject);
            hasLookedThisFrame = true;
            if (hasLookedThisFrame == true)
            {
                //Debug.Log("Yayay" + hit.transform.name);
                if (hit.collider.transform.CompareTag("Interaction"))
                {
                    //This is hardcoded I suppose I hate it I really do
                    Interaction interaction = hit.collider.transform.parent.GetComponent<Interaction>();
                    if (interaction.isPlayerInRange == true && interaction.isPlayerLooking == true)
                    {
                        //Debug.Log("Hit Interaction That's In Range!");
                        Vector3 TempVector = interaction.interactionText.transform.position = (hit.point + (transform.position - hit.point) / 2);
                        interaction.interactionText.transform.position = new Vector3(TempVector.x, TempVector.y + 0.3f, TempVector.z);
                        if (Input.GetKey(interactionKey) && currentInteraction == null)
                        {
                            interaction.StartInteract(hit);
                        }
                    }
                }
            }
            //Edgecase to prevent being able to look from one interaction to another and it not fail. Disabled for now.
            //else if(currentInteraction != hit.transform.parent.GetComponent<Interaction>())
                //currentInteraction.StopInteract();
        }
        //Because if that raycast fails, it means your not looking at an interact anymore which 99% of the time is the thing your currently interacting with.
        else if (currentInteraction != null)
            currentInteraction.StopInteract();

        if (currentInteraction != null & Input.GetKeyUp(interactionKey))
            currentInteraction.StopInteract();

        if (Input.GetKeyDown(interactionKey))
        {
            if (carriedObject != null)
                Throw();
        }
    }
    void InteractionTimeProgress()
    {
        Debug.Log(startTime - Time.time);
    }
    IEnumerator InteractionTime(Interactable interactableCache)
    {
        float startTime = interactableCache.interactionTime;
        Debug.Log("Coroutine Started");
        progressBool = true;
        yield return new WaitForSeconds(interactableCache.interactionTime);
        Debug.Log("Coroutine Ended");
        progressBool = false;
        if (interactionCheck == true)
        {
            if (interactableCache is Loot loot)
            {
                if (carriedObject != null)
                    Throw();
                else
                    Pickup(selectionObject, loot);
            }

            else if (interactableCache is Equipment equipment)
                Collect(selectionObject, equipment);

            else if (interactableCache is Button button)
                button.Interact(gameManager.playerController);
        }
    }

    public void Pickup(GameObject pickupObject, Loot loot)
    {
        GameObject baggedObject = Instantiate(bagPrefab, pickupObject.transform.position, pickupObject.transform.rotation);
        Loot bagLoot = baggedObject.GetComponent<Loot>();
        bagLoot.displayName = loot.displayName;
        bagLoot.weight = loot.weight;
        bagLoot.worth = loot.worth;
        loot = bagLoot;
        Destroy(pickupObject);
        pickupObject = baggedObject;
        loot.isBagged = true;
        carriedObject = pickupObject;
        PlayerStatAdjust(loot.weight, loot.weight);
        pickupObject.SetActive(false);
        carryingUIAnimator.SetTrigger("Enter");
        carryingUIText.text = loot.displayName;
        camTiltOffset = -5f;
    }

    void Throw()
    {
        Loot loot = carriedObject.GetComponent<Loot>();
        PlayerStatAdjust(-loot.weight, -(loot.weight));
        carriedObject.transform.position = carriedObjectPosition.position;
        carriedObject.SetActive(true);
        carriedObject.transform.rotation = Quaternion.identity;
        carriedObject.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwSpeed);
        carriedObject = null;
        camTiltOffset = 0f;
        carryingUIAnimator.SetTrigger("Exit");
    }

    void Collect(GameObject pickupItem, Equipment equipment)
    {
        pickupItem.SetActive(false);
        equipmentInventory.Add(equipment);
    }

    void CrouchToggle()
    {
        switch (crouchStatesToggle)
        {
            case CrouchStates.Standing:
                characterController.height = standHeight;
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    crouchStatesToggle = CrouchStates.Crouching;
                break;

            case CrouchStates.Crouched:
                characterController.height = crouchHeight;
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    crouchStatesToggle = CrouchStates.Rising;
                break;

            case CrouchStates.Crouching:
                characterController.height = Mathf.Lerp(characterController.height, crouchHeight, crouchSpeed);
                if (Mathf.Abs(characterController.height - crouchHeight) <= 0.2f)
                    crouchStatesToggle = CrouchStates.Crouched;
                break;

            case CrouchStates.Rising:
                characterController.height = Mathf.Lerp(characterController.height, standHeight, crouchSpeed);
                if (Mathf.Abs(characterController.height - standHeight) <= 0.2f)
                crouchStatesToggle = CrouchStates.Standing;
                break;
        }
    }

    void RunToggle()
    {
        switch (runStatesToggle)
        {
            case RunStates.Walking:
                if (Input.GetKey(KeyCode.LeftShift))
                    runStatesToggle = RunStates.Gaining;
                playerMovementSpeed = walkSpeed - playerSpeedModifier;
                break;
            case RunStates.Gaining:
                if (Input.GetKeyUp(KeyCode.LeftShift))
                    runStatesToggle = RunStates.Walking;
                playerMovementSpeed = Mathf.Lerp(playerMovementSpeed, runSpeed - playerSpeedModifier, 0.1f);
                if (playerMovementSpeed == runSpeed - playerSpeedModifier)
                    runStatesToggle = RunStates.Running;
                break;
            case RunStates.Running:
                if (Input.GetKeyUp(KeyCode.LeftShift))
                    runStatesToggle = RunStates.Walking;
                playerMovementSpeed = runSpeed - playerSpeedModifier;
                break;
        }
    }

    void PlayerStatAdjust(float playerSpeedModifierValue = 0, float jumpHeightModifierValue = 0)
    {
        jumpHeightModifier += jumpHeightModifierValue;
        playerSpeedModifier += playerSpeedModifierValue;
    }
}