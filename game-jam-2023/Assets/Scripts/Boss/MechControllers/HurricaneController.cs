
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
    }

    public void Execute() {
        Debug.Log("Starting execution of Hurricane");
        StartCoroutine(HurricaneSequence());
    }

    public IEnumerator HurricaneSequence() {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Hurricane sequence completing;");
        bossController.Mechanics.OnHurricaneComplete();
    }
}
