using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;

    public int maxHP = 3;
    public int currentHP = 3;
    public float invincibilityDuration = 1.5f;
    public bool isInvincible = false;
    public Renderer[] playerRenderers;

    public bool isFlipped = false;
    public bool isBallMode = false;
    public GameObject normalMesh;
    public GameObject ballMesh;
    public float ballScale = 0.5f;
    public float floorY = 0f;
    public float ceilingY = 4f;

    private Vector3 baseGravity;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    // 4.1 add animator variable
    public Animator animator;
    // 5.2 add particle system variable for dirt splatter effect
    public ParticleSystem fxDirtSplatter;
    // 5.3 add particle system variable for explosion smoke effect
    public ParticleSystem fxExplosionSmoke;
    // 5.7 add audio clip variable for crash sound
    public AudioClip crashSound;
    private Rigidbody rb;
    private InputAction jumpAction;
    // 5.8 add audio source variable to play crash sound
    private AudioSource audioSource;

    private bool isOnGround = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");

        // 5.8 get audio source component, if not exist, add one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        baseGravity = Physics.gravity;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        currentHP = maxHP;

        // 4.1 set animator parameter's `Speed_f` to 1f at start to make player play running animation
        if (animator != null)
        {
            animator.SetFloat("Speed_f", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpAction.triggered && isOnGround)
        {
            Vector3 jumpDir = isFlipped ? Vector3.down : Vector3.up;
            rb.AddForce(jumpForce * jumpDir, ForceMode.Impulse);
            isOnGround = false;
            // 4.2 set animator trigger `Jump_trig` to make player play jump animation
            animator.SetTrigger("Jump_trig");
            // 5.2 stop dirt splatter effect when player jumps
            fxDirtSplatter.Stop();
        }
    }

    public void SetGravityFlipped(bool flipped)
    {
        if (isFlipped == flipped) return;
        isFlipped = flipped;

        Physics.gravity = flipped ? -baseGravity : baseGravity;
        transform.rotation = flipped
            ? originalRotation * Quaternion.Euler(0, 0, 180)
            : originalRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 pos = transform.position;
        pos.y = flipped ? ceilingY : floorY;
        transform.position = pos;

        isOnGround = true;
    }

    public void SetBallMode(bool ball)
    {
        isBallMode = ball;
        transform.localScale = ball ? originalScale * ballScale : originalScale;

        if (normalMesh != null) normalMesh.SetActive(!ball);
        if (ballMesh != null) ballMesh.SetActive(ball);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            // 5.2 play dirt splatter effect when player lands on the ground
            fxDirtSplatter.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isInvincible) return;
            TakeDamage(collision.contacts[0].point);
        }
    }

    void TakeDamage(Vector3 hitPoint)
    {
        currentHP--;
        Instantiate(fxExplosionSmoke, hitPoint, Quaternion.identity);
        audioSource.PlayOneShot(crashSound);

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    void Die()
    {
        gameOver = true;
        if (GameManager.Instance != null) GameManager.Instance.StopRun();
        animator.SetBool("Death_b", true);
        animator.SetInteger("DeathType_int", 1);
        fxDirtSplatter.Stop();
    }

    System.Collections.IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;
        float flashInterval = 0.1f;

        while (elapsed < invincibilityDuration)
        {
            SetRenderersEnabled(false);
            yield return new WaitForSeconds(flashInterval);
            SetRenderersEnabled(true);
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }

        SetRenderersEnabled(true);
        isInvincible = false;
    }

    void SetRenderersEnabled(bool enabled)
    {
        if (playerRenderers == null) return;
        foreach (var r in playerRenderers)
        {
            if (r != null) r.enabled = enabled;
        }
    }

    public void GrantInvincibility(float duration)
    {
        StopCoroutine(nameof(InvincibilityFlash));
        invincibilityDuration = duration;
        StartCoroutine(InvincibilityFlash());
    }
}
