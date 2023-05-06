using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Mechanics Objects")]

    [Header("Scripts")]
    public Mechanics Mechanics;

    [Header("Configuration")]
    public float tetherCooldown;
    public float runesCooldown;

    [Header("Animation Durations")]
    public float tetherActivationDuration = 3f;
    public float runesActivationDuration = 3f;

    public const int totalHealth = 40000;

    protected float tetherTimer;
    protected float runesTimer;
    protected bool isVulnerable = false;
    protected int currentHealth = totalHealth;

    public enum Phases { BeforeStart, MainPhase_1, MainPhase_2, MainPhase_3, FinalPhase };
    public Phases currentPhase = 0;


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
            if (currentPhase == Phases.BeforeStart)
            {
                currentHealth = totalHealth;
                currentPhase++;
                StartFight();
            }
            if (currentPhase == Phases.MainPhase_1 && currentHealth - damage <= hp_66)
            {
                currentHealth = hp_66;
                currentPhase++;
                makeInvulnerable();
            }
            if (currentPhase == Phases.MainPhase_2 && currentHealth - damage <= hp_33)
            {
                currentHealth = hp_33;
                currentPhase++;
                makeInvulnerable();
            }
            if (currentPhase == Phases.MainPhase_3 && currentHealth - damage <= 0)
            {
                currentHealth = 0;
                makeInvulnerable();
                currentPhase++; // trigger the final event
            }

            currentHealth -= damage;
        }

    }
    // Start is called before the first frame update
    public void StartFight()
    {
        tetherTimer = Time.time;
        runesTimer = tetherTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - tetherTimer >= tetherCooldown)
        {
            Mechanics.Tether();
            tetherTimer = Time.time + tetherActivationDuration;
        }

        if (Time.time - runesTimer >= runesCooldown)
        {
            Mechanics.Runes();
            runesTimer = Time.time + runesActivationDuration;
        }
    }
}
