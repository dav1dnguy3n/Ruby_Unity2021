using UnityEngine;

public class RubyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    private Rigidbody2D rb;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // Normalize để tránh đi chéo nhanh hơn (gian lận tốc độ)
        if (input.sqrMagnitude > 1f) input.Normalize();
        
        // Smooth velocity với SmoothDamp
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, input * moveSpeed, ref velocity, smoothTime);
    }
}