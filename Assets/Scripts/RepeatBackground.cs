using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth = 50;

    [Header("Background Switch")]
    public Sprite background2Sprite;    // Sprite for the second background, assign in the Inspector
    public float targetDistance = 200f; // Target distance to switch the background (e.g., 200m)

    private SpriteRenderer spriteRenderer;
    private float distanceTraveled = 0f; // Tracks the total distance traveled
    private bool isSwitched = false;     // Flag to prevent switching more than once

    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;

            distanceTraveled += repeatWidth;

            if (distanceTraveled >= targetDistance && !isSwitched)
            {
                SwitchToBackground2();
            }
        }
    }

    private void SwitchToBackground2()
    {
        if (spriteRenderer != null && background2Sprite != null)
        {
            spriteRenderer.sprite = background2Sprite;
            isSwitched = true;
        }
    }
}
