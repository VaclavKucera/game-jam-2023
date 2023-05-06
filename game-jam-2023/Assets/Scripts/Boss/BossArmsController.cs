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

    // TODO: Might wanna take this from some other common place
    private float midArmAimDuration = 0.3f;
    private float midArmTopDelay = 3f;
    private float midArmAttackDuration = 0.03f;
    private float midArmWaitCooldown = 1f;

    private float sideArmAimDuration = 2f;
    private float sideArmTopDelay = 3f;
    private float sideArmAttackDuration = 0.03f;
    private float sideArmWaitCooldown = 1f;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary key-based position setting
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     SetArmPositionType(ArmPositionType.Default);
        // }
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     SetArmPositionType(ArmPositionType.SideSlam);
        // }
        // else if (Input.GetKeyDown(KeyCode.M))
        // {
        //     SetArmPositionType(ArmPositionType.MidSlam);
        // }
        if (Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(MidSlamSequence());
        }

        // Trigger RunSideSlamAnimation when a mouse button is clicked
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)) {
                RunMidSlamAnimation(mousePosition);
            } else {
                RunSideSlamAnimation(mousePosition, new Vector2(10, 0));
            }
        }
    }

    public void RunSideSlamAnimation(Vector2 leftPoint, Vector2 rightPoint)
    {
        SetArmPositionType(ArmPositionType.SideSlam);
        StartCoroutine(PointArmAtPosition(leftArm, leftPoint));
        StartCoroutine(PointArmAtPosition(rightArm, rightPoint));

        float leftArmAngle = CalculateTargetAngle(transform.position, leftPoint);
        float rightArmAngle = CalculateTargetAngle(transform.position, rightPoint);
        float lookAtAngle = (leftArmAngle + rightArmAngle) / 2 + 90;
        bossMainAnimator.LookAtAngle(lookAtAngle);
    }

    public void RunMidSlamAnimation(Vector2 point)
    {
        SetArmPositionType(ArmPositionType.MidSlam);
        StartCoroutine(PointArmAtPosition(leftArm, point));
        StartCoroutine(PointArmAtPosition(rightArm, point));

        float angle = CalculateTargetAngle(transform.position, point) + 90;
        bossMainAnimator.LookAtAngle(angle);
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
        var leftFist = leftArmOffsetContainer.transform.GetChild(1).gameObject;
        var rightFist = rightArmOffsetContainer.transform.GetChild(1).gameObject;

        leftFist.transform.GetChild(0).gameObject.SetActive(height == FistHeight.Bottom);
        rightFist.transform.GetChild(0).gameObject.SetActive(height == FistHeight.Bottom);
        leftFist.transform.GetChild(1).gameObject.SetActive(height == FistHeight.Mid);
        rightFist.transform.GetChild(1).gameObject.SetActive(height == FistHeight.Mid);
        leftFist.transform.GetChild(2).gameObject.SetActive(height == FistHeight.Top);
        rightFist.transform.GetChild(2).gameObject.SetActive(height == FistHeight.Top);
    }

    private IEnumerator MidSlamSequence() {
        this.armMoveDuration = this.midArmAimDuration;

        var playerPosition = player.transform.position;
        RunMidSlamAnimation(playerPosition);

        yield return new WaitForSeconds(this.midArmAimDuration);
    }
}