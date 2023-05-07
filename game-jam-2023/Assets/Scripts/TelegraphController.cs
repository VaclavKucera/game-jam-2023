using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphController : MonoBehaviour
{
    public float timeToFill = 1f;
    public Transform fillerCircle;
    public Action onCompletion;

    public void SetActionOnCompletion(Action action)
    {
        onCompletion = action;
    }

    public void setTimer(float seconds)
    {
        timeToFill = seconds;
    }

    public virtual void TriggerEffect()
    {
        if (onCompletion != null)
            onCompletion();
        GameObject.Destroy(gameObject);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (fillerCircle.localScale == new Vector3(1,1,1))
        {
            TriggerEffect();
        }
        fillerCircle.localScale = Vector3.MoveTowards(fillerCircle.localScale, new Vector3(1, 1, 1), timeToFill * Time.deltaTime);
    }
}
