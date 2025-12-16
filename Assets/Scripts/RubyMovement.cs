using UnityEngine;

public class RubyMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public AudioClip moveSound;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private AudioSource audioSource;
    private bool isMoving = false;

    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    public int maxHealth = 5;
    public int currentHealth { get { return _currentHealth; } }
    int _currentHealth;

    public int maxAmmo = 20;
    public int currentAmmo { get { return _currentAmmo; } }
    int _currentAmmo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _currentHealth = maxHealth;
        _currentAmmo = 5;
        Debug.Log("Ruby Started! Health: " + _currentHealth + ", Ammo: " + _currentAmmo);
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // Normalize để tránh đi chéo nhanh hơn (gian lận tốc độ)
        if (input.sqrMagnitude > 0.01f) input.Normalize();
        
        Vector2 newPosition = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        bool currentlyMoving = input.sqrMagnitude > 0.01f;
        if (currentlyMoving && !isMoving)
        {
            if (audioSource != null && moveSound != null)
            {
                audioSource.clip = moveSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (!currentlyMoving && isMoving)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
        isMoving = currentlyMoving;

        // Cập nhật hướng di chuyển nếu có input
        if (input.sqrMagnitude > 0.01f)
        {
            moveDirection = input;
        }

        // Cập nhật animation parameters
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", input.magnitude);

        // Debug.Log($"Velocity: {rb.linearVelocity}, Speed: {rb.linearVelocity.magnitude}");
    }

    public void HealthChange (int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        Debug.Log($"Current Health: {_currentHealth}/{maxHealth}");
    }

    public void AmmoChange (int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Current Ammo: {_currentAmmo}/{maxAmmo}");
    }
}