
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFeastController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
    }

    public void Execute() {
        Debug.Log("Starting execution of SoulFeast");
        StartCoroutine(SoulFeastSequence());
    }

    public IEnumerator SoulFeastSequence() {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("SoulFeast sequence completing;");
        bossController.Mechanics.OnSoulFeastComplete();
    }
}
