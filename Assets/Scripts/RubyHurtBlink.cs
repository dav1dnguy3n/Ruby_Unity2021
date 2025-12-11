using UnityEngine;
using System.Collections;

public class RubyHurtBlink : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How long the player stays invincible in seconds.")]
    public float invincibilityDuration = 5f;
    [Tooltip("How fast the sprite flashes on and off. Lower is faster.")]
    public float blinkInterval = 0.1f; // 0.1s off, 0.1s on

    [Header("References")]
    public AudioClip hurtSound;
    private AudioSource audioSource;
    private SpriteRenderer spriteRend;

    // This flag tracks if the player is currently invincible
    private bool isInvincible = false;

    void Start()
    {
        // Get components automatically
        audioSource = GetComponent<AudioSource>();
        spriteRend = GetComponent<SpriteRenderer>();

        // Basic error checking
        if (spriteRend == null) Debug.LogError("Player needs a SpriteRenderer component!");
    }

    // Detect collision with the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we hit is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Damageable"))
        {
            // Only take damage if we are NOT currently invincible
            if (!isInvincible)
            {
                StartCoroutine(HandleInvincibilitySequence());
            }
        }
    }

    // This Coroutine manages the whole hurt sequence
    private IEnumerator HandleInvincibilitySequence()
    {
        isInvincible = true;

        // 1. Play Sound immediately
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        // Calculate when the invincibility should end
        float endTime = Time.time + invincibilityDuration;

        // 2. Start the blinking loop Loop until the current time passes the end time
        while (Time.time < endTime)
        {
            // Turn sprite OFF
            spriteRend.enabled = false;
            // Wait for half the interval
            yield return new WaitForSeconds(blinkInterval);

            // Turn sprite ON
            spriteRend.enabled = true;
            // Wait for the other half
            yield return new WaitForSeconds(blinkInterval);
        }

        // --- Sequence Cleanup ---

        // CRITICAL: Ensure the sprite is visible when the loop ends!
        spriteRend.enabled = true;

        isInvincible = false;
        Debug.Log("Invincibility ended.");
    }
}