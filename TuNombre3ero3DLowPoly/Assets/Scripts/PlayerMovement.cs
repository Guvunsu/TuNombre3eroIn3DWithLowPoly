using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    #region ENUM
    public enum playerFSM {
        IDLE,
        WALKING,
        JOGGING,
        JUMPING,
        TAKE_IT,
        HIT,
        ISGROUNDED,
        NOT_GROUNDED
    }

    public enum GameState {
        PLAYING,
        WIN,
        LOSE
    }

    #endregion ENUM

    #region VARIABLES
    [SerializeField] protected playerFSM Player_FSM;
    [SerializeField] protected GameState gameFSM;

    [SerializeField] Rigidbody rb;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [SerializeField] Transform m_handTransform;
    private Transform currentPlatform;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;

    public Vector3 lastPlatformPosition;
    protected Vector3 direction;
    Vector3 velocity;

    [SerializeField] float gravity = -9.81f;

    [SerializeField] float jumpForce = 21f;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float jogSpeed = 25f;
    [SerializeField] float accelerationSpeed = 1.1f;

    bool isGrounded;
    bool isGrabbing;

    #endregion VARIABLES

    #region PublicUnityMethods

    void Start() {
        Player_FSM = playerFSM.IDLE;
        gameFSM = GameState.PLAYING;
    }

    void FixedUpdate() {
        CheckGround();

        if (!isGrounded && velocity.y > -20f) {
            velocity.y += gravity * Time.fixedDeltaTime;
        } else if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        Vector3 move = new Vector3(direction.x, 0f, direction.z).normalized;
        controller.Move(move * moveSpeed * Time.fixedDeltaTime);

        controller.Move(velocity * Time.fixedDeltaTime);

        switch (Player_FSM) {
            case playerFSM.WALKING:
                Move(moveSpeed);
                break;

            case playerFSM.JOGGING:
                Move(jogSpeed);
                break;

            case playerFSM.JUMPING:// creoq ue esto deberiua estar ebn negativo
                CheckGround();//checa y verifica matematicamente si esta en el suelo e ISGROUNDED en el FSM
                animator.SetBool("IsJumping", false);
                break;
            case playerFSM.HIT:
                break;
            case playerFSM.IDLE:
                animator.SetFloat("Speed", 0f);
                break;
        }
    }

    #endregion PublicUnityMethods

    #region PrivateMethods
    void Move(float speed) {
        Vector3 move = new Vector3(direction.x, 0, direction.z).normalized;
        if (move.magnitude >= 0.1f) {
            transform.Translate(move * speed * Time.fixedDeltaTime, Space.World);
            animator.SetFloat("Speed", speed);
        }
    }

    void CheckGround() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("IsGrounded", isGrounded);
        //Player_FSM = isGrounded ? playerFSM.ISGROUNDED : playerFSM.NOT_GROUNDED;
    }

    #endregion PrivateMethods

    #region PublicMethods
    public void MovePlayerDoll(InputAction.CallbackContext value) {
        if (value.performed && gameFSM == GameState.PLAYING) {
            Player_FSM = playerFSM.WALKING;
            Vector2 input = value.ReadValue<Vector2>();
            direction = new Vector3(input.x, 0f, input.y);
        } else if (value.canceled && gameFSM == GameState.PLAYING) {
            Player_FSM = playerFSM.IDLE;
            direction = Vector3.zero;
        }
    }
    public void JoggingPLayerDoll(InputAction.CallbackContext value) {
        if (value.performed && gameFSM == GameState.PLAYING) {
            Player_FSM = playerFSM.JOGGING;
            direction = new Vector3(jogSpeed * moveSpeed * accelerationSpeed, jogSpeed, jogSpeed);
        } else if (value.canceled) {
            Player_FSM = playerFSM.IDLE;
            direction = Vector3.zero;
        }
    }
    public void JumpPlayerDoll(InputAction.CallbackContext value) {
        if (value.performed && isGrounded && Player_FSM != playerFSM.JUMPING) {
            Player_FSM = playerFSM.JUMPING;
            velocity.y = Mathf.Sqrt(jumpForce * -10 * gravity);
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("IsGrounded");
        }
    }
    public void InteractionPlayerDoll(InputAction.CallbackContext value) {
        if (value.performed && isGrabbing) {
            Player_FSM = playerFSM.TAKE_IT;
            animator.SetBool("Ungrabe", true);
        } else {
            animator.SetBool("Ungrabe", false);
        }
    }
    public void HitPlayerDoll(InputAction.CallbackContext value) {
        if (value.performed) {
            Player_FSM = playerFSM.HIT;
            animator.SetTrigger("Hit");
        }
    }

    #endregion PublicMethods








    //    #region Variables

    //    playerFSM playerCurrentFSM;
    //    CharacterController controller;
    //    Animator animator;

    //    Vector3 direction;

    //    bool isGrounded;
    //    bool isGrabbing;

    //    [SerializeField] float walkSpeed = 30f;
    //    [SerializeField] float jogSpeed = 60f;
    //    [SerializeField] float jumpHeight = 20f;

    //    [SerializeField] float rotationSpeed = 50f;
    //    [SerializeField] float speed;
    //    [SerializeField] float gravity = -9.81f;

    //    [SerializeField] Transform m_handTransform;
    //    private Transform currentPlatform;
    //    private Vector3 lastPlatformPosition;

    //    [SerializeField] float groundDistance;
    //    [SerializeField] Transform groundCheck;
    //    [SerializeField] LayerMask groundMask;

    //    #endregion Variables

    //    #region UnityMethods
    //    void Start() {
    //        //hacer continuo la gravedad a fucnion del tiempo 
    //        animator = GetComponent<Animator>();
    //        controller = GetComponent<CharacterController>();
    //        jumpHeight = 20f;
    //    }

    //    void FixedUpdate() {
    //        gravity += Time.fixedDeltaTime;

    //    }
    //    void Update() {
    //        MovementPlayer();
    //        JumpingPlayer();
    //        UpdateAnimator();
    //    }


    //    #endregion UnityMethods

    //    #region TriggersEnters Do Important Things

    //    void OnTriggerEnter(Collider other) {
    //        if (other.CompareTag("Plataform")) {
    //            currentPlatform = other.transform;
    //            lastPlatformPosition = currentPlatform.position;
    //        }
    //        if (other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
    //            other.transform.SetParent(transform, false);
    //            other.transform.position = m_handTransform.position;
    //        }
    //    }
    //    void OnTriggerStay(Collider other) {
    //        if (!isGrabbing && other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
    //            other.transform.SetParent(m_handTransform, false);//para el padre es false y true para el objeto
    //            other.transform.position = m_handTransform.position;
    //            other.GetComponent<Rigidbody>().isKinematic = true;
    //            isGrabbing = true;
    //            StartCoroutine(timer());
    //        }

    //    }
    //    IEnumerator timer() {
    //        yield return new WaitForSeconds(0.3f);
    //        isGrabbing = true;
    //    }


    //    void OnTriggerExit(Collider other) {
    //        if (other.CompareTag("Plataform")) {
    //            currentPlatform = null;
    //        }
    //        if (other.CompareTag("Ungrabe") && Input.GetKeyDown(KeyCode.E)) {
    //            Debug.Log("Intento soltarlo pero no quiero :V");
    //            isGrabbing = false;
    //        }
    //    }
    //    #endregion TriggersEnters Do Important Things

    //    #region Moving Player & animator

    //    #region Animator
    //    void UpdateAnimator() {

    //        float Xaxis = Input.GetAxis("Horizontal");
    //        float Yaxis = Input.GetAxis("Vertical");

    //        animator.SetFloat("Xaxis", Xaxis);
    //        animator.SetFloat("Yaxis", Yaxis);

    //        // Parámetros de salto
    //        animator.SetBool("IsJumping", playerCurrentFSM == playerFSM.JUMPING);
    //        animator.SetBool("IsGrounded", isGrounded);

    //        if (Input.GetKeyDown(KeyCode.H)) {
    //            animator.SetTrigger("Hit");
    //            playerCurrentFSM = playerFSM.HIT;
    //        }

    //        if (isGrabbing) {
    //            playerCurrentFSM = playerFSM.CARRYING;
    //            animator.SetBool("Ungrabe", true);
    //        } else {
    //            animator.SetBool("Ungrabe", false);
    //        }



    //    }
    //    #endregion Animator

    //    void JumpingPlayer() {
    //        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
    //            Debug.Log("Si Brinco pero no lo hago ");
    //            direction.y = Mathf.Sqrt(jumpHeight * -20f * gravity);
    //            playerCurrentFSM = playerFSM.JUMPING;
    //            animator.SetTrigger("JumpStart");
    //        } else if (!isGrounded) {
    //            playerCurrentFSM = playerFSM.ISGROUNDED;
    //        }
    //        //actualiza la verticalidad del eje y por la gravedad 
    //        direction.y -= gravity * Time.deltaTime;
    //        controller.Move(direction * Time.deltaTime);
    //    }
    //    void MovementPlayer() {
    //        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    //        if (isGrounded && direction.y < 0) {
    //            direction.y = -2f;


    //            float m_MoveX = Input.GetAxis("Horizontal");
    //            float m_MoveY = Input.GetAxis("Vertical");
    //            Vector3 move = new Vector3(m_MoveX, m_MoveY);

    //            if (move.magnitude > 0.1f) {
    //                float speed = Input.GetKeyDown(KeyCode.LeftShift) ? jogSpeed : walkSpeed;
    //                Debug.Log("Si troto pero no lo hago ");
    //                controller.Move(move * speed * Time.deltaTime);
    //                Quaternion m_targetRotation = Quaternion.LookRotation(move);
    //                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * rotationSpeed);
    //                playerCurrentFSM = Input.GetKey(KeyCode.LeftShift) ? playerFSM.JOGGING : playerFSM.WALKING;
    //            } else {
    //                playerCurrentFSM = playerFSM.IDLE;
    //            }

    //            direction.y -= gravity * Time.deltaTime;
    //            controller.Move(direction * Time.deltaTime);

    //            // Movimiento de la plataforma
    //            if (currentPlatform != null) {
    //                Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
    //                //Vector3 rotationPos = Rotacion de la plataforma;
    //                //Player.rotation = transform.rotation + rotationPos;
    //                controller.Move(platformMovement); // Mueve el personaje con la plataforma
    //                lastPlatformPosition = currentPlatform.position;
    //            }
    //            if (isGrabbing && Input.GetKeyDown(KeyCode.E)) {
    //                Debug.Log("Si tomo cosas pero no lo hago ");
    //                GameObject temp_otherObj = m_handTransform.GetChild(0).gameObject;
    //                temp_otherObj.transform.SetParent(null);
    //                temp_otherObj.GetComponent<Rigidbody>().isKinematic = false;
    //                isGrabbing = false;
    //            }
    //        }

    //        #endregion MovingPlayer & animator



    //    }

}
