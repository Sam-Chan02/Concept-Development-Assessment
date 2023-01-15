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
    public Sprite heartVillager;
    public Sprite lifeVillager;
    public Sprite jumpVillager;
    public string power = "Heart";
    private PlayerController playerController;
    public float jumpForceUpgrade = 50;
    public float jumpHoldForceUpgrade = 10;
    public Canvas UI;
    private UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
        uiController = UI.GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        villagerList = playerController.villagers;
        storedPosition = new List<Vector2>();
        storedFacing = new List<bool>();
        sr = GetComponent<SpriteRenderer>();
        storedFace = sr.flipX;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        dead = false;
        villagerDelay = playerController.villagerDelay;
        Physics2D.IgnoreCollision(player.GetComponents<Collider2D>()[0], GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(player.GetComponents<Collider2D>()[1], GetComponent<Collider2D>());
        if (power == "Health")
        {
            sr.sprite = heartVillager;
        }
        else if (power == "Life")
        {
            sr.sprite = lifeVillager;
        }
        else
        {
            sr.sprite = jumpVillager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!following && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            villagerList.Add(this.gameObject);
            following = true;
            if (power == "Health")
            {
                playerController.maxHealth += 1;
                playerController.health += 1;
            }
            else if (power == "Life")
            {
                playerController.lives += 1;
            }
            else
            {
                playerController.jumpForce += jumpForceUpgrade;
                playerController.jumpHoldForce += jumpHoldForceUpgrade;
            }
            uiController.PowerStart(power);
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
                if (villagerList[villagerList.IndexOf(this.gameObject) - 1] != null)
                {

                    transform.position = villagerList[villagerList.IndexOf(this.gameObject) - 1].transform.GetChild(0).position;
                    sr.flipX = villagerList[villagerList.IndexOf(this.gameObject) - 1].GetComponent<VillagerController>().storedFace;
                }
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if (following && !dead)
            {
                if (power == "Heart")
                {
                    playerController.maxHealth -= 1;
                }
                else if (power == "Life")
                {
                    playerController.lives -= 1;
                }
                else
                {
                    playerController.jumpForce -= jumpForceUpgrade;
                    playerController.jumpHoldForce -= jumpHoldForceUpgrade;
                }
                uiController.PowerEnd(power);

            }
            dead = true;
            GetComponent<Collider2D>().enabled = false;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(0, 500));
            villagerList.Remove(this.gameObject);
        }
    }
}
