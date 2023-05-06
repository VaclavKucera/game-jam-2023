using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothTime = 0.1f;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            player.position + offset,
            ref currentVelocity,
            smoothTime
        );
    }
}
