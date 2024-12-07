using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMovement : MonoBehaviour
{
    [SerializeField] Transform m_firstPosition;
    [SerializeField] Transform m_secondPosition;
    [SerializeField] float m_time;

    Vector3 m_Objetivo;
    Vector3 m_StartPos;

    void Start() {
        // Empezamos en el punto A
        m_StartPos = m_firstPosition.position;
        m_Objetivo = m_secondPosition.position;
    }

    void Update() { 
        // Mueve la plataforma hacia el destino
        transform.position = Vector3.MoveTowards(transform.position, m_Objetivo, m_time * Time.deltaTime);

        // Cambia el destino si llega a un punto
        if (Vector3.Distance(transform.position, m_Objetivo) < 0.1f) {
            m_Objetivo = (m_Objetivo == m_firstPosition.position) ? m_secondPosition.position : m_firstPosition.position;
        }
    }
}
