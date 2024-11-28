using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    #region Variables 

    [SerializeField] Transform m_target;


    #endregion Variables
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = m_target.position;
            other.transform.rotation = Quaternion.Euler(m_target.position);
        }
    }
}
