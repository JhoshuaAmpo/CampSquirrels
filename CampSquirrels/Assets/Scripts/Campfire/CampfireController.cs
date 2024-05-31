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

    // Timer for burning of 1 fuel
    private float burnTimer;

    enum Type {}

    // Func increaseRemainingFuel
    // Func decrease 
    private void Awake() {
        burnTimer = fuelDuration;
        Lifespan = remainingFuel * fuelDuration;
    }

    private void Update() {
        UpdateBurnTimer();
    }

    public void IncreaseRemainingFuel(int numOfFuel) {
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
        // Debug.Log("Fire has gone out!");
    }
}
