using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivatedRagdoll : MonoBehaviour {
    [SerializeField] Rigidbody m_hipsRB;
    Animator m_animator;
    bool m_flag;
    void Start() {
        m_animator = GetComponent<Animator>();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            m_animator.enabled = m_flag;
            m_hipsRB.isKinematic = m_flag;

            m_flag = !m_flag;


        }
    }
}
