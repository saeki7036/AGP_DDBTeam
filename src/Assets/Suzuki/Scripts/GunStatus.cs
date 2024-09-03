using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunStatus : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    int remainBullets;

    public int RemainBullets
    {
        get { return remainBullets; }
    }

    void Start()
    {
        FillBullet();
    }
    /// <summary>
    /// 銃から発射方向を向いた銃弾を生成する
    /// </summary>
    /// <param name="infiniteBullet">弾を消費させるかどうか</param>
    /// <returns>撃てたかどうかを返す</returns>
    public bool Shoot(Vector3 position, Vector3 forward, string tag, bool infiniteBullet)
    {
        if(remainBullets <= 0 && !infiniteBullet) return false;
        if(!infiniteBullet) remainBullets--;
        
        GameObject bullet = Instantiate(weaponData.BulletPrefab, position, Quaternion.identity);
        bullet.tag = tag;
        bullet.transform.forward = forward;

        if(remainBullets == 0 && weaponData.Role == WeaponData.WeaponRole.main)
        {
            ChangeWeapon();
        }
        return true;
    }

    void FillBullet()
    {
        remainBullets = weaponData.MaxBullet;
    }

    void ChangeWeapon()
    {
        if (weaponData.SubWeapon != null)
        {
            Debug.Log("サブ武器を使う！");
            Transform parent = this.transform.parent;
            Instantiate(weaponData.SubWeapon, parent);// 武器の生成
            PlayerMove playerMove = parent.GetComponentInChildren<PlayerMove>();

            this.transform.SetParent(null);// 親子付けを外す
            playerMove.SetGunObject();// 持っている銃の設定
            Destroy(this.gameObject);
        }
    }
}
