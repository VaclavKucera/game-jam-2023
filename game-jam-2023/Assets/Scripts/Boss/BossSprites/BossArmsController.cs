using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmsController : MonoBehaviour
{
    public float armMoveDuration = 1.0f;

    public GameObject player;

    private Mechanics Mechanics;

    // TODO: Might wanna take this from some other common place
    private float midArmSnapDuration = 1f;
    private float midArmAimDuration = 2f;
    private float midArmTopDelay = 3f;
    private float midArmAttackDuration = 0.03f;
    private float midArmWaitCooldown = 1f;

    private float sideArmAimDuration = 2f;
    private float sideArmTopDelay = 3f;
    private float sideArmAttackDuration = 0.03f;
    private float sideArmWaitCooldown = 1f;

    private BossSpritesController.FistHeight fistHeight = BossSpritesController.FistHeight.Mid;

    public GameObject leftArm;
    public GameObject rightArm;
    private GameObject leftArmOffsetContainer;
    private GameObject rightArmOffsetContainer;
    private BossSpritesController bossSpritesController;

    // Start is called before the first frame update
    void Start()
    {
        leftArm = transform.GetChild(0).gameObject;
        rightArm = transform.GetChild(1).gameObject;
        leftArmOffsetContainer = leftArm.transform.GetChild(0).gameObject;
        rightArmOffsetContainer = rightArm.transform.GetChild(0).gameObject;
        bossSpritesController = transform.parent.GetComponent<BossSpritesController>();
        SetFistHeight(BossSpritesController.FistHeight.Mid);

        ConfigureAnimations();
    }

    void ConfigureAnimations()
    {
        Mechanics = bossSpritesController.bossController.Mechanics;

        midArmSnapDuration = Mechanics.SlamWindUpDuration * 0.3f;
        midArmAimDuration = Mechanics.SlamWindUpDuration * 0.7f;
        midArmTopDelay = Mechanics.SlamWindUpDuration * 0.2f;
        midArmAttackDuration = Mechanics.SlamDuration;
        midArmWaitCooldown = Mechanics.SlamAftercast;

        sideArmAimDuration = Mechanics.PoundWindUpDuration;
        sideArmTopDelay = Mechanics.PoundWindUpDuration * 0.1f;
        sideArmAttackDuration = Mechanics.PoundAttackDuration;
        sideArmWaitCooldown = Mechanics.PoundAftercast;
    }

    public IEnumerator PointArmAtPosition(BossSpritesController.Arm arm, Vector2 targetPoint)
    {
        var armObject = arm == BossSpritesController.Arm.Left ? leftArm : rightArm;

        Quaternion initialRotation = armObject.transform.rotation;
        Quaternion targetRotation = CalculateTargetRotation(armObject.transform.position, targetPoint);
        float elapsedTime = 0;

        while (elapsedTime < armMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp(elapsedTime / armMoveDuration, 0, 1);

            armObject.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, progress);

            yield return null;
        }
    }

    public IEnumerator PointArmAtRotation(BossSpritesController.Arm arm, Quaternion targetRotation)
    {
        var armObject = arm == BossSpritesController.Arm.Left ? leftArm : rightArm;

        Quaternion initialRotation = armObject.transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < armMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp(elapsedTime / armMoveDuration, 0, 1);

            armObject.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, progress);

            yield return null;
        }
    }

    /** Executes async, duration equal to armMoveDuration */
    public void SetArmPositionType(BossSpritesController.ArmPositionType type)
    {
        Vector3 leftArmTargetPosition, rightArmTargetPosition;
        Quaternion leftArmTargetRotation, rightArmTargetRotation;

        switch (type)
        {
            case BossSpritesController.ArmPositionType.Default:
                leftArmTargetPosition = new Vector3(0.84f, -0.5f, 0);
                leftArmTargetRotation = Quaternion.Euler(0, 0, -14f);
                rightArmTargetPosition = new Vector3(-0.84f, -0.5f, 0);
                rightArmTargetRotation = Quaternion.Euler(0, 0, 14f);

                Quaternion armRotationMidpoint =  Quaternion.Slerp(leftArm.transform.rotation, rightArm.transform.rotation , 0.5f);
                StartCoroutine(PointArmAtRotation(BossSpritesController.Arm.Left, armRotationMidpoint));
                StartCoroutine(PointArmAtRotation(BossSpritesController.Arm.Right, armRotationMidpoint));
                bossSpritesController.LookAtAngle(armRotationMidpoint.eulerAngles.z);
                break;
            case BossSpritesController.ArmPositionType.SideSlam:
                leftArmTargetPosition = new Vector3(0, -1.7f, 0);
                leftArmTargetRotation = Quaternion.Euler(0, 0, 0);
                rightArmTargetPosition = new Vector3(0, -1.7f, 0);
                rightArmTargetRotation = Quaternion.Euler(0, 0, 0);
                break;
            case BossSpritesController.ArmPositionType.MidSlam:
                leftArmTargetPosition = new Vector3(0.5f, -0.86f, 0);
                leftArmTargetRotation = Quaternion.Euler(0, 0, -40.14f);
                rightArmTargetPosition = new Vector3(-0.5f, -0.86f, 0);
                rightArmTargetRotation = Quaternion.Euler(0, 0, 40.14f);
                break;
            default:
                return;
        }

        StartCoroutine(MoveArmOffsetToTargetPosition(leftArmOffsetContainer, leftArmTargetPosition, leftArmTargetRotation));
        StartCoroutine(MoveArmOffsetToTargetPosition(rightArmOffsetContainer, rightArmTargetPosition, rightArmTargetRotation));
    }

    public void SetFistHeight(BossSpritesController.FistHeight height) {
        if (this.fistHeight == height) return;

        this.fistHeight = height;

        var leftFist = leftArmOffsetContainer.transform.GetChild(1).gameObject;
        var rightFist = rightArmOffsetContainer.transform.GetChild(1).gameObject;

        leftFist.transform.GetChild(0).gameObject.SetActive(height == BossSpritesController.FistHeight.Bottom);
        rightFist.transform.GetChild(0).gameObject.SetActive(height == BossSpritesController.FistHeight.Bottom);
        leftFist.transform.GetChild(1).gameObject.SetActive(height == BossSpritesController.FistHeight.Mid);
        rightFist.transform.GetChild(1).gameObject.SetActive(height == BossSpritesController.FistHeight.Mid);
        leftFist.transform.GetChild(2).gameObject.SetActive(height == BossSpritesController.FistHeight.Top);
        rightFist.transform.GetChild(2).gameObject.SetActive(height == BossSpritesController.FistHeight.Top);
    }

    public Quaternion CalculateTargetRotation(Vector3 armPosition, Vector2 targetPoint)
    {
        Vector2 direction = targetPoint - (Vector2)armPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator MoveArmOffsetToTargetPosition(GameObject arm, Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 initialPosition = arm.transform.localPosition;
        Quaternion initialRotation = arm.transform.localRotation;
        float elapsedTime = 0;

        while (elapsedTime < armMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / armMoveDuration;

            arm.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, progress);
            arm.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, progress);

            yield return null;
        }
    }

    public float CalculateTargetAngle(Vector3 position, Vector2 targetPoint)
    {
        Vector2 direction = targetPoint - (Vector2)position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // TODO: This needs to move

    private IEnumerator SideSlamSequence(Vector2 leftPoint, Vector2 rightPoint) {
        SetArmPositionType(BossSpritesController.ArmPositionType.SideSlam);
        SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Animate to provided points
        this.armMoveDuration = this.sideArmAimDuration;
        StartCoroutine(PointArmAtPosition(BossSpritesController.Arm.Left, leftPoint));
        StartCoroutine(PointArmAtPosition(BossSpritesController.Arm.Right, rightPoint));

        float leftArmAngle = CalculateTargetAngle(transform.position, leftPoint);
        float rightArmAngle = CalculateTargetAngle(transform.position, rightPoint);
        float lookAtAngle = (leftArmAngle + rightArmAngle) / 2 + 90;
        bossSpritesController.LookAtAngle(lookAtAngle);

        // Raise fists mid animation
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        SetFistHeight(BossSpritesController.FistHeight.Top);

        // Wait until slam
        yield return new WaitForSeconds(this.sideArmTopDelay);

        // Slam down
        SetFistHeight(BossSpritesController.FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAttackDuration);
        SetFistHeight(BossSpritesController.FistHeight.Bottom);

        // Cooldown and reset
        yield return new WaitForSeconds(this.sideArmWaitCooldown);
        SetFistHeight(BossSpritesController.FistHeight.Mid);
        SetArmPositionType(BossSpritesController.ArmPositionType.Default);
        yield return new WaitForSeconds(armMoveDuration);
    }
}