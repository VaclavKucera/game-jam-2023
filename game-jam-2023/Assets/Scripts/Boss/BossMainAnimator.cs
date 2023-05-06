using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainAnimator : MonoBehaviour
{
    public float floatSpeed = 3.0f;
    public float floatMagnitude = 0.03f;
    public float rotationSpeed = 5.0f;

    private GameObject firstChild;
    private Vector3 originalScale;
    private float currentTime;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        firstChild = transform.GetChild(0).gameObject;
        originalScale = firstChild.transform.localScale;
        currentTime = 0.0f;
        targetAngle = firstChild.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Floating up and down
        currentTime += Time.deltaTime;
        float newScale = originalScale.y + Mathf.Sin(currentTime * floatSpeed) * floatMagnitude;
        firstChild.transform.localScale = new Vector3(newScale, newScale, originalScale.z);

        // Smooth rotation
        float currentAngle = firstChild.transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        firstChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
    }

    public void LookAtAngle(float angle)
    {
        targetAngle = angle;
    }
}
