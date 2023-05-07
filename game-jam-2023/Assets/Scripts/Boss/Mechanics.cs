using System;
using System.Collections;
using System.Collections.Generic;
using Library;
using UnityEngine;
using static BossController;

public class Mechanics : MonoBehaviour
{
    public BossController bossController;

    public GameObject MinionPrefab;
    public GameObject BulletPrefab;

    [Header("Configuration")]
    public float SlamWindUpDuration = 1f;
    public float SlamDuration = 0.03f;
    public float SlamAftercast = 0.5f;

    public float PoundWindUpDuration = 2f;
    public float PoundAttackDuration = 0.05f;
    public float PoundAftercast = 1.5f;

    [Header("Mechanic controllers")]
    public CascadeController cascadeController;
    public MidSlamController midSlamController;
    public SideSlamController sideSlamController;
    public SummonTotemsController summonTotemsController;
    public HurricaneController hurricaneController;
    public CataclysmController cataclysmController;
    public SoulFeastController soulFeastController;

    private bool AASpecial_iterator = true;
    private enum AutoAttackTypes { Totems, Slam, GroundPound, Hurricane, Special }
    private AutoAttackTypes nextAutoAttack = 0;

    void Start() {
        if (bossController == null)
            bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
        else
            bossController = bossController.GetComponent<BossController>();
    }

    public void AutoAttack()
    {
        bossController.isAttacking = true;

        Debug.Log("Auto-attack started: " + nextAutoAttack);
        switch (nextAutoAttack)
        {
            case AutoAttackTypes.Totems: SummonTotems(); break;
            case AutoAttackTypes.Slam: Slam(); break;
            case AutoAttackTypes.GroundPound: GroundPound(); break;
            case AutoAttackTypes.Hurricane: Hurricane(); break;
            case AutoAttackTypes.Special:
                if (AASpecial_iterator)
                    Cataclysm();
                else
                    SoulFeast();
                AASpecial_iterator = !AASpecial_iterator;
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
            nextAutoAttack = 0;
        }
        else nextAutoAttack++;
    }

    #region Basic Attacks
    public void SummonTotems()
    {
        summonTotemsController.Execute();
    }
    public void OnSummonTotemsComplete()
    {
        bossController.isAttacking = false;
    }

    public void Slam()
    {
        midSlamController.Execute();
    }
    public void OnSlamComplete()
    {
        bossController.isAttacking = false;
    }

    public void GroundPound()
    {
        sideSlamController.Execute();
    }
    public void OnGroundPoundComplete()
    {
        bossController.isAttacking = false;
    }

    public void Hurricane()
    {
        Debug.Log("Hurricane start");
        hurricaneController.Execute();
        Debug.Log("Hurricane end");
    }
    public void OnHurricaneComplete()
    {
        Debug.Log("Hurricane End - async, executing next attack");
        bossController.isAttacking = false;
    }

    #endregion

    #region Special Attacks

    public void Cataclysm()
    {
        Debug.Log("Cataclysm start");
        cataclysmController.Execute();
        Debug.Log("Cataclysm end");
    }
    public void OnCataclysmComplete()
    {
        Debug.Log("Cataclysm End - async, executing next attack");
        bossController.isAttacking = false;
    }

    public void SoulFeast()
    {
        Debug.Log("SoulFeast start");
        soulFeastController.Execute();
        Debug.Log("SoulFeast end");
    }
    public void OnSoulFeastComplete()
    {
        Debug.Log("SoulFeast End - async, executing next attack");
        bossController.isAttacking = false;
    }

    //bullet hell mech
    // public void Cataclysm()
    // {
    //     Debug.Log("Cataclysm start");
    //     // Cracks connect

    //     // Shoots 3x3
    //     // Turn to player
    //     // 3x coroutine
    //     StartCoroutine(TripleBullet());

    //     // Beam attack
    // }

    // public IEnumerator TripleBullet()
    // {
    //     var spread = Quaternion.AngleAxis(10, transform.position);
    //     var spread2 = Quaternion.AngleAxis(-10, transform.position);

    //     Instantiate(BulletPrefab, transform.position, transform.rotation * spread);
    //     Instantiate(BulletPrefab, transform.position, transform.rotation);
    //     Instantiate(BulletPrefab, transform.position, transform.rotation * spread2);

    //     yield return null;
    // }

    // public void SoulFeast()
    // {
    //     //spawn adds from the sides of the map crawling to the boss
    //     Instantiate(MinionPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
    //     Instantiate(MinionPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
    // }
    #endregion

    #region HP Based Attacks

    public void Cascade()
    {
        Debug.Log("Cascade start");
        cascadeController.Execute();
        Debug.Log("Cascade end");
    }
    public void OnCascadeComplete()
    {
        Debug.Log("Cascade End - async");
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
