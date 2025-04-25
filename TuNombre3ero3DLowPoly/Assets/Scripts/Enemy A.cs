using UnityEngine;
using UnityEngine.AI;

public class EnemyA : MonoBehaviour {
    public float detectionRange = 15f;
    public float moveSpeed = 15f;

    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate() {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRange) {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);

            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 extraMovement = direction * moveSpeed * Time.fixedDeltaTime;

            agent.Move(extraMovement);
        } else {
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
        }
    }
}
