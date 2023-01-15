using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Image blocker;
    public Text loseText;
    public Text winText;
    public Button restartButton;
    private bool lostYet;
    private bool wonYet;

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
        blocker.enabled = false;
        loseText.enabled = false;
        winText.enabled = false;
        restartButton.gameObject.SetActive(false);
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

        if (playerController.won)
        {
            if (!wonYet)
            {
                wonYet = true;
                blocker.enabled = true;
                winText.enabled = true;
                restartButton.gameObject.SetActive(true);
                blocker.GetComponent<CanvasGroup>().alpha = 0;
                winText.GetComponent<CanvasGroup>().alpha = 0;
                restartButton.GetComponent<CanvasGroup>().alpha = 0;
            }
            blocker.GetComponent<CanvasGroup>().alpha += 0.01f;
            winText.GetComponent<CanvasGroup>().alpha += 0.01f;
            restartButton.GetComponent<CanvasGroup>().alpha += 0.01f;
        }
        if (playerController.lost)
        {
            if (!lostYet)
            {
                lostYet = true;
                blocker.enabled = true;
                loseText.enabled = true;
                restartButton.gameObject.SetActive(true);
                blocker.GetComponent<CanvasGroup>().alpha = 0;
                loseText.GetComponent<CanvasGroup>().alpha = 0;
                restartButton.GetComponent<CanvasGroup>().alpha = 0;
            }
            blocker.GetComponent<CanvasGroup>().alpha += 0.1f;
            loseText.GetComponent<CanvasGroup>().alpha += 0.1f;
            restartButton.GetComponent<CanvasGroup>().alpha += 0.1f;
        }
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

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
