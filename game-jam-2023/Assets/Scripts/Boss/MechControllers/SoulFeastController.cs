using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library;

public class SoulFeastController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    public GameObject SoulPrefab;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
    }

    public void Execute() {
        Debug.Log("Starting execution of SoulFeast");
        StartCoroutine(SoulFeastSequence());
    }

    public IEnumerator SoulFeastSequence() {
        for (var c = 0; c < 15; c++)
        {
            var interval = RandomGeneration.RandomInterval(0.5f, 7.5f);
            Invoke(nameof(SpawnSoul), interval);
            yield return null;
        }
        yield return new WaitForSeconds(10);
        Debug.Log("All Souls spawned");
        bossController.Mechanics.OnSoulFeastComplete();
    }

    void SpawnSoul()
    {
        Instantiate(SoulPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
    }
}
