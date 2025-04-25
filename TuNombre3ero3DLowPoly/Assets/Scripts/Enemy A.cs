using UnityEngine;
using UnityEngine.AI;

public class EnemyA : MonoBehaviour {
    public Transform player;
    public float detectionRange = 10f;
    private NavMeshAgent agent;
    private Animator animator;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update() {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRange) {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        } else {
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
        }
    }
}
