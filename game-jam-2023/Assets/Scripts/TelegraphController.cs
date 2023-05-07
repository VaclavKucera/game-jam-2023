using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphController : MonoBehaviour
{
    public float duration = 1f;
    public float elapsedTime = 0f;

    public Transform fillerCircle;
    public Action onCompletion;

    public void SetActionOnCompletion(Action action)
    {
        onCompletion = action;
    }

    public void setTimer(float seconds)
    {
        duration = seconds;
    }

    public virtual void TriggerEffect()
    {
        if (onCompletion != null) {
            onCompletion();
        }
        GameObject.Destroy(gameObject);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / duration;
        if (progress >= 1f)
        {
            TriggerEffect();
        }

        fillerCircle.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Mathf.Clamp(progress, 0, 1));
    }
}
