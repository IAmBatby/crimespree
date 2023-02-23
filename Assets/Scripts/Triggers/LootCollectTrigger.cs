using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollectTrigger : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trying To Collect Loot!" + other.name);
        Transform loot = other.transform.parent;
        if (loot.GetComponent<Loot>() != null)
        {
            loot.gameObject.SetActive(false);
            gameManager.AddCurrency(loot.GetComponent<Loot>());
            loot.transform.parent = gameManager.collectedLootObjects.transform;
            loot.transform.position = Vector3.zero;
        }
    }
}
