using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject coinPrefab;
    public float minSpawnRate = 1.5f;
    public float maxSpawnRate = 3f;
    public float minY = 0.5f;
    public float maxY = 2.5f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Invoke(nameof(Spawn), minSpawnRate);
    }

    void Spawn()
    {
        if (playerController.gameOver) return;

        Vector3 pos = spawnPoint.position;
        pos.y = Random.Range(minY, maxY);
        Instantiate(coinPrefab, pos, coinPrefab.transform.rotation);

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
