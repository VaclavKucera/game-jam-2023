using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Library;
using UnityEngine;
using static BossController;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
    private Transform player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

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
        Debug.Log("Cataclysm");
        // Cracks connect

        // Shoots 3x3
        StartCoroutine(CataclysmBullets(180));
        StartCoroutine(CataclysmBullets(90));
        StartCoroutine(CataclysmBullets(-90));

        // Beam attack
    }

    private void FollowPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
    }

    private IEnumerator CataclysmBullets(float angle)
    {
        Debug.Log("Cataclysm bullets");
        yield return TripleBullet(angle);
        yield return new WaitForSeconds(1f);
        yield return TripleBullet(angle);
        yield return new WaitForSeconds(1f);
        yield return TripleBullet(angle);
    }

    public IEnumerator TripleBullet(float angle)
    {
        var position = transform.position;
        var start = position + Vector3.up;

        var shiftedPoint = start - position;
        var rotation = Quaternion.Euler(Vector3.forward * angle);
        shiftedPoint = rotation * shiftedPoint;
        var rotatedPoint = shiftedPoint + position;

        var spread = 20f;
        var angle1 = Quaternion.AngleAxis(angle + spread, Vector3.forward);
        var angle2 = Quaternion.AngleAxis(angle, Vector3.forward);
        var angle3 = Quaternion.AngleAxis(angle - spread, Vector3.forward);


        Instantiate(BulletPrefab, rotatedPoint, angle1);
        Instantiate(BulletPrefab, rotatedPoint, angle2);
        Instantiate(BulletPrefab, rotatedPoint, angle3);

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
