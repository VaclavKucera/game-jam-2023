using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidSlamController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;
    private GameObject player;

    private float midArmSnapDuration = 1f;
    private float midArmAimDuration = 2f;
    private float midArmTopDelay = 3f;
    private float midArmAttackDuration = 0.03f;
    private float midArmWaitCooldown = 1f;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        var mechanics = bossController.Mechanics;

        midArmSnapDuration = mechanics.SlamWindUpDuration * 0.3f;
        midArmAimDuration = mechanics.SlamWindUpDuration * 0.7f;
        midArmTopDelay = mechanics.SlamWindUpDuration * 0.2f;
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
        yield return new WaitForSeconds(this.midArmTopDelay);

        // Slam down
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.midArmAttackDuration);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Wait cooldown
        yield return new WaitForSeconds(this.midArmWaitCooldown);
        bossSpritesController.SetIdleMovement(true);
    }
}
