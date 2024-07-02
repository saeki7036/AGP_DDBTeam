using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    /// <summary>
    /// 銃から発射方向を向いた銃弾を生成する
    /// </summary>
    /// <param name="bullet">キャラクターの種類ごとに設定されたBulletプレハブを渡す</param>
    /// <param name="position">発射元の位置</param>
    /// <param name="direction">発射先の方向</param>
    /// <param name="remainBullets">残弾数</param>
    /// <returns></returns>
    public bool Shoot(GameObject bulletPrefab, Vector3 position, Vector3 direction, int remainBullets)
    {
        if(remainBullets <= 0) return false;
        remainBullets--;
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.transform.LookAt(direction);
        return true;
    }
}
