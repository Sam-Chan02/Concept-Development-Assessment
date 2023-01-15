using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheckerController : MonoBehaviour
{
    private EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            enemyController.direction = -enemyController.direction;
        }
    }
}
