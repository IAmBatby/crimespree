using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : Device
{
    public enum DoorToggle { Opened, Closed }
    public DoorToggle doorToggleState = DoorToggle.Closed;
    public enum LockToggle { Unlocked, Locked }
    public LockToggle lockToggleState = LockToggle.Locked;
    public GameObject doorObject;
    public List<TextMeshProUGUI> nameTextList = new List<TextMeshProUGUI>();
    public Equipment equipmentRequirement;
    PlayerController playerController;
    void Start()
    {
        playerController = GameManager.Instance.playerController;
        if (equipmentRequirement != null)
            foreach (TextMeshProUGUI text in nameTextList)
                text.text = "Requirement: " + equipmentRequirement.name;
    }
    public override void Interact(PlayerController player = null, Loot loot = null)
    {
        switch (lockToggleState)
        {
            case LockToggle.Unlocked:
                switch (doorToggleState)
                {
                    case DoorToggle.Opened:
                        doorToggleState = DoorToggle.Closed;
                        break;
                    case DoorToggle.Closed:
                        doorToggleState = DoorToggle.Opened;
                        break;
                }
                if (doorToggleState == DoorToggle.Closed)
                    doorObject.SetActive(true);

                else if (doorToggleState == DoorToggle.Opened)
                    doorObject.SetActive(false);
                break;
            case LockToggle.Locked:
                if (player != null)
                {
                    foreach (Equipment equipment in playerController.equipmentInventory)
                    {
                        if (equipment == equipmentRequirement)
                        {
                            foreach (TextMeshProUGUI text in nameTextList)
                            {
                                text.gameObject.SetActive(false);
                            }
                            lockToggleState = LockToggle.Unlocked;
                        }
                    }
                }
                break;
        }
    }
}
