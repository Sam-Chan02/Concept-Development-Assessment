using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public GameObject followPos;
    public bool storedFace;
    private GameObject player;
    private bool following = false;
    private List<Vector2> storedPosition;
    private List<bool> storedFacing;
    private List<GameObject> villagerList;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool dead;
    private int villagerDelay;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        villagerList = player.GetComponent<PlayerController>().villagers;
        storedPosition = new List<Vector2>();
        storedFacing = new List<bool>();
        sr = GetComponent<SpriteRenderer>();
        storedFace = sr.flipX;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        dead = false;
        villagerDelay = player.GetComponent<PlayerController>().villagerDelay;
        Physics2D.IgnoreCollision(player.GetComponents<Collider2D>()[0], GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(player.GetComponents<Collider2D>()[1], GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!following && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            villagerList.Add(this.gameObject);
            following = true;
        }
        if (transform.position.y < -8 && dead)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (following && !dead)
        {
            storedPosition.Add(transform.position);
            storedFacing.Add(sr.flipX);

            if (storedPosition.Count > villagerDelay)
            {
                followPos.transform.position = storedPosition[0];
                storedPosition.RemoveAt(0);
                storedFace = storedFacing[0];
                storedFacing.RemoveAt(0);
            }

            if (villagerList.IndexOf(this.gameObject) == 0)
            {
                transform.position = (player.transform.GetChild(0).position);
                sr.flipX = player.GetComponent<PlayerController>().storedFace;
            }
            else
            {
                transform.position = villagerList[villagerList.IndexOf(this.gameObject) - 1].transform.GetChild(0).position;
                sr.flipX = villagerList[villagerList.IndexOf(this.gameObject) - 1].GetComponent<VillagerController>().storedFace;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            villagerList.RemoveAt(villagerList.IndexOf(this.gameObject));
            dead = true;
            GetComponent<Collider2D>().enabled = false;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(0, 500));
        }
    }
}
