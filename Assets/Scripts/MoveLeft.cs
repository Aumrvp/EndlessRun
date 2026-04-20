using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;

    private PlayerController playerController;

    void Start()
    {
        var playerObj = GameObject.Find("Player");
        if (playerObj != null) playerController = playerObj.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController != null && playerController.gameOver)
        {
            return;
        }

        float multiplier = GameManager.Instance != null ? GameManager.Instance.speedMultiplier : 1f;
        transform.Translate(Vector3.left * speed * multiplier * Time.deltaTime, Space.World);

        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
