using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectInteractions : MonoBehaviour
{
    public event Action<int> OnChangeOfFuelCount;
    public int FuelCount {get; private set;} = 0;
    PlayerActions playerActions;
    private void Awake() {
        playerActions = new();
        playerActions.Interact.Enable();
    }

    private void OnTriggerEnter(Collider other) {
        // DisplayInteraction(); 
        // Debug.Log(other.name + "has entered my box");
        ProcessInteraction(other);
    }

    private void OnTriggerStay(Collider other) {
        // DisplayInteraction();
        // Debug.Log(other.name);
        ProcessInteraction(other);
    }

    private void DisplayInteraction() {
        // Display text over the item that says "press Left Click to pickup item"
        throw new System.NotImplementedException();
    }

    private void ProcessInteraction(Collider other) {
        if (!playerActions.Interact.PickUpItem.IsPressed()) { return; }
        if (other.TryGetComponent<Pickupables>(out var pickCom)) {
            Pickupables.ItemType itemType = pickCom.GetItemType();
            switch (itemType) {
            case Pickupables.ItemType.wood:
                PickUpWood(other);
                break;
            case Pickupables.ItemType.weapon:
                break;
            case Pickupables.ItemType.lantern:
                break;
            case Pickupables.ItemType.powerup:
                break;
            default:
                Debug.LogError(other.name + " has no ItemType");
                break;
            }
        }
        if (other.TryGetComponent<CampfireController>(out var campfireController)) {
            DepositWood(campfireController);
        }
    }

    private void PickUpWood(Collider other) {
        FuelCount++;
        other.gameObject.SetActive(false);
        OnChangeOfFuelCount.Invoke(FuelCount);
        // Debug.Log("Picked up wood");
    }
    private void DepositWood(CampfireController cfc) {
        if (FuelCount <= 0) { return; }
        cfc.IncreaseRemainingFuel(1);
        FuelCount--;
        OnChangeOfFuelCount.Invoke(FuelCount);
        // Debug.Log("Depositied wood");
    }
}
