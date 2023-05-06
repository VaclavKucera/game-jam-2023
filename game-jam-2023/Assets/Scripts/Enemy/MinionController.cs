using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    public Transform player;
    public Transform boss;
    public float speed = 0.5f;
    public int totalHealth = 100;
    private Rigidbody2D rb;

    public void takeDamage(int damage)
    {
        totalHealth -= damage;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boss = GameObject.FindGameObjectWithTag("Boss").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (totalHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Follow(boss);

        rb.velocity = transform.up * speed;
    }

    void Follow(Transform transformToFollow)
    {
        Vector3 direction = transformToFollow.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            var bossController = collision.collider.gameObject.transform.parent.GetComponentInChildren<BossController>();
            bossController.takeDamage(-10);
            Destroy(gameObject);
        }
    }
}
