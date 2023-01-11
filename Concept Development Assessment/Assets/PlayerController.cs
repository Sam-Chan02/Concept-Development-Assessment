using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public InputActionAsset inputController;
    public List<GameObject> villagers = new List<GameObject>();
    public float maxSpeed = 0.2f;
    public float acceleration = 0.04f;
    public GameObject followPos;
    private List<Vector2> storedPosition;
    private InputAction moveInput;
    private Rigidbody2D rb;
    private float velocity = 0;
    private float moveDirection;
    private float direction;
    private int jumps = 1;
    private bool airborne = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = inputController.FindAction("Move");
        transform.position = new Vector2(0f, 0f);
        storedPosition = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        storedPosition.Add(transform.position);

        if(storedPosition.Count > 30)
        {
            followPos.transform.position = storedPosition[0];
            storedPosition.RemoveAt(0);
        }
    }

    private void FixedUpdate()
    { 
        if (moveDirection != 0)
        {
            direction = moveDirection;
            velocity += acceleration * direction;
            if (Math.Abs(velocity) > maxSpeed)
            {
                velocity = maxSpeed * direction;
            }
        }
        else if (Math.Abs(velocity) > 0)
        {
            velocity -= acceleration * direction;
            if (Math.Abs(velocity) < 0.02)
            {
                velocity = 0;
            }
        }
        transform.Translate(new Vector2(velocity, 0));
    }

    void OnMove(InputValue direction)
    {
        moveDirection = direction.Get<float>();
    }

    void OnJump()
    {
        if (jumps > 0)
        {
            jumps--;
            airborne = true;
            rb.AddForce(new Vector2(0, 300));
        }
    }

    private void OnCrouch()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        airborne = false;
        jumps = 1;
    }
}
