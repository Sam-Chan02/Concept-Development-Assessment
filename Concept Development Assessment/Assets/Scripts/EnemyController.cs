using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;
    private Rigidbody2D rb;
    public float enemySpeed = 2;
    private float direction = 1;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(enemySpeed * direction, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
        {
            sr.flipX = direction < 0;
            direction = -direction;
        }
    }
}
