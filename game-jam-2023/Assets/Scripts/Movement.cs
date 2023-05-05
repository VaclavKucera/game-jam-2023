using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashSpeed = 15.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var moveInputHorizontal = Input.GetAxis("Horizontal");
        var moveInputVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space)) Debug.Log("Space");

        var speed = Input.GetKey(KeyCode.Space) ? dashSpeed : moveSpeed;

        rb.velocity = new Vector2(moveInputHorizontal * speed, moveInputVertical * speed);
    }
}