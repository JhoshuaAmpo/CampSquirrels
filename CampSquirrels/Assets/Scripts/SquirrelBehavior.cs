using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;

public class SquirrelBehavior : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float attackDelay;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    [Tooltip("X = X & Z, Y = Y")]
    private Vector2 leapForce = Vector2.one;
    [SerializeField]
    private float moveSpeed;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;
    private Vector3 squirrelVelocity;
    private Vector3 initialSquirrelVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    private float leapTimer = 0f;

    bool isAttacking = false;

    private void Awake() {
        // targetTransform = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        navMeshAgent.enabled = Vector3.Distance(targetTransform.position, transform.position) >= navMeshAgent.stoppingDistance;
        if(navMeshAgent.enabled) {
            navMeshAgent.destination = targetTransform.position;
            Vector3 initDir = (transform.forward + transform.up).normalized;
            initialSquirrelVelocity = new(initDir.x * leapForce.x, initDir.y * leapForce.y, initDir.z * leapForce.x);
            squirrelVelocity = initialSquirrelVelocity;
            leapTimer = 0;
        } else {
            Leap();
        }
    }

    private void Leap()
    {
        squirrelVelocity.y -= gravityValue;
        Debug.Log("SquirrelVelocity" + squirrelVelocity);
        characterController.Move(squirrelVelocity * Time.deltaTime);
        // if(characterController.isGrounded) {
        //     navMeshAgent.enabled = true;
        //     squirrelVelocity = initialSquirrelVelocity;
        // }
        leapTimer += Time.deltaTime;
        navMeshAgent.enabled = characterController.isGrounded;
    }
}
