using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int maxObstacles = 50; // maximum number of obstacles to spawn
    public float initialSpawnRate = 1f; // initial rate at which obstacles spawn (in seconds)
    public float spawnDistance = 100f; // distance from the player to spawn obstacles
    public float spawnAreaSize = 10f; // size of the square area in front of the player to spawn obstacles
    public float minSpawnRate = 0.5f; // minimum spawn rate allowed

    private GameObject player; // reference to player (airplane) GameObject
    private float spawnTimer; // timer to control obstacle spawning
    private float spawnRate; // current spawn rate

    void Start()
    {
        spawnTimer = 0f;
        spawnRate = initialSpawnRate;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // increment spawn timer
        spawnTimer += Time.deltaTime;

        // check if it's time to spawn a new obstacle
        if (spawnTimer >= spawnRate)
        {
            // reset spawn timer
            spawnTimer = 0f;

            // spawn a new obstacle if the maximum number hasn't been reached
            if (GameObject.FindGameObjectsWithTag("Obstacle").Length < maxObstacles)
            {
                SpawnObstacle();
            }
        }

        // decrease spawn rate over time until it reaches the minimum spawn rate
        if (Time.time % 2 == 0 && spawnRate > minSpawnRate)
        {
            spawnRate -= 0.01f;
        }

        // despawn obstacles after the player has passed by them
        DespawnObstacles();
    }

    // spawn obstacles in a random position within the spawn area (a square area in front of the player)
    void SpawnObstacle()
    {
        if (player != null)
        {
            // calculate a random position within the spawn area in front of the player
            Vector3 playerPosition = player.transform.position;
            Vector3 spawnDirection = player.transform.forward;
            Vector3 spawnPosition = playerPosition + spawnDirection * spawnDistance + Random.insideUnitSphere * spawnAreaSize;

            // instantiate the obstacle at the calculated position
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }

    // despawn obstacles if they are behind the player
    void DespawnObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            // check if the obstacle's position is behind the player
            if (Vector3.Dot(player.transform.forward, obstacle.transform.position - player.transform.position) < -20)
            {
                Destroy(obstacle);
            }
        }
    }
}
