using System;
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
    [SerializeField]
    private AudioClip squirrelDamageSFX;
    
    public bool InCampfireRange { get; set; } = true;
    
    private float currentHP;
    public event Action<float, float> OnHealthChange;
    private AudioSource audioSource;
    private Animator animator;

    private void Awake() {
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (!InCampfireRange) {
            // Debug.Log("Ouch taking cold damage!");
            ChangeCurrentHP(-coldDamage * Time.deltaTime);
        } else if(currentHP < maxHP) {
            ChangeCurrentHP(warmthRegen * Time.deltaTime);
        }
    }

    public void ChangeCurrentHP(float delta){
        currentHP += delta;
        if(delta < -coldDamage) {
            if(audioSource.isPlaying) {audioSource.Stop();}
            audioSource.PlayOneShot(squirrelDamageSFX);
        }
        Mathf.Clamp(currentHP, 0, maxHP);
        OnHealthChange?.Invoke(currentHP, maxHP);
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
        animator.SetBool("Death", true);
        // Debug.Log("Player has died!");
    }
}
