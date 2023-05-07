using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmsController : MonoBehaviour
{
    public enum ArmPositionType { Default, SideSlam, MidSlam }
    public enum FistHeight { Top, Mid, Bottom }
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

    private FistHeight fistHeight = FistHeight.Mid;

    private GameObject leftArm;
    private GameObject rightArm;
    private GameObject leftArmOffsetContainer;
    private GameObject rightArmOffsetContainer;
    private BossMainAnimator bossMainAnimator;

    // Start is called before the first frame update
    void Start()
    {
        leftArm = transform.GetChild(0).gameObject;
        rightArm = transform.GetChild(1).gameObject;
        leftArmOffsetContainer = leftArm.transform.GetChild(0).gameObject;
        rightArmOffsetContainer = rightArm.transform.GetChild(0).gameObject;
        bossMainAnimator = transform.parent.GetComponent<BossMainAnimator>();
        SetFistHeight(FistHeight.Mid);

        ConfigureAnimations();
    }

    void ConfigureAnimations()
    {
        Mechanics = bossMainAnimator.BossController.Mechanics;

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

    // Update is called once per frame
    void Update()
    {
    }

    public void RunSideSlamAnimation(Vector2 leftPoint, Vector2 rightPoint)
    {
        StartCoroutine(SideSlamSequence(leftPoint, rightPoint));
    }

    public void RunMidSlamAnimation(int count)
    {
        StartCoroutine(MidSlamSequence(count));
    }

    public void ResetToDefaultPosition() {
        SetArmPositionType(ArmPositionType.Default);
    }

    IEnumerator PointArmAtPosition(GameObject arm, Vector2 targetPoint)
    {
        Quaternion initialRotation = arm.transform.rotation;
        Quaternion targetRotation = CalculateTargetRotation(arm.transform.position, targetPoint);
        float elapsedTime = 0;

        while (elapsedTime < armMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / armMoveDuration;

            arm.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, progress);

            yield return null;
        }
    }

    IEnumerator PointArmAtRotation(GameObject arm, Quaternion targetRotation)
    {
        Quaternion initialRotation = arm.transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < armMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / armMoveDuration;

            arm.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, progress);

            yield return null;
        }
    }

    private Quaternion CalculateTargetRotation(Vector3 armPosition, Vector2 targetPoint)
    {
        Vector2 direction = targetPoint - (Vector2)armPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void SetArmPositionType(ArmPositionType type)
    {
        Vector3 leftArmTargetPosition, rightArmTargetPosition;
        Quaternion leftArmTargetRotation, rightArmTargetRotation;

        switch (type)
        {
            case ArmPositionType.Default:
                leftArmTargetPosition = new Vector3(0.84f, -0.5f, 0);
                leftArmTargetRotation = Quaternion.Euler(0, 0, -14f);
                rightArmTargetPosition = new Vector3(-0.84f, -0.5f, 0);
                rightArmTargetRotation = Quaternion.Euler(0, 0, 14f);

                Quaternion armRotationMidpoint =  Quaternion.Slerp(leftArm.transform.rotation, rightArm.transform.rotation , 0.5f);
                StartCoroutine(PointArmAtRotation(leftArm, armRotationMidpoint));
                StartCoroutine(PointArmAtRotation(rightArm, armRotationMidpoint));
                break;
            case ArmPositionType.SideSlam:
                leftArmTargetPosition = new Vector3(0, -1.7f, 0);
                leftArmTargetRotation = Quaternion.Euler(0, 0, 0);
                rightArmTargetPosition = new Vector3(0, -1.7f, 0);
                rightArmTargetRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ArmPositionType.MidSlam:
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

    IEnumerator MoveArmOffsetToTargetPosition(GameObject arm, Vector3 targetPosition, Quaternion targetRotation)
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

    private float CalculateTargetAngle(Vector3 position, Vector2 targetPoint)
    {
        Vector2 direction = targetPoint - (Vector2)position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void SetFistHeight(FistHeight height) {
        this.fistHeight = height;

        var leftFist = leftArmOffsetContainer.transform.GetChild(1).gameObject;
        var rightFist = rightArmOffsetContainer.transform.GetChild(1).gameObject;

        leftFist.transform.GetChild(0).gameObject.SetActive(height == FistHeight.Bottom);
        rightFist.transform.GetChild(0).gameObject.SetActive(height == FistHeight.Bottom);
        leftFist.transform.GetChild(1).gameObject.SetActive(height == FistHeight.Mid);
        rightFist.transform.GetChild(1).gameObject.SetActive(height == FistHeight.Mid);
        leftFist.transform.GetChild(2).gameObject.SetActive(height == FistHeight.Top);
        rightFist.transform.GetChild(2).gameObject.SetActive(height == FistHeight.Top);
    }

    private IEnumerator MidSlamSequence(int count) {
        this.armMoveDuration = this.midArmSnapDuration;
        SetArmPositionType(ArmPositionType.MidSlam);

        // Snap animation for locking onto player
        var playerPosition = player.transform.position;
        StartCoroutine(PointArmAtPosition(leftArm, playerPosition));
        StartCoroutine(PointArmAtPosition(rightArm, playerPosition));

        float angle = CalculateTargetAngle(transform.position, playerPosition) + 90;
        bossMainAnimator.LookAtAngle(angle);

        yield return new WaitForSeconds(this.midArmSnapDuration);

        // Keep following player for a certain duration
        float elapsedTime = 0;
        while (elapsedTime < this.midArmAimDuration) {
            elapsedTime += Time.deltaTime;
            playerPosition = player.transform.position;
            angle = CalculateTargetAngle(transform.position, playerPosition) + 90;
            leftArm.transform.rotation = CalculateTargetRotation(leftArm.transform.position, playerPosition);
            rightArm.transform.rotation = CalculateTargetRotation(rightArm.transform.position, playerPosition);

            if (elapsedTime > this.midArmSnapDuration / 2 && this.fistHeight != FistHeight.Mid) {
                SetFistHeight(FistHeight.Mid);
            }

            bossMainAnimator.LookAtAngle(angle);

            yield return null;
        }

        // Wait until slam
        SetFistHeight(FistHeight.Top);
        yield return new WaitForSeconds(this.midArmTopDelay);

        // Slam down
        SetFistHeight(FistHeight.Mid);
        yield return new WaitForSeconds(this.midArmAttackDuration);
        SetFistHeight(FistHeight.Bottom);

        // Wait and return to default
        yield return new WaitForSeconds(this.midArmWaitCooldown);
        if (count == 0)
        {
            SetFistHeight(FistHeight.Mid);
            SetArmPositionType(ArmPositionType.Default);
            yield return new WaitForSeconds(armMoveDuration);
        }
        bossMainAnimator.SlamAttack(count);
    }

    private IEnumerator SideSlamSequence(Vector2 leftPoint, Vector2 rightPoint) {
        SetArmPositionType(ArmPositionType.SideSlam);
        SetFistHeight(FistHeight.Bottom);

        // Animate to provided points
        this.armMoveDuration = this.sideArmAimDuration;
        StartCoroutine(PointArmAtPosition(leftArm, leftPoint));
        StartCoroutine(PointArmAtPosition(rightArm, rightPoint));

        float leftArmAngle = CalculateTargetAngle(transform.position, leftPoint);
        float rightArmAngle = CalculateTargetAngle(transform.position, rightPoint);
        float lookAtAngle = (leftArmAngle + rightArmAngle) / 2 + 90;
        bossMainAnimator.LookAtAngle(lookAtAngle);

        // Raise fists mid animation
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        SetFistHeight(FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAimDuration / 2);
        SetFistHeight(FistHeight.Top);

        // Wait until slam
        yield return new WaitForSeconds(this.sideArmTopDelay);

        // Slam down
        SetFistHeight(FistHeight.Mid);
        yield return new WaitForSeconds(this.sideArmAttackDuration);
        SetFistHeight(FistHeight.Bottom);

        // Cooldown and reset
        yield return new WaitForSeconds(this.sideArmWaitCooldown);
        SetFistHeight(FistHeight.Mid);
        SetArmPositionType(ArmPositionType.Default);
        yield return new WaitForSeconds(armMoveDuration);
    }
}