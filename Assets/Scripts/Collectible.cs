using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject collectibleEffect;
    public AudioClip pickupSound;

    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + (Mathf.Sin(Time.time * floatSpeed) * floatHeight);

        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Fired! Collided with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 1f);
            }
            if (collectibleEffect != null)
            {
                Instantiate(collectibleEffect, transform.position, Quaternion.identity);
            }

            if (gameObject.CompareTag("Health"))
            {
                RubyMovement ruby = collision.GetComponent<RubyMovement>();
                if (ruby != null && ruby.currentHealth < ruby.maxHealth)
                {
                    ruby.HealthChange(1);
                    // ruby.SendMessage("HealthChange", 1);
                }
            }
            
            if (gameObject.CompareTag("Ammo"))
            {
                RubyMovement ruby = collision.GetComponent<RubyMovement>();
                if (ruby != null && ruby.currentAmmo < ruby.maxAmmo)
                {
                    ruby.AmmoChange(5);
                    // ruby.SendMessage("AmmoChange", 5);
                }
            }

            Debug.Log("Player collected the item!");
            Destroy(gameObject);
        }
    }
}
