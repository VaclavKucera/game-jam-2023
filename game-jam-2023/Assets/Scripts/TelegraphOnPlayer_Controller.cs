using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TelegraphOnPlayerController : TelegraphController
{
    public GameObject Player;

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        gameObject.transform.position = Player.transform.position;

        float progress = elapsedTime / duration;
        if (progress >= 1f)
        {
            TriggerEffect();
        }

        fillerCircle.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Mathf.Clamp(progress, 0, 1));
    }
}
