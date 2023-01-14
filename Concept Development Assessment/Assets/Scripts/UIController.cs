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

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
    }
}
