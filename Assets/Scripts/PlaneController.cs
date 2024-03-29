using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private GameManager gameManager;

    private float thrustIncrement = 0.1f; // acceleration
    private float maxThrust = 200f;
    private float responsiveness = 20f; // responsiveness of controls
    private float throttle = 50f;
    private float roll;
    private float pitch;
    private float yaw;

    public GameObject explosionPrefab;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // handle player inputs
    private void HandleInputs() {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) {
            throttle += thrustIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftShift)) {
            throttle -= thrustIncrement;
        }

        throttle = Mathf.Clamp(throttle, 50f, 200f);
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier * 2);
    }

    // instantiate explosion and call GameOver() on collision
    void OnCollisionEnter(Collision collision)
    {
        // check for collision with obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // instantiate the explosion effect at the collision point
            Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);

            // call GameOver() method from GameManager
            gameManager.GameOver();
        }
    }
}