using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquirrelBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    [Header("Leap values")]
    [SerializeField]
    [Tooltip("X = X & Z, Y = Y")]
    private Vector2 leapForce = Vector2.one;
    [SerializeField]
    private float gravityValue = 9.81f;
    [Header("Attack values")]
    [SerializeField]
    private float squirrelDamage = 1f;
    [SerializeField]
    private AudioClip attackSFX;
    

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool inFlight = false;
    private float stoppingDistance;
    

    private void Awake() {
        // targetTransform = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        stoppingDistance = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        animator.SetBool("Run", !inFlight);
        animator.SetBool("Attack", inFlight);
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
        Transform otherRoot = other.transform.root;
        switch (otherRoot.tag)
        {
            case "Ground":
                Reset();
            break;
            case "Player":
                // Debug.Log(name + " has hit the player!");
                otherRoot.GetComponent<PlayerHealth>().ChangeCurrentHP(-squirrelDamage);
                gameObject.SetActive(false);
            break;
            default:
            break;
        }
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Reset()
    {
        navMeshAgent.enabled = true;
        inFlight = false;
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravityValue * Time.deltaTime * Vector3.down,ForceMode.Force);
    }

    private void Leap()
    {
        PlaySFX(attackSFX);
        navMeshAgent.enabled = false;
        transform.LookAt(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z));
        Vector3 leapVelocity = transform.forward + transform.up.normalized;
        leapVelocity = new(leapVelocity.x * leapForce.x, leapVelocity.y * leapForce.y, leapVelocity.z * leapForce.x);
        rb.AddForce(leapVelocity, ForceMode.Impulse);
        inFlight = true;
    }

    private void PlaySFX(AudioClip ac) {
        if(audioSource.isPlaying) {audioSource.Stop();}
        audioSource.PlayOneShot(ac);
    }
}
