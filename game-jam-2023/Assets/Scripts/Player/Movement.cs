using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dashSpeed = 4f;
    public float dashDuration = 0.3f;
    public float dashRecoveryDuration = 0.5f;
    public float stamina = 100;
    public float maxStamina = 100;
    public float staminaRegen = 0.5f;

    private float moveInputHorizontal;
    private float moveInputVertical;
    private Rigidbody2D rb;
    private bool dashing;
    private bool canDash = true;
    private Camera mainCamera;

    private const int dashCost = 50;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (stamina < maxStamina + staminaRegen)
        {
            stamina += staminaRegen;
        }

        moveInputHorizontal = Input.GetAxis("Horizontal");
        moveInputVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && canDash && stamina >= dashCost)
        {
            Debug.Log("Dash");
            StartCoroutine(Dash());
        }

        var speed = dashing ? dashSpeed : moveSpeed;

        rb.velocity = new Vector2(moveInputHorizontal * speed, moveInputVertical * speed);
        UpdateRotation();
    }

    private IEnumerator Dash()
    {
        stamina -= dashCost;
        dashing = true;
        canDash = false;
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

    private void UpdateRotation()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var position = transform.position;
        var angle = Mathf.Atan2(mousePosition.y - position.y, mousePosition.x - position.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;
    }
}