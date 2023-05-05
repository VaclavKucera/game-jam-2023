using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.3f;
    public float dashRecoveryDuration = 1f;

    private float moveInputHorizontal;
    private float moveInputVertical;
    private Rigidbody2D rb;
    private bool dashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInputHorizontal = Input.GetAxis("Horizontal");
        moveInputVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            Debug.Log("Dash");
            dashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }

        var speed = dashing ? dashSpeed : moveSpeed;

        rb.velocity = new Vector2(moveInputHorizontal * speed, moveInputVertical * speed);
    }

    IEnumerator Dash()
    {
        var dashTime = dashDuration;
        while (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
            yield return null;
        }
        dashing = false;

        var dashRecoveryTime = dashRecoveryDuration;
        while (dashRecoveryTime > 0)
        {
            dashRecoveryTime -= Time.deltaTime;
            yield return null;
        }

        canDash = true;
    }
}