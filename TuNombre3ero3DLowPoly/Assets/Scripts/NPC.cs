using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
    public enum EnemyState {
        Idle,
        Walking
    }

    [Header("MOVEMENT")]
    public float baseSpeed = 3.5f;
    public float speedMultiplier = 1.0f;

    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyState currentState = EnemyState.Idle;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = baseSpeed * speedMultiplier;
    }

    void FixedUpdate() {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > 1.5f) {
            currentState = EnemyState.Walking;
            agent.SetDestination(player.position);
        } else {
            currentState = EnemyState.Idle;
            agent.ResetPath();
        }

        UpdateAnimation();
    }

    void UpdateAnimation() {
        if (animator != null) {
            animator.SetBool("IsWalking", currentState == EnemyState.Walking);
        }
    }
}
