using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerController playerController;
    public Image[] hearts;
    public Sprite full;
    public Sprite empty;
    public Text coins;
    public string coinString;
    public Text healthUp;
    public Text jumpUp;
    public Text lifeUp;
    public Text healthDown;
    public Text jumpDown;
    public Text lifeDown;
    public Text lives;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthUp.enabled = false;
        jumpUp.enabled = false;
        lifeUp.enabled = false;
        healthDown.enabled = false;
        jumpDown.enabled = false;
        lifeDown.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerController.health)
            {
                hearts[i].GetComponent<Image>().sprite = full;
            }
            else
            {
                hearts[i].GetComponent <Image>().sprite = empty;
            }

            if (i < playerController.maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        coinString = playerController.coins.ToString();
        if (coinString.Length < 2)
        {
            coins.text = "00" + coinString;
        }
        else if (coinString.Length < 3)
        {
            coins.text = "0" + coinString;
        }
        lives.text = playerController.lives.ToString();
    }

    public void PowerStart(string power)
    {
        if (power == "Health")
        {
            healthUp.enabled = true;
            Invoke("disableHealth", 0.5f);
        }
        else if (power == "Life")
        {
            lifeUp.enabled = true;
            Invoke("disableLife", 0.5f);
        }
        else if (power == "Jump")
        {
            jumpUp.enabled = true;
            Invoke("disableJump", 0.5f);
        }
    }
    public void PowerEnd(string power)
    {
        if (power == "Health")
        {
            healthDown.enabled = true;
            Invoke("disableHealthDown", 0.5f);
        }
        else if (power == "Life")
        {
            lifeDown.enabled = true;
            Invoke("disableLifeDown", 0.5f);
        }
        else if (power == "Jump")
        {
            jumpDown.enabled = true;
            Invoke("disableJumpDown", 0.5f);
        }
    }

    void disableHealth()
    {
        healthUp.enabled = false;
    }
    void disableLife()
    {
        lifeUp.enabled = false;
    }
    void disableJump()
    {
        jumpUp.enabled=false;
    }
    void disableHealthDown()
    {
        healthDown.enabled = false;
    }
    void disableLifeDown()
    {
        lifeDown.enabled = false;
    }
    void disableJumpDown()
    {
        jumpDown.enabled = false;
    }
}
