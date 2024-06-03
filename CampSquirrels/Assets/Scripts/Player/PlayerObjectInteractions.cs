using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectInteractions : MonoBehaviour
{
    [SerializeField]
    private float slapCooldown;
    [SerializeField]
    private AudioClip AddLogSFX;
    [SerializeField]
    private AudioClip DropLogSFX;

    public event Action<int> OnChangeOfFuelCount;
    public int FuelCount {get; private set;} = 0;
    
    AudioSource audioSource;
    PlayerActions playerActions;
    List<GameObject> squirrelsInStrikeRange;
    private float slapTimer = 0;

    private void Awake() {
        playerActions = new();
        playerActions.Interact.Enable();
        playerActions.Interact.TorchSlap.performed += ProcessAttack;
        squirrelsInStrikeRange = new();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        slapTimer -= Time.deltaTime;
        Mathf.Clamp(slapTimer, 0f, slapCooldown);
    }
    private void OnTriggerEnter(Collider other) {
        // DisplayInteraction(); 
        // Debug.Log(other.name + "has entered my box");
        if (other.gameObject.CompareTag("Squirrel")) {
            squirrelsInStrikeRange.Add(other.gameObject);
            Debug.Log("Squirrel entered my box!");
        }
        ProcessInteraction(other);
    }

    private void OnTriggerStay(Collider other) {
        // DisplayInteraction();
        // Debug.Log(other.name);
        ProcessInteraction(other);
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Squirrel")) {
            squirrelsInStrikeRange.Remove(other.gameObject);
            Debug.Log("Squirrel exited my box!");
        }
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
        else if (other.TryGetComponent<CampfireController>(out var campfireController) && Vector3.Distance(other.transform.position, this.transform.position) < 5f) {
            DepositWood(campfireController);
        }
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        if (slapTimer <= 0f) { return; }
        slapTimer = slapCooldown;
        // Play slap animation
        if(squirrelsInStrikeRange.Count <= 0) { 
            // PlaySFX(/*WhooshSFX*/);
            return;
        }
        foreach(var squirrel in squirrelsInStrikeRange) {
            squirrel.SetActive(false);
        }
        // PlaySFX(/*SlapSFX*/);
        // Debug.Log("Slapped " + squirrelsInStrikeRange.Count + " squirrels!");
        squirrelsInStrikeRange.Clear();
    }

    private void PickUpWood(Collider other) {
        FuelCount++;
        other.gameObject.SetActive(false);
        OnChangeOfFuelCount.Invoke(FuelCount);
        PlaySFX(AddLogSFX);
    }

    private void DepositWood(CampfireController cfc) {
        if (FuelCount <= 0) { return; }
        StartCoroutine(ProcessDepositWoodSFX());
        cfc.IncreaseRemainingFuel(FuelCount);
        FuelCount = 0;
        OnChangeOfFuelCount.Invoke(FuelCount);
    }

    private IEnumerator ProcessDepositWoodSFX()
    {
        int max = FuelCount;
        for(int i = 0; i < max; i++) {
            PlaySFX(DropLogSFX);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void PlaySFX(AudioClip ac) {
        if(audioSource.isPlaying) {audioSource.Stop();}
        audioSource.PlayOneShot(ac);
    }
}
