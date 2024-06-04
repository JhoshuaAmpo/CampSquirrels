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
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private GameObject deathHUD;
    [SerializeField]
    private PauseController pauseController;
    
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
            StartCoroutine(ProcessDeath());
        }
    }

    public void RestoreToFull() {
        ChangeCurrentHP(maxHP);
    }

    public void IsInCampFireRange(bool b) {
        InCampfireRange = b;
    }

    private IEnumerator ProcessDeath() {
        if(audioSource.isPlaying) {audioSource.Stop();}
        audioSource.PlayOneShot(deathSound);
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(3);
        pauseController.Pause(false);
        AudioListener.pause = false;
        deathHUD.SetActive(true);
    }
}
