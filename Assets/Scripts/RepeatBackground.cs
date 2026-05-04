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

    // Start is called once before the first execution of Update
    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;

        // Get the SpriteRenderer component to change the sprite later
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - repeatWidth)
        {
            // 1. Reset position to the starting point
            transform.position = startPos;

            // 2. Add the traveled distance (equal to the repeat width)
            distanceTraveled += repeatWidth;

            // 3. Check if target distance is reached and not yet switched
            if (distanceTraveled >= targetDistance && !isSwitched)
            {
                SwitchToBackground2();
            }
        }
    }

    // Function to switch the background sprite
    private void SwitchToBackground2()
    {
        if (spriteRenderer != null && background2Sprite != null)
        {
            spriteRenderer.sprite = background2Sprite;
            isSwitched = true; // Mark as switched

            // Note: If the second sprite has a different width than the first, 
            // you might need to recalculate the repeatWidth here, for example:
            // repeatWidth = spriteRenderer.bounds.size.x / 2;
        }
        else
        {
            
        }
    }
}