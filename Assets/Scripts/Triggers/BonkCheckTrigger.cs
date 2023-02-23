using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkCheckTrigger : MonoBehaviour
{
    PlayerController playerController;

    void Start()
    {
        playerController = GameManager.Instance.playerController;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject != playerController.gameObject)
            if (playerController.crouchStatesToggle == PlayerController.CrouchStates.Rising || playerController.crouchStatesToggle == PlayerController.CrouchStates.Crouched)
                playerController.crouchStatesToggle = PlayerController.CrouchStates.Crouched;
    }
}
