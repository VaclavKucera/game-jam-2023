using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunesController : TelegraphController
{
    public int damage = 40;

    public bool isActive = false;

    public PlayerController player;
    BossController bossController;

    private void Start()
    {
        bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        onCompletion = () => { isActive = true; };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (isActive && collision.gameObject.tag == "Player") 
        { 
            player.TakeDamage(damage);
            Destroy(this.gameObject);

            if (Vector2.Distance(transform.position, Vector2.zero) <= (0.5 + 0.9))
            {
                bossController.takeDamage(damage * 10);
            }
        }
    }

    public override void TriggerEffect()
    {
        onCompletion();
    } 
}
