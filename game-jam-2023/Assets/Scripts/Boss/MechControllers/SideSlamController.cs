using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library;

public class SideSlamController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;

    public GameObject StaticTelegraphPrefab;

    private float sideArmAimDuration = 2f;
    private float sideArmTopDelay = 3f;
    private float sideArmAttackDuration = 0.03f;
    private float sideArmWaitCooldown = 1f;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();

        var mechanics = bossController.Mechanics;

        sideArmAimDuration = mechanics.PoundWindUpDuration;
        sideArmTopDelay = mechanics.PoundWindUpDuration * 0.1f;
        sideArmAttackDuration = mechanics.PoundAttackDuration;
        sideArmWaitCooldown = mechanics.PoundAftercast;
    }

    public void Execute() {
        StartCoroutine(SideSlamSequence());
    }

    public IEnumerator SideSlamSequence() {
        var leftPoint = RandomGeneration.RandomPosition();
        var rightPoint = RandomGeneration.RandomPosition();

        var firstTG = Instantiate(StaticTelegraphPrefab, leftPoint, Quaternion.identity);
        firstTG.GetComponent<TelegraphController>().setTimer(this.sideArmAimDuration);
        var secondTG = Instantiate(StaticTelegraphPrefab, rightPoint, Quaternion.identity);
        secondTG.GetComponent<TelegraphController>().setTimer(this.sideArmAimDuration);

        bossSpritesController.SetArmPositionType(BossSpritesController.ArmPositionType.SideSlam);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Animate to provided points
        bossSpritesController.SetArmMoveDuration(this.sideArmAimDuration);
        StartCoroutine(bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Left, leftPoint));
        StartCoroutine(bossSpritesController.PointArmAtPosition(BossSpritesController.Arm.Right, rightPoint));

        float leftArmAngle = bossSpritesController.armsController.CalculateTargetAngle(transform.position, leftPoint);
        float rightArmAngle = bossSpritesController.armsController.CalculateTargetAngle(transform.position, rightPoint);
        float lookAtAngle = (leftArmAngle + rightArmAngle) / 2 + 90;
        bossSpritesController.LookAtAngle(lookAtAngle);

        // Raise fists mid animation
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Top);

        bossSpritesController.SetIdleMovement(true);

        // Wait until slam
        yield return new WaitForSeconds(this.sideArmTopDelay);

        // Slam down
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAttackDuration);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Cooldown and reset
        yield return new WaitForSeconds(this.sideArmWaitCooldown);
        bossSpritesController.SetArmMoveDuration(0.5f);
        bossSpritesController.SetFistHeight(BossSpritesController.FistHeight.Mid);
        bossSpritesController.SetArmPositionType(BossSpritesController.ArmPositionType.Default);
        bossSpritesController.SetIdleMovement(true);

        yield return new WaitForSeconds(0.5f);

        bossController.Mechanics.OnGroundPoundComplete();
    }
}
