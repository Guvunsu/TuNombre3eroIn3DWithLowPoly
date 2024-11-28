using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent m_agent;
    [SerializeField] Transform m_position;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        m_agent.SetDestination(m_position.position);
    }
}
