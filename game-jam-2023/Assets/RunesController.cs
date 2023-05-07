using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunesController : TelegraphController
{
    public int damage = 40;

    public bool isActive = false;

    public PlayerController player;
    public BossController bossController;

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

            if (Math.Abs(gameObject.transform.position.x) <= (0.5f + 0.9f) &&
                Math.Abs(gameObject.transform.position.y) <= (0.5f + 0.9f)
                )
            {
                bossController.takeDamage(damage * 20);
                Debug.Log("BOSS GOT SLAPPADOODLED LMAO");
            }

            GameObject[] souls = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var soul in souls)
            {
                if (Vector2.Distance(soul.transform.position, this.transform.position) <= 0.5f)
                {
                    soul.GetComponent<SoulController>().takeDamage(damage * 10);
                } 
            }
        }
    }

    public override void TriggerEffect()
    {
        onCompletion();
    } 
}
