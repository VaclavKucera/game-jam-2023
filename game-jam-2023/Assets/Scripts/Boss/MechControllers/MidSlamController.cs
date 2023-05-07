using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidSlamController : MonoBehaviour
{
    public int damageOnHit = 5;

    private BossSpritesController bossSpritesController;
    private BossController bossController;
    private GameObject player;
    private GameObject sectorHighlight;
    private PolygonCollider2D sectorCollider;

    private float midArmSnapDuration = 1f;
    private float midArmAimDuration = 2f;
    private float midArmTopDelay = 3f;
    private float midArmAttackDuration = 0.03f;
    private float midArmWaitCooldown = 1f;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        sectorHighlight = gameObject.transform.GetChild(0).gameObject;
        sectorCollider = gameObject.GetComponent<PolygonCollider2D>();

        var mechanics = bossController.Mechanics;

        midArmSnapDuration = mechanics.SlamWindUpDuration * 0.3f;
        midArmAimDuration = mechanics.SlamWindUpDuration * 0.7f;
        midArmTopDelay = mechanics.SlamWindUpDuration * 0.5f;
        midArmAttackDuration = mechanics.SlamDuration;
        midArmWaitCooldown = mechanics.SlamAftercast;
    }

    public void Execute() {
        StartCoroutine(MidSlamSequence());
    }

    public IEnumerator MidSlamSequence() {
        yield return StartCoroutine(SlamOnce());
        yield return StartCoroutine(SlamOnce());
        yield return StartCoroutine(SlamOnce());

        bossSpritesController.SetArmMoveDuration(this.midArmSnapDuration);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        bossSpritesController.SetArmPositionType(BossSpritesController.ArmPositionType.Default);
        yield return new WaitForSeconds(this.midArmSnapDuration);

        bossController.Mechanics.OnSlamComplete();
    }

    public IEnumerator SlamOnce() {
        bossSpritesController.SetArmMoveDuration(this.midArmSnapDuration);

        bossSpritesController.SetArmPositionType(BossSpritesController.ArmPositionType.MidSlam);

        // Snap animation for locking onto player
        var playerPosition = player.transform.position;
        StartCoroutine(bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Left, playerPosition));
        StartCoroutine(bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Right, playerPosition));

        float angle = bossSpritesController.armsController.CalculateTargetAngle(transform.position, playerPosition) + 90;
        bossSpritesController.LookAtAngle(angle);

        yield return new WaitForSeconds(this.midArmSnapDuration);


        // Keep following the player for a certain duration
        float elapsedTime = 0;
        while (elapsedTime < this.midArmAimDuration) {
            elapsedTime += Time.deltaTime;
            playerPosition = player.transform.position;

            // bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Left, playerPosition);
            // bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Right, playerPosition);
            bossSpritesController.armsController.leftArm.transform.rotation = bossSpritesController.armsController.CalculateTargetRotation(bossSpritesController.armsController.leftArm.transform.position, playerPosition);
            bossSpritesController.armsController.rightArm.transform.rotation = bossSpritesController.armsController.CalculateTargetRotation(bossSpritesController.armsController.rightArm.transform.position, playerPosition);

            angle = bossSpritesController.armsController.CalculateTargetAngle(transform.position, playerPosition) + 90;

            if (elapsedTime > this.midArmSnapDuration / 2) {
                bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
            }

            bossSpritesController.LookAtAngle(angle);

            yield return null;
        }


        bossSpritesController.SetIdleMovement(false);

        // Wait until slam
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Top);

        float clampedAngle = (angle + 36 + 360) % 360;
        float pentagonAngle = 0;
        if (clampedAngle > 0 && clampedAngle < 72) {
            pentagonAngle = 0;
        } else if (clampedAngle > 72 && clampedAngle < 144) {
            pentagonAngle = 72;
        } else if (clampedAngle > 144 && clampedAngle < 216) {
            pentagonAngle = 144;
        } else if (clampedAngle > 216 && clampedAngle < 288) {
            pentagonAngle = 216;
        } else if (clampedAngle > 288 && clampedAngle < 360) {
            pentagonAngle = 288;
        }

        sectorHighlight.SetActive(true);
        transform.rotation = Quaternion.Euler(0, 0, pentagonAngle + 72 * 3);

        yield return new WaitForSeconds(this.midArmTopDelay);

        // Slam down
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.midArmAttackDuration);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Check if player is in sector
        if (sectorCollider.IsTouching(player.GetComponent<CircleCollider2D>())) {
            player.GetComponent<PlayerController>().TakeDamage(damageOnHit);
        }

        // Wait cooldown
        yield return new WaitForSeconds(this.midArmWaitCooldown);
        sectorHighlight.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        bossSpritesController.SetIdleMovement(true);
    }
}
