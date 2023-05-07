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
        StartCoroutine(CataclysmSequence());
    }

    public IEnumerator CataclysmSequence() {
        // Cracks connect

        // Shoots 3x3
        var offset = 36f;
        var pentagonAngle = 72f;

        for (int i = 0; i <= 4; i++)
        {
            yield return StartCoroutine(CataclysmBullets(offset + i * pentagonAngle));
        }

        // Beam attack
        bossController.Mechanics.OnCataclysmComplete();
    }

    private IEnumerator CataclysmBullets(float angle)
    {
        yield return TripleBullet(angle);
        yield return new WaitForSeconds(1f);
        yield return TripleBullet(angle);
        yield return new WaitForSeconds(1f);
        yield return TripleBullet(angle);
    }

    private IEnumerator TripleBullet(float angle)
    {
        Debug.Log($"Triple bullet {angle}");
        var position = transform.position;
        var start = position + Vector3.up;

        var shiftedPoint = start - position;
        var rotation = Quaternion.Euler(Vector3.forward * angle);
        shiftedPoint = rotation * shiftedPoint;
        var rotatedPoint = shiftedPoint + position;

        var spread = 20f;
        var angle1 = Quaternion.AngleAxis(angle + spread, Vector3.forward);
        var angle2 = Quaternion.AngleAxis(angle, Vector3.forward);
        var angle3 = Quaternion.AngleAxis(angle - spread, Vector3.forward);


        Instantiate(BulletPrefab, rotatedPoint, angle1);
        Instantiate(BulletPrefab, rotatedPoint, angle2);
        Instantiate(BulletPrefab, rotatedPoint, angle3);

        yield return null;
    }    
}
