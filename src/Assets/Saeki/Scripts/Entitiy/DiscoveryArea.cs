using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryArea : MonoBehaviour
{
    [SerializeField]
    EnemyDiscoveryController[] enemy;
    /// <summary>
    /// プレイヤーが一定範囲内に入った時にEnemyの移動を設定
    /// </summary>
    /// <param name="other">コライダ</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //再度起動しないように非表示
            this.gameObject.SetActive(false);

            foreach (EnemyDiscoveryController enemies in enemy)
            {
                //先に撃破されている場合を除外
                if (enemies != null)
                    enemies.IsDiscobery();
            }
        }
    }
}
