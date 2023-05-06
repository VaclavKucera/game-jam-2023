using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.collider.gameObject.GetComponentInChildren<PlayerController>();
            Debug.Log("Player hit");
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Arena"))
        {
            Debug.Log("Enemy bullet wall hit");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy bullet enemy hit");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Enemy bullet boss hit");
            Destroy(gameObject);
        }
    }
}
