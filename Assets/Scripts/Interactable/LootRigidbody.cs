using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootRigidbody : Loot
{

    public override void Start()
    {
        base.Start();
        interaction.interactionTerm = GameManager.Instance.playerController.interactionKey.ToString() + "To Grab The " + displayName;
        Physics.IgnoreCollision(GetComponent<Collider>(), GameManager.Instance.playerController.characterController);
        interaction.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            //transform.GetComponent<Rigidbody>().drag = 5;
            transform.GetComponent<Rigidbody>().mass = 500;
            interaction.gameObject.SetActive(true);
        }
    }
}
