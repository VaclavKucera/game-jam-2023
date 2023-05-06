using System;
using System.Collections;
using System.Collections.Generic;
using Library;
using UnityEngine;
using static BossController;

public class Mechanics : MonoBehaviour
{
    public BossController BossController;
    public GameObject TotemPrefab;
    public GameObject MinionPrefab;


    private bool AASpecial_iterator = true;
    private enum AutoAttackTypes { Totems, Slam, GroundPound, Hurricane, Special }
    private AutoAttackTypes nextAutoAttack = 0;


    #region Basic Attacks
    public void AutoAttack()
    {
        BossController.isAttacking = true;

        switch (nextAutoAttack)
        {
            case AutoAttackTypes.Totems: SummonTotems(); break;
            case AutoAttackTypes.Slam: Slam(); break;
            case AutoAttackTypes.GroundPound: GroundPound(); break;
            case AutoAttackTypes.Hurricane: Hurricane(); break;
            case AutoAttackTypes.Special:
                if (AASpecial_iterator)
                {
                    Cataclysm();
                    AASpecial_iterator = false;
                }
                else
                {
                    SoulFeast();
                    AASpecial_iterator = true;
                }
                break;
        }
        
        QueueNextAuto();
    }
    private void QueueNextAuto()
    {
        if (nextAutoAttack == AutoAttackTypes.Special)
        {
            nextAutoAttack = 0;
        }
        else nextAutoAttack++;
    }

    public void SummonTotems()
    {
        //place 3 turrets that can be destroyed with Slam attacks
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
    }

    public void Slam()
    {
        // 3 consecutive slam animations; basic attack
    }
    
    public void GroundPound()
    {
        //two points producing shockwaves
    }

    public void Hurricane()
    {
        //3 circular patterns
    }
    #endregion

    #region Special Attacks
    
    public void Cataclysm()
    {
        //bullet hell mech
    }

    public void SoulFeast()
    {
        //spawn adds from the sides of the map crawling to the boss
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
