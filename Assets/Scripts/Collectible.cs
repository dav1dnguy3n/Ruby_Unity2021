using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject collectibleEffect;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Fired! Collided with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");
            // Tạo hiệu ứng khi thu thập
            if (collectibleEffect != null)
            {
                Instantiate(collectibleEffect, transform.position, Quaternion.identity);
            }
            // Hủy đối tượng thu thập
            Debug.Log("Player collected the item!");
            Destroy(gameObject);
        }
    }
}
