using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Fired! Collided with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

            RubyMovement ruby = collision.GetComponent<RubyMovement>();
            if (ruby != null)
            {
                ruby.HealthChange(-1);
                // ruby.SendMessage("HealthChange", 1);
            }
        }
    }
}
