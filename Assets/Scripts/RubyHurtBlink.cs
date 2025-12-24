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
    private RubyMovement rubyMovement;

    // This flag tracks if the player is currently invincible
    private bool isInvincible = false;
    private float nextDamageTime = 0f;


    void Start()
    {
        // Get components automatically
        audioSource = GetComponent<AudioSource>();
        spriteRend = GetComponent<SpriteRenderer>();
        rubyMovement = GetComponent<RubyMovement>();


        // Basic error checking
        if (spriteRend == null) Debug.LogError("Player needs a SpriteRenderer component!");
    }

    // Detect collision with enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we hit is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
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

    // Detect collide with the damage zone
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object we hit is tagged as "Enemy"
        if (collider.gameObject.CompareTag("Damageable"))
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible && Time.time >= nextDamageTime)
            {
                TakeDamage();
            }

            if (DynamicMusic.instance != null)
            {
                DynamicMusic.instance.TriggerCombatMusic();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Damageable"))
        {
            // Chỉ gây damage nếu đã hết thời gian invincible và đủ thời gian damage interval
            if (!isInvincible && Time.time >= nextDamageTime)
            {
                TakeDamage();
            }

            if (DynamicMusic.instance != null)
            {
                DynamicMusic.instance.TriggerCombatMusic();
            }
        }
    }

    private void TakeDamage()
    {
        if (rubyMovement != null)
        {
            rubyMovement.HealthChange(-1);
        }

        // Set thời gian cho lần damage tiếp theo
        nextDamageTime = Time.time + invincibilityDuration;

        // Bắt đầu hiệu ứng blink và invincibility
        StartCoroutine(HandleInvincibilitySequence());
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