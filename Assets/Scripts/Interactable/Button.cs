using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    public enum ButtonToggle {Deactivated, Activated, Locked}
    public ButtonToggle buttonToggleState = ButtonToggle.Locked;
    public List<Device> connectedDevices = new List<Device>();
    GameObject buttonTriggerCollider;

    void Start()
    {
        buttonTriggerCollider = transform.GetChild(0).gameObject;
    }
    public void Interact(PlayerController player = null, Loot loot = null)
    {
        foreach (Device device in connectedDevices)
        {
            device.Interact(player, loot);
        }
    }
}
