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
    public BossController bossController;
    public GameObject TelegraphOnPlayer;
    public GameObject Rune;

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
    private Transform player;

    void Start() {
        if (bossController == null)
            bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        else
            bossController = bossController.GetComponent<BossController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

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
            nextAutoAttack = AutoAttackTypes.Special;
        }
        else if (nextAutoAttack == AutoAttackTypes.Special)
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
        cataclysmController.Execute();
    }
    public void OnCataclysmComplete()
    {
        bossController.isAttacking = false;
    }

    public void SoulFeast()
    {
        soulFeastController.Execute();
    }
    public void OnSoulFeastComplete()
    {
        bossController.isAttacking = false;
    }

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
        StartCoroutine(SpawnRunes());
    }
    IEnumerator SpawnRunes()
    {
        for (int i = 5; i > 0; i--)
        {
            var circleController = Instantiate(TelegraphOnPlayer).GetComponent<TelegraphOnPlayerController>();
            circleController.onCompletion = () => { Instantiate(Rune, circleController.transform.position, Quaternion.identity); };
            yield return new WaitForSeconds(1);
        }
        
    }
    #endregion
}
