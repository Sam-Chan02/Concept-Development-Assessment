using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public InputActionAsset inputController;
    public List<GameObject> villagers = new List<GameObject>();
    public Camera cam;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;
    public float airborneAcceleration;
    public float airborneDeceleration;
    public float jumpForce;
    public GameObject followPos;
    public int health = 3;
    public int lives = 5;
    public Vector2 spawn = new Vector2(0f, 0f);
    public int coins;

    private List<Vector2> storedPosition;
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
        transform.position = spawn;
        storedPosition = new List<Vector2>();
        coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(velocity);
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
            if (airborne) { velocity += airborneAcceleration * direction; }
            else { velocity += acceleration * direction; }
            if (velocity > maxSpeed)
            {
                velocity = maxSpeed;
            }
            else if (velocity < -maxSpeed)
            {
                velocity = -maxSpeed;
            }
        }
        else if (Mathf.Abs(velocity) > 0)
        {
            if (airborne) { velocity -= airborneDeceleration * direction; }
            else { velocity -= deceleration * direction; }
            if ((!airborne && Mathf.Abs(velocity) < deceleration/2) || (airborne && Mathf.Abs(velocity) < airborneDeceleration / 2))
            {
                velocity = 0;
            }
        }
        rb.velocity = new Vector2(velocity, rb.velocity.y);
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
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }

    private void OnCrouch()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y < 0.9f)
            {
                if (collision.transform.tag == "Enemy")
                {
                    health--;
                    if (health <= 0)
                    {
                        lives--;
                        if (lives <= 0)
                        {
                            transform.position = spawn;
                            velocity = 0;
                            health = 3;
                        }
                    }
                }
            }
            if (Mathf.Abs(contact.normal.x) > 0.9f)
            {
                velocity = 0;
            }
            else
            {
                airborne = false;
                jumps = 1;
            }
        }
    }
}
