using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSlamParticleSystemController : MonoBehaviour
{
    public int waveDamage = 5;

    private bool hasDoneDamage = false;
    private GameObject player;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, 5f);
    }

    void OnParticleTrigger()
    {
        if (!hasDoneDamage && !player.GetComponent<Movement>().IsDashing()) {
            Debug.Log("Hit by shockwave");
            player.GetComponent<PlayerController>().TakeDamage(waveDamage);
            hasDoneDamage = true;
        }
    }
}
