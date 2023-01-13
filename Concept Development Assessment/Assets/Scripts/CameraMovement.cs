using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    private Vector3 distance;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        distance = player.position - transform.position;
        if (distance.x > 0)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
        if (distance.y > 0)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        if (distance.y + 2 < 0 && transform.position.y > 2.1)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + 2, transform.position.z);
        }
    }
}
