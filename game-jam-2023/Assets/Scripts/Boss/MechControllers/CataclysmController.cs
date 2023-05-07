using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CataclysmController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
    }

    public void Execute() {
        Debug.Log("Starting execution of Cataclysm");
        StartCoroutine(CataclysmSequence());
    }

    public IEnumerator CataclysmSequence() {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Cataclysm sequence completing;");
        bossController.Mechanics.OnCataclysmComplete();
    }
}
