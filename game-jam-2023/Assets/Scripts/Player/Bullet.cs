using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 150;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Boss"))
        {
            var boss = collision.collider.gameObject.transform.parent.GetComponentInChildren<BossController>();
            Debug.Log("Boss hit");
            boss.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Arena"))
        {
            Debug.Log("Wall hit");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            var minion = collision.collider.gameObject.GetComponentInChildren<MinionController>();
            if (minion != null)
            {
                minion.takeDamage(damage);
                Debug.Log("Minion hit");
            }

            var turret = collision.collider.gameObject.GetComponentInChildren<TurretShooting>();
            if (turret != null)
            {
                turret.takeDamage(damage);
                Debug.Log("Turret hit");
            }

            Destroy(gameObject);
        }
    }
}
