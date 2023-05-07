using System;
using System.Collections;
using System.Collections.Generic;
using Library;
using UnityEngine;
using static BossController;

public class Mechanics : MonoBehaviour
{
    public BossController BossController;
    public BossMainAnimator mainAnimator;
    public GameObject TotemPrefab;
    public GameObject SoulPrefab;
    public GameObject BulletPrefab;

    [Header("Configuration")]
    public float SlamWindUpDuration = 1f;
    public float SlamDuration = 0.03f;
    public float SlamAftercast = 0.5f;

    public float PoundWindUpDuration = 2f;
    public float PoundAttackDuration = 0.05f;
    public float PoundAftercast = 1.5f;


    private bool AASpecial_iterator = true;
    private enum AutoAttackTypes { Totems, Slam, GroundPound, Hurricane, Special }
    private AutoAttackTypes nextAutoAttack = 0;


    #region Basic Attacks
    public void AutoAttack()
    {
        Debug.Log("Auto-attack started: " + nextAutoAttack);
        switch (nextAutoAttack)
        {
            case AutoAttackTypes.Totems: SummonTotems(); break;
            case AutoAttackTypes.Slam: Slam(); break;
            case AutoAttackTypes.GroundPound: GroundPound(); break;
            case AutoAttackTypes.Hurricane: Hurricane(); break;
            case AutoAttackTypes.Special:
                /* if (AASpecial_iterator)
                 {
                     Cataclysm();
                     AASpecial_iterator = false;
                 }
                 else*/
                {
                    SoulFeast();
                    AASpecial_iterator = true;
                }
                break;
        }
        Debug.Log("We should still have: " + nextAutoAttack);
        QueueNextAuto();
    }
    private void QueueNextAuto()
    {
        //TODO: add all available autos
        if (nextAutoAttack == AutoAttackTypes.GroundPound)
        {
            nextAutoAttack = AutoAttackTypes.Special;
        }
        else if (nextAutoAttack == AutoAttackTypes.Special)
        {
            nextAutoAttack = 0;
        }
        else nextAutoAttack++;
    }

    public void SummonTotems()
    {
        Debug.Log("Summon start");
        StartCoroutine(SummonTotemsCoroutine());
        Debug.Log("Summon end");
        //place 3 turrets that can be destroyed with Slam attacks
    }
    IEnumerator SummonTotemsCoroutine()
    {
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);
        BossController.onAutoattackAnimationComplete();
        Debug.Log("End of Totem Coro");
    }

    public void Slam()
    {
        BossController.isAttacking = true;
        mainAnimator.SlamAttack(3);
    }
    
    public void GroundPound()
    {
        var left = RandomGeneration.RandomPosition();
        var right = RandomGeneration.RandomPosition();
        mainAnimator.GroundPoundAttack(left, right);
    }

    public void Hurricane()
    {
        //3 circular patterns
    }
    #endregion

    #region Special Attacks

    //bullet hell mech
    public void Cataclysm()
    {
        Debug.Log("Cataclysm start");
        // Cracks connect

        // Shoots 3x3
        // Turn to player
        // 3x coroutine
        StartCoroutine(TripleBullet());

        // Beam attack
    }

    public IEnumerator TripleBullet()
    {
        var spread = Quaternion.AngleAxis(10, transform.position);
        var spread2 = Quaternion.AngleAxis(-10, transform.position);

        Instantiate(BulletPrefab, transform.position, transform.rotation * spread);
        Instantiate(BulletPrefab, transform.position, transform.rotation);
        Instantiate(BulletPrefab, transform.position, transform.rotation * spread2);

        yield return null;
    }

    public void SoulFeast()
    {
        Debug.Log("SoulFeast start");
        BossController.isAbleToAutoAttack = false;
        StartCoroutine(SpawnSouls());
        Debug.Log("SoulFeast end");
    }

    IEnumerator SpawnSouls()
    {
        for (var c = 0; c < 15; c++)
        {
            var interval = RandomGeneration.RandomInterval(0.5f, 1.5f);
            Invoke(nameof(SpawnSoul), interval);
            yield return null;
        }
        yield return new WaitForSeconds(10);
        BossController.isAbleToAutoAttack = true;
        BossController.onAutoattackAnimationComplete();
        Debug.Log("All Souls spawned");
    }
    void SpawnSoul()
    {
        Instantiate(SoulPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
    }
#endregion

#region HP Based Attacks

public void Cascade()
    {
        //spawn cascade
    }

    public void Sacrifice()
    {
        //spawn adds, boss invuln
    }

    public void Disintegrate(GameObject pylon)
    {
        //launch a projectile at the pylons
    }

    public void FinalRitual()
    {

    }
    #endregion

    #region Time Based Attacks

    public void Tether()
    {
        //do something
    }

    public void Runes()
    {
        //do something
    }
    #endregion
}
