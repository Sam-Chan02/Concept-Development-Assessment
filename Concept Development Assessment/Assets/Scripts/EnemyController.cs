using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;
    private Rigidbody2D rb;
    public float enemySpeed = 2;
    public float direction = -1;
    private SpriteRenderer sr;
    public Camera cam;
    private bool active;
    public GameObject coin;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0 && active && !dead)
        {
            dead = true;
            Instantiate(coin, new Vector2(transform.position.x, transform.position.y+2), Quaternion.identity);
            GetComponent<Collider2D>().enabled = false;
            Invoke("Die", 0.5f);
        }
        if (Mathf.Abs(transform.position.x - cam.transform.position.x) < 10)
        {
            active = true;
        }
        if (active && Mathf.Abs(transform.position.x - cam.transform.position.x) > 10)
        sr.flipX = direction > 0;
    }

    private void FixedUpdate()
    {
        if (active && !dead)
        {
            rb.velocity = new Vector2(enemySpeed * direction, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && active && !dead)
        {
            direction = -direction;
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}
