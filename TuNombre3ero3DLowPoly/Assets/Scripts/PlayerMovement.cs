using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public enum PlayerState {
        IDLE,
        WALKING,
        JOGGING,
        JUMPING,
        CARRYING,
        HIT,
        GROUNDED
    }
    #region Variables

    PlayerState currentState;
    CharacterController controller;
    Animator animator;

    Vector3 velocity;

    bool isGrounded;
    bool isGrabbing;

    [SerializeField] float walkSpeed = 30f;
    [SerializeField] float jogSpeed = 60f;
    [SerializeField] float jumpHeight = 20f;

    [SerializeField] float speed;
    float gravity;

    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] Transform m_handTransform;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    [SerializeField] float groundDistance;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    #endregion Variables

    #region UnityMethods
    private void Start() {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gravity = -9.81f;
        jumpHeight = 20f;
    }

    void Update() {
        MovementPlayer();
        JumpingPlayer();
        UpdateAnimator();
    }


    #endregion UnityMethods

    #region TriggersEnters Do Important Things

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plataform")) {
            currentPlatform = other.transform;
            lastPlatformPosition = currentPlatform.position;
        }
        if (other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
            other.transform.SetParent(transform, false);
            other.transform.position = m_handTransform.position;
        }
    }
    void OnTriggerStay(Collider other) {
        if (!isGrabbing && other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
            other.transform.SetParent(m_handTransform, false);//para el padre es false y true para el objeto
            other.transform.position = m_handTransform.position;
            other.GetComponent<Rigidbody>().isKinematic = true;
            isGrabbing = true;
            StartCoroutine(timer());
        }

    }
    IEnumerator timer() {
        yield return new WaitForSeconds(0.3f);
        isGrabbing = true;
    }


    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Plataform")) {
            currentPlatform = null;
        }
        if (other.CompareTag("Ungrabe")&& Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Intento soltarlo pero no quiero :V");
            isGrabbing = false;
        }
    }
    #endregion TriggersEnters Do Important Things

    #region Moving Player & animator

    #region Animator
    void UpdateAnimator() {

        float Xaxis = Input.GetAxis("Horizontal");
        float Yaxis = Input.GetAxis("Vertical");

        animator.SetFloat("Xaxis", Xaxis);
        animator.SetFloat("Yaxis", Yaxis);

        // Parámetros de salto
        animator.SetBool("IsJumping", currentState == PlayerState.JUMPING);
        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.H)) {
            animator.SetTrigger("Hit");
            currentState = PlayerState.HIT;
        }

        if (isGrabbing) {
            currentState = PlayerState.CARRYING;
            animator.SetBool("Ungrabe", true);
        } else {
            animator.SetBool("Ungrabe", false);
        }



    }
    #endregion Animator

    void JumpingPlayer() {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            Debug.Log("Si Brinco pero no lo hago ");
            velocity.y = Mathf.Sqrt(jumpHeight * -20f * gravity);
            currentState = PlayerState.JUMPING;
            animator.SetTrigger("JumpStart");
        } else if (!isGrounded) {
            currentState = PlayerState.GROUNDED;
        }
        //chatgpt 
        //actualiza la verticalidad del eje y por la gravedad 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void MovementPlayer() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;


            float m_MoveX = Input.GetAxis("Horizontal");
            float m_MoveY = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(m_MoveX, m_MoveY);

            if (move.magnitude > 0.1f) {
                float speed = Input.GetKeyDown(KeyCode.LeftShift) ? jogSpeed : walkSpeed;
                Debug.Log("Si troto pero no lo hago ");
                controller.Move(move * speed * Time.deltaTime);
                Quaternion m_targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * rotationSpeed);
                currentState = Input.GetKey(KeyCode.LeftShift) ? PlayerState.JOGGING : PlayerState.WALKING;
            } else {
                currentState = PlayerState.IDLE;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Movimiento de la plataforma
            if (currentPlatform != null) {
                Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
                //Vector3 rotationPos = Rotacion de la plataforma;
                //Player.rotation = transform.rotation + rotationPos;
                controller.Move(platformMovement); // Mueve el personaje con la plataforma
                lastPlatformPosition = currentPlatform.position;
            }
            if (isGrabbing && Input.GetKeyDown(KeyCode.E)) {
                Debug.Log("Si tomo cosas pero no lo hago ");
                GameObject temp_otherObj = m_handTransform.GetChild(0).gameObject;
                temp_otherObj.transform.SetParent(null);
                temp_otherObj.GetComponent<Rigidbody>().isKinematic = false;
                isGrabbing = false;
            }
        }

        #endregion MovingPlayer & animator



    }

}
