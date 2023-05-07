using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Mechanics Objects")]
    public GameObject Pylon_Top;
    public GameObject Pylon_TopRight;
    public GameObject Pylon_BottomRight;
    public GameObject Pylon_BottomLeft;
    public GameObject Pylon_TopLeft;

    [Header("Scripts")]
    public Mechanics Mechanics;

    [Header("Configuration")]
    public float tetherCooldown;
    public float runesCooldown;

    [Header("Animation Durations")]
    public float tetherActivationDuration = 3f;
    public float runesActivationDuration = 3f;

    [Header("Public Fields")]
    public const int totalHealth = 40000;
    
    
    public bool isAbleToAutoAttack = true;
    public bool isAttacking = false;

    protected float tetherTimer;
    protected float runesTimer;
    protected bool isVulnerable = false;
    protected int currentHealth = totalHealth;

    [Flags]
    public enum Breakpoints {
        break_100 = 1, 
        break_75 = 2, 
        break_66 = 4, 
        break_50 = 8,
        break_40 = 16,
        break_33 = 32, 
        break_30 = 64, 
        break_25 = 128, 
        break_20 = 256, 
        break_10 = 512, 
        break_0 = 1024,
    }

    public Breakpoints breakpoints;

    #region hp_breakpoints
    private const int hp_75 = totalHealth / 100 * 75;
    private const int hp_66 = totalHealth / 100 * 66;
    private const int hp_50 = totalHealth / 100 * 50;
    private const int hp_40 = totalHealth / 100 * 40;
    private const int hp_33 = totalHealth / 100 * 33;
    private const int hp_30 = totalHealth / 100 * 30;
    private const int hp_25 = totalHealth / 100 * 25;
    private const int hp_20 = totalHealth / 100 * 20;
    private const int hp_10 = totalHealth / 100 * 10; 
    private const int hp_0 = 0;
    #endregion

    public void EnableAutoAttack()
    {
        isAbleToAutoAttack = true;
    }

    public void DisableAutoAttack()
    {
        isAbleToAutoAttack = false;
    }

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
            int prevHealth = currentHealth;
            currentHealth -= damage;
            checkBreakpoints(prevHealth);
        }
    }

    private void checkBreakpoints(int healthBeforeDamage)
    {
        if (breakpoints.HasFlag(Breakpoints.break_100))
        {
            breakpoints |= Breakpoints.break_100;
            currentHealth = totalHealth;
            StartFight();
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_75) &&
            (currentHealth <= hp_75 && hp_75 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_75;
            Mechanics.Cascade();
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_66) &&
            (currentHealth <= hp_66 && hp_66 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_66;
            currentHealth = hp_66;
            makeInvulnerable();
            Mechanics.Sacrifice();
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_50) &&
            (currentHealth <= hp_50 && hp_50 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_50;
            Mechanics.Cascade();
            Mechanics.Disintegrate(Pylon_Top); // add a waiting time for the cascade to pass
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_40) &&
            (currentHealth <= hp_40 && hp_40 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_40;
            Mechanics.Disintegrate(Pylon_TopRight);
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_33) &&
            (currentHealth <= hp_33 && hp_33 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_33;
            currentHealth = hp_33;
            makeInvulnerable();
            Mechanics.Sacrifice();
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_30) &&
            (currentHealth <= hp_30 && hp_30 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_30;
            Mechanics.Disintegrate(Pylon_BottomRight);
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_25) &&
            (currentHealth <= hp_25 && hp_25 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_25;
            Mechanics.Cascade();
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_20) &&
            (currentHealth <= hp_20 && hp_20 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_20;
            Mechanics.Disintegrate(Pylon_BottomLeft);
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_10) &&
            (currentHealth <= hp_10 && hp_10 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_10;
            Mechanics.Disintegrate(Pylon_TopLeft);
            return;
        }
        if (breakpoints.HasFlag(Breakpoints.break_0) &&
            (currentHealth <= hp_0 && hp_0 <= healthBeforeDamage))
        {
            breakpoints |= Breakpoints.break_0;
            makeInvulnerable();
            Mechanics.FinalRitual();
            return;
        }
    }
    // Start is called before the first frame update
    public void StartFight()
    {
        tetherTimer = Time.time;
        runesTimer = tetherTimer;
    }

    void Start() {
        // Mechanics.SoulFeast();
        // Mechanics.Cataclysm();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAbleToAutoAttack && !isAttacking)
        {
            isAttacking = true;
            Mechanics.AutoAttack();
        }

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
