using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public GameObject followPos;
    private GameObject player;
    private bool following = false;
    private List<Vector2> storedPosition;
    private List<GameObject> villagerList;
    private Rigidbody2D rb;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        villagerList = player.GetComponent<PlayerController>().villagers;
        storedPosition = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        dead = false;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!following && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            villagerList.Add(this.gameObject);
            following = true;
        }
        if (following)
        {
            storedPosition.Add(transform.position);

            if (storedPosition.Count > 30)
            {
                followPos.transform.position = storedPosition[0];
                storedPosition.RemoveAt(0);
            }
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
            if (villagerList.IndexOf(this.gameObject) == 0)
            {
                transform.position = (player.transform.GetChild(0).position - new Vector3(0,0.65f, 0));
            }
            else
            {
                transform.position = villagerList[villagerList.IndexOf(this.gameObject) - 1].transform.GetChild(0).position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            villagerList.RemoveAt(villagerList.IndexOf(this.gameObject));
            dead = true;
            GetComponent<Collider2D>().enabled = false;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(0, 500));
        }
    }
}
