using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
    }

    public void Execute() {
        Debug.Log("Starting execution of Cascade");
        StartCoroutine(CascadeSequence());
    }

    public IEnumerator CascadeSequence() {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Cascade sequence completing;");
        bossController.Mechanics.OnCascadeComplete();
    }
}
