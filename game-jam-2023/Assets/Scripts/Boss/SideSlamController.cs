using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSlamController : MonoBehaviour
{
    private GameObject leftArm;
    private GameObject rightArm;
    private BossMainAnimator bossMainAnimator;

    // Start is called before the first frame update
    void Start()
    {
        leftArm = transform.GetChild(0).gameObject;
        rightArm = transform.GetChild(1).gameObject;
        bossMainAnimator = transform.parent.GetComponent<BossMainAnimator>();
    }

    public void Activate(Vector2 leftPoint, Vector2 rightPoint)
    {
        RotateArmToPoint(leftArm, leftPoint);
        RotateArmToPoint(rightArm, rightPoint);

        // Calculate angle between left and right points
        float angle = Vector2.Angle(leftPoint - (Vector2)transform.position, rightPoint - (Vector2)transform.position) / 2;

        // Call LookAtAngle method of BossMainAnimator
        if (bossMainAnimator != null)
        {
            bossMainAnimator.LookAtAngle(angle + 90);
        }

        // Activate SpriteCycler and FistSlamController scripts for both arms
        ActivateChildScripts(leftArm);
        ActivateChildScripts(rightArm);
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        // {
        //     Vector2 leftPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Vector2 rightPoint = new Vector2(10, 0);
        //     Activate(leftPoint, rightPoint);
        // }
    }

    private void RotateArmToPoint(GameObject arm, Vector2 point)
    {
        Vector2 armPosition = arm.transform.position;
        Vector2 parentPosition = transform.position;
        Vector2 direction = point - parentPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        // Calculate new position for the arm
        float distanceFromParent = Vector3.Distance(parentPosition, armPosition);
        Vector3 newPosition = parentPosition + direction.normalized * distanceFromParent;

        // Apply rotation and position updates
        arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        arm.transform.position = newPosition;
    }

    private void ActivateChildScripts(GameObject arm)
    {
        // Activate SpriteCycler script
        SpriteCycler spriteCycler = arm.transform.GetChild(0).GetComponent<SpriteCycler>();
        if (spriteCycler != null)
        {
            spriteCycler.active = true;
        }

        // Activate FistSlamController script
        FistSlamController fistSlamController = arm.transform.GetChild(1).GetComponent<FistSlamController>();
        if (fistSlamController != null)
        {
            fistSlamController.Activate();
        }
    }
}