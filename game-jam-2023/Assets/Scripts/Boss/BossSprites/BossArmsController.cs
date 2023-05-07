using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmsController : MonoBehaviour
{
    public float armMoveDuration = 1.0f;
    public GameObject player;

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
}