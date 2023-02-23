using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public GameObject playerGameObject;
    public float interactionTime;

    public void Start()
    {
        gameManager = GameManager.Instance;
        playerController = gameManager.playerController;
        playerGameObject = gameManager.playerGameObject;
    }
}
