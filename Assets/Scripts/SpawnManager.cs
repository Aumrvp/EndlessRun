using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] obstaclePrefabs;
    public float minSpawnRate = 1.2f;
    public float maxSpawnRate = 2.5f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Invoke(nameof(Spawn), minSpawnRate);
    }

    void Spawn()
    {
        if (playerController.gameOver) return;

        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(prefab, spawnPoint.position, prefab.transform.rotation);
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
