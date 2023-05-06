using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    public BossController BossController;


    #region Basic Attacks

    public void SpawnTurrets()
    {
        //place 3 turrets that can be destroyed with Slam attacks
    }

    public void Slam()
    {
        //basic attack
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

    public void Disintegrate()
    {
        //launch a projectile at the pylons
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
