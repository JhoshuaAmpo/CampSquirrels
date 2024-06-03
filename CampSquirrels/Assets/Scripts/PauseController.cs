using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenuOverlay;

    PlayerActions playerActions;
    private bool isPaused = false;
    float prevTimeScale = 0f;
    private void Awake() {
        playerActions = new();
        playerActions.Interact.Enable();
        playerActions.Interact.TogglePauseMenu.performed += ProcessPause;
    }

    private void ProcessPause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        if (isPaused) {
            Pause();
        } else {
            Resume();
        }
        
    }

    public void Pause()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuOverlay.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = prevTimeScale;
        AudioListener.pause = false;
        pauseMenuOverlay.SetActive(false);
    }
}
