using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0;
    PlayerActions playerActions;
    CharacterController characterController;

    void Awake() {
        playerActions = new();
        playerActions.Movement.Enable();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable() {
        playerActions.Movement.Enable();
    }

    private void OnDisable() {
        playerActions.Movement.Disable();
    }

    private void Update() {
        Move();
    }

    private void Move(){
        int forwardDir = (int)playerActions.Movement.Forward.ReadValue<float>();
        int sideDir = (int)playerActions.Movement.Strafe.ReadValue<float>();
        Vector3 moveVelocity = Vector3.zero;
        if (forwardDir != 0 && sideDir != 0) {
            Vector3 moveDir = forwardDir * transform.forward + sideDir * transform.right;
            moveDir.y = 0f;
            moveDir = moveDir.normalized;
            moveVelocity = moveSpeed * moveDir;
        }
        else if (forwardDir != 0) {
            moveVelocity = forwardDir * moveSpeed  * transform.forward;
        }
        else if (sideDir != 0) {
            moveVelocity = sideDir * moveSpeed  * transform.right;
        }
        characterController.SimpleMove(moveVelocity);
    }
}
