using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameManager : SerializedMonoBehaviour
{
    //Singleton
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    public int currency = 00000;
    public GameObject collectedLootObjects;
    public GameObject playerGameObject;
    public PlayerController playerController;

    [FoldoutGroup("Text")]
    public TextMeshProUGUI currencyText;
    [FoldoutGroup("Sprites")]
    public Sprite crosshair;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI carryingText;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI carryingObjectText;
    public GameObject debugMenuObject;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI jumpStates;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI runStates;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI pickupText;
    [FoldoutGroup("Text")]
    public Animator currencyUIAnimator;
    [FoldoutGroup("Text")]
    public TextMeshProUGUI crouchStates;
    public enum DebugStates { Inactive, Active }
    public DebugStates debugStatesToggle = DebugStates.Inactive;
    Scene currentScene;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        currencyText.text = "$" + currency;
        jumpStates.text = "JumpState: " + playerController.jumpStatesToggle.ToString();
        runStates.text = "RunState: " + playerController.runStatesToggle.ToString();
        crouchStates.text = "CrouchState: " + playerController.crouchStatesToggle.ToString();

        switch (debugStatesToggle)
        {
            case DebugStates.Inactive:
                debugMenuObject.SetActive(false);
                if (Input.GetKeyDown(KeyCode.B))
                    debugStatesToggle = DebugStates.Active;
                break;
            case DebugStates.Active:
                debugMenuObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.B))
                    debugStatesToggle = DebugStates.Inactive;
                if (Input.GetKeyDown(KeyCode.T))
                    Debug.Break();
                if (Input.GetKeyDown(KeyCode.R))
                    SceneManager.LoadScene(currentScene.name);
                break;
        }
    }

    public void AddCurrency(Loot loot)
    {
        currency += loot.worth;
        currencyUIAnimator.SetTrigger("Animate");
    }
}
