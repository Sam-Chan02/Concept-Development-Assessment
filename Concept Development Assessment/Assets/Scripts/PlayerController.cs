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
    public float groundedAcceleration;
    public float groundedDeceleration;
    public float airborneAcceleration;
    public float airborneDeceleration;
    public float jumpForce;
    public float jumpHoldForce;
    public GameObject followPos;
    public int maxHealth = 3;
    public int health = 3;
    public int lives = 5;
    public Vector2 spawn = new Vector2(0f, 0f);
    public int coins;
    public bool storedFace;
    public int villagerDelay = 15;
    public bool won = false;
    public bool lost = false;

    private List<bool> storedFacing;
    private List<Vector2> storedPosition;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private InputAction jumpHold;
    private float speed = 0;
    private float acceleration;
    private float deceleration;
    private float moveDirection;
    private float direction;
    private float cmpDirection;
    private int jumps = 1;
    private float jumpLength = 0.2f;
    private float jumpTimer = 0.2f;
    private bool airborne = false;
    private float inVulnLength = 0.5f;
    private float inVulnTimer = 0.5f;
    private bool inVuln = false;
    private bool hit;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        storedPosition = new List<Vector2>();
        storedFacing = new List<bool>();
        storedFace = sr.flipX;
        jumpHold = inputController.FindAction("Jump");
        transform.position = spawn;
        coins = 0;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (inVuln)
        {
            inVulnTimer -= Time.deltaTime;
            if (inVulnTimer < 0)
            {
                inVuln = false;
                inVulnTimer = inVulnLength;
            }
        }
        if (player.transform.position.y < -3 && !dead && !won)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        if (!won)
        {
            hit = false;
            if (moveDirection != 0)
            {
                cmpDirection = direction * moveDirection;
                if (speed == 0 && cmpDirection <= 0)
                {
                    direction = moveDirection;
                    cmpDirection = 1;
                }

                if (airborne) { acceleration = airborneAcceleration; }
                else { acceleration = groundedAcceleration; }
                speed += acceleration * cmpDirection;

            }
            else if (speed > 0)
            {
                if (airborne) { deceleration = airborneDeceleration; }
                else { deceleration = groundedDeceleration; }
                speed -= deceleration;
            }

            speed = Mathf.Clamp(speed, 0, maxSpeed);
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);


            if (jumpHold.ReadValue<float>() > 0 && jumpTimer > 0)
            {
                rb.AddForce(new Vector2(0, jumpHoldForce));
            }
            jumpTimer = Mathf.Clamp(jumpTimer - Time.deltaTime, 0, jumpLength);
        }
        storedPosition.Add(transform.position);
        storedFacing.Add(sr.flipX);

        if (storedPosition.Count > villagerDelay)
        {
            followPos.transform.position = storedPosition[0];
            storedPosition.RemoveAt(0);
            storedFace = storedFacing[0];
            storedFacing.RemoveAt(0);
        }
    }

    void OnMove(InputValue direction)
    {
        moveDirection = direction.Get<float>();
        if (moveDirection != 0)
        {
        sr.flipX = (moveDirection < 0);
        }
    }

    void OnJump()
    {
        if (jumps > 0)
        {
            jumps--;
            airborne = true;
            jumpTimer = jumpLength;
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
            if (Mathf.Abs(contact.normal.x) > 0.9f)
            {
                speed = 0;
            }
            else
            {
                if (contact.collider.tag == "Enemy")
                {
                    contact.collider.GetComponentInParent<EnemyController>().health -= 1;
                    if (jumpHold.ReadValue<float>() > 0 && !hit)
                    {
                        rb.AddForce(new Vector2(0, jumpForce * 1.5f));
                    }
                    else if (!hit)
                    {
                        rb.AddForce(new Vector2(0, 400));
                    }
                    hit = true;

                }
                else
                {
                    jumps = 1;
                    airborne = false;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y < 0.9f)
            {
                if (contact.collider.tag == "Enemy" && !inVuln)
                {
                    inVuln = true;
                    health--;
                    if (health <= 0)
                    {
                        Death();
                    }
                }
            }
        }

    }

    private void Death()
    {
        rb.velocity = new Vector2(0, 20);
        dead = true;
        Invoke("Die", 1f);
    }
    private void Die()
    {
        if (lives < 0)
        {
            transform.position = spawn;
            cam.transform.position = new Vector3(spawn.x + 7, spawn.y + 2.2f, cam.transform.position.z);
            speed = 0;
            rb.velocity = new Vector2(0, 0);
            health = 3;
        }
        else
        {
            Lose();
        }
    }

    public void Win()
    {
        rb.velocity = new Vector2(10, 0);
        won = true;
    }

    private void Lose()
    {
        lost = true;
    }
}
