using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public bool isVulnerable = false;
    public int currentHealth = totalHealth;
    public int phase = 0;

    public const int totalHealth = 40000;

    private const int hp_75 = totalHealth / 100 * 75;
    private const int hp_66 = totalHealth / 100 * 66;
    private const int hp_50 = totalHealth / 100 * 50;
    private const int hp_33 = totalHealth / 100 * 33;
    private const int hp_25 = totalHealth / 100 * 25;

    public void makeVulnerable()
    {
        isVulnerable = true;
    }

    public void makeInvulnerable()
    {
        isVulnerable = false;
    }

    public void takeDamage(int damage)
    {
        if (!isVulnerable)
        {
            if (phase == 0)
            {
                currentHealth = totalHealth;
                phase++;
            }
            if (phase == 1 && currentHealth - damage <= hp_66)
            {
                currentHealth = hp_66;
                phase++;
                makeInvulnerable();
            }
            if (phase == 2 && currentHealth - damage <= hp_33) 
            {
                currentHealth = hp_33;
                phase++;
                makeInvulnerable();
            }
            if (phase == 3 && currentHealth - damage <= 0)
            {
                makeInvulnerable();
                phase++; // trigger the final event
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
