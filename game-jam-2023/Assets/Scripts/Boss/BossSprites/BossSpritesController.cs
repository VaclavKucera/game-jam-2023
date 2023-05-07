using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BossSpritesController : MonoBehaviour
{
    public enum Arm { Left, Right }
    public enum ArmPositionType { Default, SideSlam, MidSlam }
    public enum FistHeight { Top, Mid, Bottom }

    public float floatSpeed = 3.0f;
    public float floatMagnitude = 0.03f;
    public float rotationSpeed = 5.0f;

    public BossController bossController;
    public BossArmsController armsController;
    private GameObject skullSprite;
    private GameObject arms;
    private Vector3 originalScale;

    private float currentTime;
    private float targetAngle;
    private bool isIdleMovementActive = true;

    void Start()
    {
        if (bossController == null)
            bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        else
            bossController = bossController.GetComponent<BossController>();

        skullSprite = transform.GetChild(0).gameObject;
        arms = transform.GetChild(1).gameObject;
        armsController = arms.GetComponent<BossArmsController>();

        originalScale = skullSprite.transform.localScale;
        currentTime = 0.0f;
        targetAngle = skullSprite.transform.eulerAngles.z;
    }

    void Update()
    {
        // Floating up and down
        if (isIdleMovementActive) {
            currentTime += Time.deltaTime;
            float newScale = originalScale.y + Mathf.Sin(currentTime * floatSpeed) * floatMagnitude;
            skullSprite.transform.localScale = new Vector3(newScale, newScale, originalScale.z);
            arms.transform.localScale = new Vector3(newScale, newScale, originalScale.z);
        }

        // Smooth rotation
        float currentAngle = skullSprite.transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        skullSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
    }

    // Skull animation
    public void LookAtAngle(float angle)
    {
        targetAngle = angle;
    }

    public void LookAtPoint(Vector2 targetPoint)
    {
        float angle = Mathf.Atan2(targetPoint.y, targetPoint.x) * Mathf.Rad2Deg;
        LookAtAngle(angle);
    }

    public void SetIdleMovement(bool active)
    {
        isIdleMovementActive = active;
    }

    // Arm animations
    public IEnumerator PointArmAtPosition(Arm arm, Vector2 targetPoint)
    {
       yield return StartCoroutine(armsController.PointArmAtPosition(arm, targetPoint));
    }

    public IEnumerator PointArmAtRotation(Arm arm, Quaternion targetRotation)
    {
        yield return StartCoroutine(armsController.PointArmAtRotation(arm, targetRotation));
    }

    public void SetFistHeight(FistHeight height) {
        armsController.SetFistHeight(height);
    }

    public void SetArmPositionType(ArmPositionType type) {
        armsController.SetArmPositionType(type);
    }

    public void SetArmMoveDuration(float duration) {
        armsController.armMoveDuration = duration;
    }

    public float GetArmMoveDuration() {
        return armsController.armMoveDuration;
    }
}
