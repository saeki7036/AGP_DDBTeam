using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EreaOfDiscovery : MonoBehaviour
{
    [SerializeField]
    EnemyDiscoveryController[] enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (EnemyDiscoveryController enemies in enemy)
            {
                enemies.IsDiscobery();
            }
            this.gameObject.SetActive(false);
        }
    }
}
