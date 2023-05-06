using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainAnimator : MonoBehaviour
{
    public GameObject StaticTelegraph;
    public GameObject OnPlayerTelegraph;
    public BossController BossController;
    public float floatSpeed = 3.0f;
    public float floatMagnitude = 0.03f;
    public float rotationSpeed = 5.0f;

    private GameObject skullSprite;
    private GameObject arms;
    private BossArmsController armsController;
    private Vector3 originalScale;
    private float currentTime;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        if (BossController == null)
        {
            BossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
        }
        else BossController = BossController.GetComponent<BossController>();

        skullSprite = transform.GetChild(0).gameObject;
        arms = transform.GetChild(1).gameObject;
        armsController = arms.GetComponent<BossArmsController>();
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

    public void SlamAttack()
    {
        BossController.isAttacking = true;
        StartCoroutine(SlamAttackCoroutine());
    }
    IEnumerator SlamAttackCoroutine()
    {
        armsController.RunMidSlamAnimation();
        yield return new WaitForSeconds(0.5f);
        armsController.RunMidSlamAnimation();
        yield return new WaitForSeconds(0.5f);
        armsController.RunMidSlamAnimation();
        yield return new WaitForSeconds(0.5f);
        BossController.onAutoattackAnimationComplete();
    }
    public void GroundPoundAttack(Vector3 left, Vector3 right)
    {
        BossController.isAttacking = true;
        StartCoroutine(GroundPoundAttackCoroutine(left, right));
    }
    IEnumerator GroundPoundAttackCoroutine(Vector3 left, Vector3 right)
    {
        var firstTG = Instantiate(StaticTelegraph, left, Quaternion.identity);
        firstTG.GetComponent<TelegraphController>().setTimer(2);
        var secondTG = Instantiate(StaticTelegraph, right, Quaternion.identity);
        secondTG.GetComponent<TelegraphController>().setTimer(2);

        armsController.RunSideSlamAnimation(left, right);
        yield return new WaitForSeconds(1.5f);
        BossController.onAutoattackAnimationComplete();
    }


}
