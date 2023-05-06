using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TelegraphOnPlayerController : TelegraphController
{
    public GameObject Player;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        if (fillerCircle.localScale == new Vector3(1,1,1))
        {
            TriggerEffect();
        }
        gameObject.transform.position = Player.transform.position;
        fillerCircle.localScale = Vector3.MoveTowards(fillerCircle.localScale, new Vector3(1, 1, 1), timeToFill * Time.deltaTime);
    }
}
