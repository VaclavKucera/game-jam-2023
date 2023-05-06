using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphController : MonoBehaviour
{
    public float timeToFill = 1f;
    public GameObject Player;
    public Transform fillerCircle;

    public void onComplete()
    {
        GameObject.Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("Awake");

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        if (fillerCircle.localScale == new Vector3(1,1,1))
        {
            onComplete();
        }
        gameObject.transform.position = Player.transform.position;
        fillerCircle.localScale = Vector3.MoveTowards(fillerCircle.localScale, new Vector3(1, 1, 1), timeToFill * Time.deltaTime);
    }
}
