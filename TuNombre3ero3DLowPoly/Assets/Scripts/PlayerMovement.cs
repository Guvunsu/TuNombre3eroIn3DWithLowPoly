using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public enum PlayerState {
        //WALKING
        IDLE,
        WALKING_LEFT,
        WALKING_RIGHT,
        WALKING_FORWARD,
        WALKING_BACKWARD,
        //JOGGING
        IDLE_JOGGING,
        JOGGING_LEFT,
        JOGGING_RIGHT,
        JOGGING_FORWARD,
        JOGGING_BACKWARD,
        //HIT
        MELEE_HIT,
        //CARRYING
        IDLE_CARRYING,
        CARRYING_FORWARD,
        CARRYING_BACKWARD,
        CARRYING_LEFT,
        CARRYING_RIGHT,
        //STAND_UP
        STAND_UP,
        //agregar jump
        JUMPING,
        GROUND
    }
    #region Variables

    PlayerState currentState;
    CharacterController controller;
    Animator animator;

    [SerializeField] float speed;
    float gravity;
    [SerializeField] float jumpHeight;

    Vector3 velocity;
    bool isGrounded;
    bool isJumping;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform m_handTransform;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    bool isGrabbing;

    #endregion Variables

    #region UnityMethods
    private void Start() {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gravity = -9.81f;
        jumpHeight = 20f;
    }

    void Update() {
        ActionsPlayer();
    }


    #endregion UnityMethods

    #region Moving Player
    public void ActionsPlayer() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;


            float m_MoveX = Input.GetAxis("Horizontal");
            float m_MoveY = Input.GetAxis("Vertical");
            //float m_MoveZ = if (Input.GetButtonDown( KeyCode.Space)) {

            //}

            Vector3 move = new Vector3(m_MoveX, m_MoveY/*,m_MoveZ*/);

            if (move.magnitude > 0.1f) {
                controller.Move(move * speed * Time.deltaTime);
                Quaternion m_targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * rotationSpeed);
            }

            if (Input.GetButtonDown("Jump") && isGrounded) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
                GameObject temp_otherObj = m_handTransform.GetChild(0).gameObject;
                temp_otherObj.transform.SetParent(null);
                temp_otherObj.GetComponent<Rigidbody>().isKinematic = false;
                isGrabbing = false;
            }
        }

    }

    /* //#region AnimationsActions
     //public void AnimationsActions() {
     //    float m_MoveX = Input.GetAxis("Horizontal");
     //    float m_MoveZ = Input.GetAxis("Vertical");
     //    if (isJumping) {
     //        if (velocity.y > 0) {
     //            currentState = PlayerState.JUMPING;
     //        } else if (velocity.y < 0) {
     //            currentState = PlayerState.GROUND;
     //        }
     //    } else if (m_MoveX != 0 || m_MoveZ != 0) {
     //        currentState = Input.GetKey(KeyCode.LeftShift) ? PlayerState.IDLE_JOGGING : PlayerState.IDLE;
     //    } else if (isGrounded) {
     //        currentState = PlayerState.IDLE;
     //    }

     //    // Controlar las animaciones según el estado
     //    switch (currentState) {
     //        case PlayerState.IDLE:
     //            animator.SetFloat("MoveX", 0);
     //            animator.SetFloat("MoveY", 0);
     //            animator.SetBool("IsJumping", false);
     //            break;
     //        case PlayerState.WALKING_FORWARD:
     //            animator.SetFloat("MoveX", 0);
     //            animator.SetFloat("MoveY", 0);
     //            animator.SetBool("IsJumping", false);
     //            break;
     //        case PlayerState.WALKING_BACKWARD:
     //            animator.SetFloat("MoveX", 0);
     //            animator.SetFloat("MoveY", 0);
     //            animator.SetBool("IsJumping", false);
     //            break;
     //        case PlayerState.WALKING_RIGHT:
     //            animator.SetFloat("MoveX", 0);
     //            animator.SetFloat("MoveY", 0);
     //            animator.SetBool("IsJumping", false);
     //            break;
     //        case PlayerState.WALKING_LEFT:
     //            animator.SetFloat("MoveX", 0);
     //            animator.SetFloat("MoveY", 0);
     //            animator.SetBool("IsJumping", false);
     //            break;
     //    }
     //}

     #endregion AnimationsActions*/

    #endregion MovingPlayer

    #region TriggersEnters Do Important Things
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plataform")) {
            currentPlatform = other.transform;
            lastPlatformPosition = currentPlatform.position;
        }
        if (other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
            other.transform.SetParent(transform, false);
            other.transform.position = m_handTransform.position;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E) && !isGrabbing) {
            other.transform.SetParent(m_handTransform, false);//para el padre es false y true para el objeto
            other.transform.position = m_handTransform.position;
            other.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(timer());
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Plataform")) {
            currentPlatform = null;
        }
    }
    #endregion TriggersEnters Do Important Things

    IEnumerator timer() {
        yield return new WaitForSeconds(0.3f);
        isGrabbing = true;
    }
}
