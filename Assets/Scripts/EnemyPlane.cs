using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float movementSpeed = 5f; // enemy airplane speed
    public float rotationSpeed = 2f; //enemy airplane rotation speed
    public float minDistance = 5f; // minimum distance to maintain from the player
    public float maxDistance = 15f; // maximum distance to maintain from the player
    public float avoidanceDistance = 5f; // distance to start detecting and avoiding obstacles

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ensure player reference is set
        if (player == null)
        {
            Debug.LogError("Player reference not set for EnemyAI script!");
        }
    }

    void Update()
    {
        // calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // rotate towards the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // check distance from the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // avoid obstacles
        AvoidObstacles();

        // move towards the player if too far
        if (distanceToPlayer > maxDistance)
        {
            rb.MovePosition(transform.position + transform.forward * movementSpeed * Time.deltaTime);
        }
        // move away from the player if too close
        else if (distanceToPlayer < minDistance)
        {
            rb.MovePosition(transform.position - transform.forward * movementSpeed * Time.deltaTime);
        }
        // move towards the player if within desired distance range
        else
        {
            rb.MovePosition(transform.position + transform.forward * movementSpeed * Time.deltaTime);
        }
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        Vector3 forward = transform.forward;

        // check for obstacles in front of the airplane
        if (Physics.Raycast(transform.position, forward, out hit, avoidanceDistance))
        {
            // if an obstacle is detected, adjust the movement direction
            Vector3 avoidanceDirection = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * forward;
            Quaternion targetRotation = Quaternion.LookRotation(avoidanceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
