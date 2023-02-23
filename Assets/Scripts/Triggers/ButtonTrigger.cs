using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = transform.parent.GetComponent<Button>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            button.Interact(loot:other.GetComponent<Loot>());
    }
}
