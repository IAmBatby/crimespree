using UnityEngine;

public class GroundCheckTrigger : MonoBehaviour
{
    [SerializeField]
    string[] TagBlacklist = {"NotWalkable"};
    GameManager gameManager;
    GameObject playerGameObject;
    PlayerController playerController;
    public int count;
    public bool collidedthisturn;

    void Start()
    {
        gameManager = GameManager.Instance;
        playerController = gameManager.playerController;
    }

    private void FixedUpdate()
    {
        count = 0;
    }

    void OnTriggerStay(Collider other)
    {
        count++;
        if (other.gameObject != playerGameObject)
            foreach (string blacklistTag in TagBlacklist)
                if (!other.CompareTag(blacklistTag))
                    playerController.jumpStatesToggle = PlayerController.JumpStates.Standing;
    }

    void OnTriggerExit(Collider other)
    {
        count--;
    }

    void Update()
    {
        if (count == 0)
            playerController.jumpStatesToggle = PlayerController.JumpStates.Jumping;
    }
}
