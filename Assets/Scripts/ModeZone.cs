using UnityEngine;

public class ModeZone : MonoBehaviour
{
    public enum Mode { Normal, GravityFlip, Ball }

    public Mode mode = Mode.GravityFlip;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        switch (mode)
        {
            case Mode.Normal:
                player.SetGravityFlipped(false);
                player.SetBallMode(false);
                break;
            case Mode.GravityFlip:
                player.SetGravityFlipped(true);
                player.SetBallMode(false);
                break;
            case Mode.Ball:
                player.SetGravityFlipped(false);
                player.SetBallMode(true);
                break;
        }
    }
}
