using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [Title("Properties")]
    public float interactionTime;
    public string interactionTerm;
    public Pickup requiredPickup;
    //public AudioClip soundEffect;
    [Space(15)]
    [Title("Events")]
    public bool hasFailureEvent;
    public UnityEvent interactionSuccessEvent;
    [ShowIf("hasFailureEvent")]
    public UnityEvent interactionFailureEvent;

    [Title("Debug Values")]
    [ReadOnly] [ProgressBar(0,"interactionTime", DrawValueLabel = false)] public float interactionProgressTime;
    //[ReadOnly] public bool hasInteractionFailed = false;
    [ReadOnly] public bool isPlayerInRange;
    [ReadOnly] public bool isPlayerLooking;
    [ReadOnly] public Coroutine interactCoroutine;
    [ReadOnly] [SerializeField] bool interactCoroutineIsNull;
    [FoldoutGroup("References")] public GameObject interactionIcon;
    [FoldoutGroup("References")] public Collider PlayerRangeCollider;
    [FoldoutGroup("References")] public Collider interactionCollider;
    [FoldoutGroup("References")] public GameObject interactionObject;
    [FoldoutGroup("References")] public TextMeshProUGUI interactionText;
    [FoldoutGroup("References")] public Image radialBarImage;

    float interactStartTime;


    void Start()
    {
        interactionIcon.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractUIUpdate(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractUIUpdate(false);
    }

    void InteractUIUpdate(bool playerStatus)
    {
        if (playerStatus == true)
        {
            isPlayerInRange = true;
            interactionIcon.SetActive(true);
            //interactionText.gameObject.SetActive(true);
        }
        else
        {
            isPlayerInRange = false;
            interactionIcon.SetActive(false);
            //interactionText.gameObject.SetActive(false);
        }
    }

    public void StartInteract(RaycastHit myHit)
    {
        if (interactCoroutine == null)
        {
            GameManager.Instance.playerController.currentInteraction = this;
            interactCoroutine = StartCoroutine(Interact());
        }
        else
            Debug.Log("Interaction Failure! Tried To Start Ineraction While Already Being Interacted With!");
    }

    public void StopInteract()
    {
        StopCoroutine(interactCoroutine);
        interactCoroutine = null;
        interactionProgressTime = 0;
        GameManager.Instance.playerController.currentInteraction = null;
        if (interactionFailureEvent != null && hasFailureEvent == true)
            interactionFailureEvent.Invoke();
    }

    public IEnumerator Interact()
    {
        Debug.Log("Starting Interact");
        interactStartTime = Time.time;
        yield return new WaitForSeconds(interactionTime);

        Debug.Log("Coroutine Finished");
        GameManager.Instance.playerController.currentInteraction = null;
        interactCoroutine = null;
        interactStartTime = 0;
        if (interactionSuccessEvent != null)
            interactionSuccessEvent.Invoke();
    }

    void Update()
    {
        isPlayerLooking = GameManager.Instance.playerController.hasLookedThisFrame;
        if(interactCoroutine != null)
        {
            interactionProgressTime = interactionTime + (interactStartTime - Time.time);
            interactCoroutineIsNull = true;
        }
        else
        {
            interactCoroutineIsNull = false;
        }
        //interactionText.transform.position = GameManager.Instance.playerController.transform.position;
        interactionText.transform.rotation = Quaternion.LookRotation(transform.position - GameManager.Instance.playerController.transform.position);

        if (isPlayerLooking == true && isPlayerInRange == true)
        {
            interactionText.gameObject.SetActive(true);
            if (interactCoroutine != null)
            {
                interactionText.text = interactionProgressTime.ToString("0.#");
                radialBarImage.gameObject.SetActive(true);
                radialBarImage.fillAmount = 1f - (interactionProgressTime / interactionTime);
            }
            else
            {
                radialBarImage.gameObject.SetActive(false);
                interactionText.text = "Hold [" + GameManager.Instance.playerController.interactionKey.ToString() + "] " + interactionTerm;
            }    
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }


    public void Succeeded()
    {
        Debug.Log("Has Succeed!");
        gameObject.SetActive(false);
    }

    public void Failed()
    {
        Debug.Log("Has Failed!");
    }
}
