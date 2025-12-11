using UnityEngine;

public class RubyMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 velocity;

    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // Normalize để tránh đi chéo nhanh hơn (gian lận tốc độ)
        if (input.sqrMagnitude > 0.01f) input.Normalize();
        
        // Smooth velocity với SmoothDamp
        transform.position += (Vector3)(input * moveSpeed * Time.fixedDeltaTime);

        // Cập nhật hướng di chuyển nếu có input
        if (input.sqrMagnitude > 0.01f)
        {
            moveDirection = input;
        }

        // Cập nhật animation parameters
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", input.magnitude);

        Debug.Log($"Velocity: {rb.linearVelocity}, Speed: {rb.linearVelocity.magnitude}");
    }
}