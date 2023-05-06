using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainAnimator : MonoBehaviour
{
    public float floatSpeed = 3.0f;
    public float floatMagnitude = 0.03f;
    public float rotationSpeed = 5.0f;

    private GameObject skullSprite;
    private GameObject arms;
    private Vector3 originalScale;
    private float currentTime;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        skullSprite = transform.GetChild(0).gameObject;
        arms = transform.GetChild(1).gameObject;
        originalScale = skullSprite.transform.localScale;
        currentTime = 0.0f;
        targetAngle = skullSprite.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Floating up and down
        currentTime += Time.deltaTime;
        float newScale = originalScale.y + Mathf.Sin(currentTime * floatSpeed) * floatMagnitude;
        skullSprite.transform.localScale = new Vector3(newScale, newScale, originalScale.z);
        arms.transform.localScale = new Vector3(newScale, newScale, originalScale.z);

        // Smooth rotation
        float currentAngle = skullSprite.transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        skullSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
    }

    public void LookAtAngle(float angle)
    {
        targetAngle = angle;
    }
    public void LookAtPoint(Vector2 targetPoint)
    {
        float angle = Mathf.Atan2(targetPoint.y, targetPoint.x) * Mathf.Rad2Deg;
        LookAtAngle(angle);
    }
}
