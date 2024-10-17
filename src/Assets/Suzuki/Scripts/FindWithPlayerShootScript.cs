using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWithPlayerShootScript : MonoBehaviour
{
    [SerializeField] EnemyChaseController enemyChaseController;
    [SerializeField] PlayerRay playerRay;
    [SerializeField] PlayerMove player;
    [SerializeField] float findableRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (playerRay.Shoot)
        {
            DetectPlayerShoot();
        }
    }
    void DetectPlayerShoot()
    {
        float distanceSquare = (player.transform.position - transform.position).sqrMagnitude;
        if(distanceSquare <= findableRange * findableRange && !enemyChaseController.enabled)
        {
            enemyChaseController.enabled = true;
        }
    }
}
