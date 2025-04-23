using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryArea : MonoBehaviour
{
    [SerializeField]
    EnemyDiscoveryController[] enemy;//特定の発見フラグを持つ敵だけ格納

    /// <summary>
    /// プレイヤーが一定範囲内に入った時にEnemyの移動を設定
    /// </summary>
    /// <param name="other">コライダ</param>
    private void OnTriggerStay(Collider other)
    {
        //TagがPlayerの場合
        if (other.gameObject.tag == "Player")
        {
            //再度起動しないように非表示
            this.gameObject.SetActive(false);
            //格納してある敵に発見を送信
            foreach (EnemyDiscoveryController enemies in enemy)
            {
                //先に撃破されている場合を除外する
                if (enemies != null)
                    enemies.IsDiscobery();//発見を送信
            }
        }
    }
}
