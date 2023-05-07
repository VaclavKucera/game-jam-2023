using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library;

public class SummonTotemsController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    public GameObject TotemPrefab;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
    }

    public void Execute() {
        StartCoroutine(SummonTotemsSequence());
    }

    public IEnumerator SummonTotemsSequence() {
        //place 3 turrets that can be destroyed with Slam attacks
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Instantiate(TotemPrefab, RandomGeneration.RandomPosition(), Quaternion.identity);
        yield return new WaitForSeconds(1);

        bossController.Mechanics.OnSummonTotemsComplete();
    }
}
