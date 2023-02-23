using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loot : Pickup
{
    public int worth;
    public int weight = 0;
    public bool isBagged;
    public string displayName;
    public Interaction interaction;
    
    public virtual void Start()
    {
        interaction.interactionTerm = "To Take The " + displayName;
    }


    public void PlayerPickup()
    {
        GameManager.Instance.playerController.Pickup(this.gameObject,this);
    }

    public void Update()
    {
        //if (Physics.Raycast(transform.position, transform.position + new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), out RaycastHit hit, 0.5f))
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
