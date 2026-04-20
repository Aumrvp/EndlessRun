using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public enum PowerUpType { Heal, Invincibility, SpeedBoost }

    public PowerUpType type = PowerUpType.Heal;
    public int healAmount = 1;
    public float invincibilityDuration = 4f;
    public float speedMultiplier = 2f;
    public float speedBoostDuration = 4f;
    public float rotateSpeed = 120f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        switch (type)
        {
            case PowerUpType.Heal:
                player.currentHP = Mathf.Min(player.currentHP + healAmount, player.maxHP);
                break;
            case PowerUpType.Invincibility:
                player.GrantInvincibility(invincibilityDuration);
                break;
            case PowerUpType.SpeedBoost:
                if (GameManager.Instance != null)
                    GameManager.Instance.StartSpeedBoost(speedMultiplier, speedBoostDuration);
                player.GrantInvincibility(speedBoostDuration);
                break;
        }

        Destroy(gameObject);
    }
}
