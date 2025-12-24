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
    public int maxHealth = 10;
    public int currentHealth { get { return _currentHealth; } }
    int _currentHealth;

    public int maxAmmo = 20;
    public int currentAmmo { get { return _currentAmmo; } }
    int _currentAmmo;

    public float invincibleTime = 2.0f;
    float invincibleTimer;
    public bool isInvincible { get { return invincibleTimer > 0; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _currentHealth = maxHealth;
        _currentAmmo = 5;
        Debug.Log("Ruby Started! Health: " + _currentHealth + ", Ammo: " + _currentAmmo);
        
        // Cập nhật UI thanh máu khi bắt đầu game (dùng Invoke để đảm bảo UIHandler đã khởi tạo)
        Invoke("UpdateHealthUI", 0.1f);
    }

    void UpdateHealthUI()
    {
        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetHealthValue((float)_currentHealth / maxHealth);
        }
    }

    void FixedUpdate()
    {
        // Cập nhật invincible timer
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.fixedDeltaTime;
        }

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
        // Nếu đang bất khả xâm phạm và nhận sát thương, bỏ qua
        if (amount < 0 && isInvincible)
        {
            return;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        // Debug.Log($"Current Health: {_currentHealth}/{maxHealth}");
        UIHandler.instance.SetHealthValue(_currentHealth / (float)maxHealth);

        // Nếu nhận sát thương, kích hoạt invincibility
        if (amount < 0)
        {
            invincibleTimer = invincibleTime;
        }
    }

    public void AmmoChange (int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Current Ammo: {_currentAmmo}/{maxAmmo}");
    }
}