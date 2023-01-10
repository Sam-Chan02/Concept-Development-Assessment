using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float maxSpeed = 0.2f;
    public float acceleration = 0.02f;
    private Rigidbody2D rb;
    private float moveDirection;
    private float speedMag = 0;
    private float speed = 0;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moving);
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(speed, 0));
    }

    void OnMove(InputValue direction)
    {
        moving = true;
        moveDirection = direction.Get<float>();
        speedMag += acceleration;
        if (speedMag > maxSpeed)
        {
            speedMag = maxSpeed;
        }
        speed = speedMag * moveDirection;
    }

    void OnJump()
    {

    }

    private void OnCrouch()
    {
        
    }
}
