using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 100;
    [SerializeField]
    [Tooltip("Damage Per Second")]
    private float coldDamage;
    [SerializeField]
    [Tooltip("Health gain per second insdie the fire")]
    private float warmthRegen;

    private float currentHP;

    public bool InCampfireRange { get; set; } = true;

    private void Awake() {
        currentHP = maxHP;
    }

    private void Update() {
        if (!InCampfireRange) {
            // Debug.Log("Ouch taking cold damage!");
            ChangeCurrentHP(-coldDamage * Time.deltaTime);
        } else {
            ChangeCurrentHP(warmthRegen * Time.deltaTime);
        }
    }

    public void ChangeCurrentHP(float delta){
        currentHP += delta;
        Mathf.Clamp(currentHP, 0, maxHP);
        if (currentHP < 0){
            ProcessDeath();
        }
    }

    public void RestoreToFull() {
        ChangeCurrentHP(maxHP);
    }

    public void IsInCampFireRange(bool b) {
        InCampfireRange = b;
    }

    private void ProcessDeath() {
        // Debug.Log("Player has died!");
    }
}
