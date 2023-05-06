using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        BossController boss = collision.collider.GetComponentInChildren<BossController>();
        if (boss != null)
        {
            Debug.Log("Boss hit");
            boss.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
