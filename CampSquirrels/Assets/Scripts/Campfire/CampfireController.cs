using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class CampfireController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount of fuel the fire has currently")]
    private int remainingFuel = 0;
    [SerializeField]
    [Tooltip("Number of seconds 1 fuel increases the lifespan by")]
    private float fuelDuration;
    [SerializeField]
    private GameObject fire;

    [Tooltip("Total time before the fire goes out in seconds")]
    public float Lifespan {get; private set;}
    bool isExtinguished = false;
    // Timer for burning 1 fuel
    private float burnTimer;
    private ParticleSystem fireParticles;
    enum Type {}
    GameObject player;
    PlayerHealth playerHealth;

    // Func increaseRemainingFuel
    // Func decrease 
    private void Awake() {
        burnTimer = fuelDuration;
        Lifespan = remainingFuel * fuelDuration;
        fireParticles = GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update() {
        UpdateBurnTimer();
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log(other.name + " has entered the campfire");
        if (other.CompareTag("Player")) {
            playerHealth.IsInCampFireRange(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        // Debug.Log(other.name + " has left the campfire");
        if (other.CompareTag("Player")) {
            playerHealth.IsInCampFireRange(false);
        }
    }

    public void IncreaseRemainingFuel(int numOfFuel) {
        if (isExtinguished) { return; }
        remainingFuel += numOfFuel;
        Lifespan += numOfFuel * fuelDuration;
    }

    private void UpdateBurnTimer() {
        if (burnTimer <= 0) {
            if (remainingFuel > 0) {
                burnTimer = fuelDuration;
                remainingFuel--;
            } else {
                ExtinguishFlame();
            }
        }
        burnTimer -= Time.deltaTime;
        Lifespan = remainingFuel * fuelDuration + burnTimer;
    }

    private void ExtinguishFlame() {
        isExtinguished = true;
        fire.SetActive(false);
        fireParticles.gameObject.SetActive(false);
        // Debug.Log("Fire has gone out!");
    }
}
