using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public GameObject player;
    private bool collected;
    private Vector2 startLocation;
    private float frame;
    private float opacity;
    private SpriteRenderer r;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        r = GetComponent<SpriteRenderer>();
        startLocation = transform.position;
        frame = startLocation.x * 30 % 360;
        opacity = 1f;
    }

    private void FixedUpdate()
    {
        if (!collected)
        {
            transform.position = new Vector2(startLocation.x, startLocation.y + Mathf.Sin(Mathf.Deg2Rad * frame) / 15);
            frame = (frame + 7.5f) % 360;
        }
        else
        {
            transform.Translate(new Vector2(-0.2f, 0.5f));
            opacity -= 0.1f;
            r.color = new Color(1f, 1f, 1f, opacity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !collected)
        {
            collected = true;
            player.GetComponent<PlayerController>().coins++;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Die", 0.2f);
        }
    }

    private void Die()
    {
        GameObject.Destroy(this.gameObject);
    }
}
