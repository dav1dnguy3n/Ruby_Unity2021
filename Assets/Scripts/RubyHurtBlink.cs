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

            if (DynamicMusic.instance != null)
            {
                DynamicMusic.instance.TriggerCombatMusic();
            }
        }
    }

    private IEnumerator HandleInvincibilitySequence()
    {
        isInvincible = true;

        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        float endTime = Time.time + invincibilityDuration;

        while (Time.time < endTime)
        {
            spriteRend.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            spriteRend.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
        spriteRend.enabled = true;

        isInvincible = false;
        Debug.Log("Invincibility ended.");
    }
}