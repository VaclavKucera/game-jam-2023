using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library;

public class SideSlamController : MonoBehaviour
{
    private BossSpritesController bossSpritesController;
    private BossController bossController;
    private GameObject player;

    public GameObject StaticTelegraphPrefab;
    public GameObject SideSlamWavePrefab;

    public int directSlamDamage = 30;
    public int shockwaveDamage = 5;

    private float sideArmAimDuration = 2f;
    private float sideArmTopDelay = 3f;
    private float sideArmAttackDuration = 0.03f;
    private float sideArmWaitCooldown = 1f;

    void Start()
    {
        bossSpritesController = GameObject.FindGameObjectWithTag("BossSpritesController").GetComponent<BossSpritesController>();
        bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player");

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

        // Spawn shockwaves
        var wave1 = Instantiate(SideSlamWavePrefab, leftPoint, Quaternion.identity);
        var wave2 = Instantiate(SideSlamWavePrefab, rightPoint, Quaternion.identity);
        wave1.GetComponent<SideSlamParticleSystemController>().waveDamage = shockwaveDamage;
        wave2.GetComponent<SideSlamParticleSystemController>().waveDamage = shockwaveDamage;
        wave1.GetComponent<ParticleSystem>().trigger.SetCollider(0, player.GetComponent<Collider2D>());
        wave2.GetComponent<ParticleSystem>().trigger.SetCollider(0, player.GetComponent<Collider2D>());


        // Check if player is within 5 units of either point
        var playerPos = (Vector2)player.transform.position;
        if ((playerPos - leftPoint).magnitude < 0.5f  || (playerPos - rightPoint).magnitude < 0.5f) {
            Debug.Log("Hit by side slam");
            player.GetComponent<PlayerController>().TakeDamage(directSlamDamage);
        }

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
