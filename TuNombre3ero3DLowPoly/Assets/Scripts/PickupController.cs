using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Rigidbody itemtRB;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform container;
    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;
    public static bool slotFull;
    public bool equipaded;
    // Start is called before the first frame update
    void Start() {
        if (!equipaded) {
            itemtRB.isKinematic = true;
            itemCollider.isTrigger = false;

        } else {
            itemtRB.isKinematic = true;
            itemCollider.isTrigger = true;
            slotFull = true;
        }

    }

    // Update is called once per frame
    void Update() {
        Vector3 distamceToPlayer = transform.position - transform.position;
        if (!equipaded && distamceToPlayer.magnitude <= pickUpRange) {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            if (Input.GetKeyDown(KeyCode.E) && !slotFull) {
                PickUp();
            }
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        if (equipaded && Input.GetKeyDown(KeyCode.Q)) {
            Drop();
        }

    }
    private void PickUp() {
        equipaded = true;
        slotFull = true;
        gameObject.GetComponent<Renderer>().material.color = Color.white;

        //make weapon a child of the player
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // make rb kinematic and boxcollider a trigger
        itemtRB.isKinematic = true;
        itemCollider.isTrigger = true;
    }

    private void Drop() {
        equipaded = false;
        slotFull = false;

        //set parent to null
        transform.SetParent(null);

        // make rb not kinematic and boxcollider normal
        itemtRB.isKinematic = false;
        itemCollider.isTrigger = false;

        //add random rotation
        float random = Random.Range(-4f, 1f);
        itemtRB.AddTorque(new Vector3(random, random, random) * 10);
    }
}
