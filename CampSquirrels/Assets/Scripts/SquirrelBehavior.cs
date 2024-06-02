using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField]
    private float gravityValue = 9.81f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Rigidbody rb;
    private bool inFlight = false;
    private float stoppingDistance;
    

    private void Awake() {
        // targetTransform = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stoppingDistance = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        if (Vector3.Distance(targetTransform.position, transform.position) >= stoppingDistance && !inFlight) {
            if( rb.velocity != Vector3.zero) { rb.velocity = Vector3.zero; }
            navMeshAgent.enabled = true;
            navMeshAgent.destination = targetTransform.position;
        } else if (!inFlight) {
            Leap();
        } else if (inFlight) {
            ApplyGravity();
        } 
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Hit: " + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "Ground":
                navMeshAgent.enabled = true;
                inFlight = false;
            break;
            default:
            break;
        }
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravityValue * Time.deltaTime * Vector3.down,ForceMode.Force);
    }

    private void Leap()
    {
        navMeshAgent.enabled = false;
        transform.LookAt(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z));
        Vector3 leapVelocity = transform.forward + transform.up.normalized;
        leapVelocity = new(leapVelocity.x * leapForce.x, leapVelocity.y * leapForce.y, leapVelocity.z * leapForce.x);
        rb.AddForce(leapVelocity, ForceMode.Impulse);
        inFlight = true;
    }
}
